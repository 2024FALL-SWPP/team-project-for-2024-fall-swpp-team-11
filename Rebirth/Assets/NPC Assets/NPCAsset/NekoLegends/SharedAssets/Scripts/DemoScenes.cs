using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace NekoLegends
{
    public class DemoScenes : MonoBehaviour
    {
        [SerializeField] protected Light directionalLight;
        [SerializeField] protected List<CameraDOFData> CameraDOFDatas;
        [SerializeField] protected List<CameraData> CameraDatas;
        [SerializeField] protected Transform BGTransform;

        [SerializeField] protected List<Transform> TargetPositions; //for game objects
        [SerializeField] protected Button GlobalVolumnBtn;
        [SerializeField] protected Volume GlobalVolume;
        [SerializeField] protected Button LogoBtn;
        [SerializeField] public TextMeshProUGUI DescriptionText;
        [SerializeField] protected GameObject DemoUI;
        [SerializeField] protected bool hideMouse = false;

        
        [SerializeField] public AudioSource BGMSource, SFX;

        private int currentIndex = 0;
        private bool isAnimating = false;
        protected float transitionSpeed = 1.0f;
        protected int currentCameraIndex = 0;
        protected bool isTransitioning = false;
        private GameObject lightObject;
        private DepthOfField _depthOfField;
        protected Dictionary<Button, UnityAction> buttonActions = new Dictionary<Button, UnityAction>();

        public bool isShowOutlines;


        protected const string publisherSite = "https://assetstore.unity.com/publishers/82927";
        protected const string WebsiteURL = "https://nekolegends.com";
        

        [System.Serializable]
        public struct CameraDOFData //manual camera dof settings
        {
            public Transform CameraAngle;
            public float FocusDistance; //manual settings
            public float Aperture;//manual settings
            public float BackgroundScale; // This remains a single float
        }


        #region Singleton
        public static DemoScenes Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType(typeof(DemoScenes)) as DemoScenes;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static DemoScenes _instance;
        #endregion

        protected virtual void Start()
        {
            if (!directionalLight)
            {
                Light[] lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

                // Iterate through the unsorted lights to find the first directional light
                foreach (var light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        directionalLight = light;
                        break; // Exit the loop once the first directional light is found
                    }
                }
            }
            Cursor.visible = !hideMouse;
            //Application.targetFrameRate = 500;

            GlobalVolume.profile.TryGet<DepthOfField>(out _depthOfField);

        }

        protected virtual void OnEnable()
        {
            if(GlobalVolumnBtn)
                GlobalVolumnBtn.onClick.AddListener(GlobalVolumnBtnClicked);
            LogoBtn.onClick.AddListener(LogoBtnClicked);


            foreach (var pair in buttonActions)
            {
                pair.Key.onClick.AddListener(pair.Value);
                //Debug.Log(pair.Key.name);
            }
        }

        protected virtual void OnDisable()
        {
            if (GlobalVolumnBtn)
                GlobalVolumnBtn.onClick.RemoveListener(GlobalVolumnBtnClicked);
            LogoBtn.onClick.RemoveListener(LogoBtnClicked);


            foreach (var pair in buttonActions)
            {
                pair.Key.onClick.RemoveListener(pair.Value);
            }
        }

        protected void LogoBtnClicked()
        {
            DemoUI.SetActive(!DemoUI.activeSelf);
        }


        protected void GlobalVolumnBtnClicked()
        {
            GlobalVolume.enabled = !GlobalVolume.enabled;
        }
        

        protected void FlyToNextCameraHandler()
        {
            if (isTransitioning) return;

            int nextCameraIndex = (currentCameraIndex + 1) % CameraDOFDatas.Count;
            CameraDOFData nextCameraData = CameraDOFDatas[nextCameraIndex];

           
            if (nextCameraData.FocusDistance != 0f || nextCameraData.Aperture != 0f)
            {
                SetDOF(nextCameraData.FocusDistance, nextCameraData.Aperture);
            }
            

            if (BGTransform)
            {
                float targetScale = (nextCameraData.BackgroundScale != 0) ? nextCameraData.BackgroundScale : 1f;
                SetBackgroundScale(targetScale);
            }

            StartCoroutine(TransitionToNextCameraAngle(CameraDOFDatas[currentCameraIndex].CameraAngle, nextCameraData.CameraAngle));
            currentCameraIndex = nextCameraIndex;
        }

        public void SetDOFImmediate(float in_focusDistance, float in_aperture)
        {

            _depthOfField.focusDistance.value = in_focusDistance;
            _depthOfField.aperture.value = in_aperture;
        }


        protected void SetDOF(float targetValue, float targetAperture, float delay = 0f)
        {
            float currentFocusDistance = _depthOfField.focusDistance.value;
            float currentAperture = _depthOfField.aperture.value;

            StartCoroutine(TransitionDOF(currentFocusDistance, currentAperture, targetValue, targetAperture, delay));
        }

        public void SetDOFFromDataIndex(int index, float delay = 1f)
        {
            float currentFocusDistance = _depthOfField.focusDistance.value;
            float currentAperture = _depthOfField.aperture.value;
            StartCoroutine(TransitionDOF(currentFocusDistance, currentAperture, CameraDOFDatas[index].FocusDistance, CameraDOFDatas[index].Aperture, delay));
        }

        protected void SetBackgroundScale(float targetScale)
        {
            float currentScale = BGTransform.transform.localScale.x; // Assuming uniform scale
            StartCoroutine(TransitionBackgroundScale(currentScale, targetScale));
        }

        protected void ToggleLight()
        {
            if(!lightObject)
                lightObject = GameObject.Find("Directional Light");

            if (lightObject)
            {
                directionalLight = lightObject.GetComponent<Light>();
                directionalLight.enabled = !directionalLight.enabled;
            }
            else
            {
                Debug.LogWarning("Directional Light not found!");
            }

        }

        protected IEnumerator TransitionToNextCameraAngle(Transform fromAngle, Transform toAngle)
        {
            isTransitioning = true;
            float timeElapsed = 0;

            Vector3 startPosition = fromAngle.position;
            Quaternion startRotation = fromAngle.rotation;

            Vector3 endPosition = toAngle.position;
            Quaternion endRotation = toAngle.rotation;

            while (timeElapsed < transitionSpeed)
            {
                float t = timeElapsed / transitionSpeed;
                Camera.main.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                Camera.main.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Make sure we end at the exact position
            Camera.main.transform.position = endPosition;
            Camera.main.transform.rotation = endRotation;

            isTransitioning = false;
        }

        protected IEnumerator TransitionDOF(float startValue, float startAperture, float endValue, float endAperture, float delay = 0f)
        {
            // If a delay is specified, smoothly interpolate the values during the delay
            if (delay > 0f)
            {
                float delayElapsed = 0f;

                while (delayElapsed < delay)
                {
                    float delayT = delayElapsed / delay;

                    _depthOfField.focusDistance.value = Mathf.Lerp(startValue, endValue, delayT);
                    _depthOfField.aperture.value = Mathf.Lerp(startAperture, endAperture, delayT);

                    delayElapsed += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                float timeElapsed = 0f;

                while (timeElapsed < transitionSpeed)
                {
                    float t = timeElapsed / transitionSpeed;

                    _depthOfField.focusDistance.value = Mathf.Lerp(startValue, endValue, t);
                    _depthOfField.aperture.value = Mathf.Lerp(startAperture, endAperture, t);

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }

            // Ensure we end with the exact values
            _depthOfField.focusDistance.value = endValue;
            _depthOfField.aperture.value = endAperture;
        }



        protected IEnumerator TransitionBackgroundScale(float startScale, float endScale)
        {
            float timeElapsed = 0;

            while (timeElapsed < transitionSpeed)
            {
                float t = timeElapsed / transitionSpeed;

                float currentScale = Mathf.Lerp(startScale, endScale, t);
                BGTransform.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure we end with the exact scale
            BGTransform.transform.localScale = new Vector3(endScale, endScale, endScale);
        }

        protected void AnimToNextDestination(Transform in_itemToMove)
        {
            if (!isAnimating)
            {
                // Move to the next index (looping back to the start if we reach the end)
                int nextIndex = (currentIndex + 1) % TargetPositions.Count;

                // Start the animation
                StartCoroutine(MoveToTarget(in_itemToMove, TargetPositions[nextIndex].position));

                // Update the current index
                currentIndex = nextIndex;
            }
        }

        IEnumerator MoveToTarget(Transform itemToMove, Vector3 endPosition)
        {
            isAnimating = true;
            float duration = 1f; // Animation duration in seconds
            float elapsedTime = 0f;

            Vector3 startPosition = itemToMove.position;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float smoothedT = Mathf.SmoothStep(0.0f, 1.0f, t);  // SmoothStep easing

                // Update the position of the GameObject
                itemToMove.position = Vector3.Lerp(startPosition, endPosition, smoothedT);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Make sure the GameObject ends up in the exact end position
            itemToMove.position = endPosition;

            isAnimating = false;
        }

        public void SetDescriptionText(string inText)
        {

            DescriptionText.SetText(inText);
        }

        protected void RegisterButtonAction(Button button, UnityAction action)
        {
            buttonActions[button] = action;
        }


        protected void HideObjects(List<GameObject> in_OBJ)
        {
            foreach (var item in in_OBJ)
            {
                item.SetActive(false);
            }
        }


        public void PlaySFX(AudioClip clip)
        {
            SFX.clip = clip;
            SFX.Play();
        }


        public virtual void AssetBtnHandler()
        {
            Application.OpenURL(publisherSite);
        }

        public virtual void LogoBtnHandler()
        {
            Application.OpenURL(WebsiteURL);
        }

        public virtual void LaunchURL(string in_url)
        {
            Application.OpenURL(in_url);
        }

    }

}