using System;
using System.Collections;
using UnityEngine;


namespace NekoLegends
{
    public class DemoNekoCharacter : MonoBehaviour
    {
        [SerializeField] protected AnimationClip[] poses;

        [SerializeField] protected SkinnedMeshRenderer skinnedMeshRendererBody, skinnedMeshRendererHair, skinnedMeshRendererHairBase, 
            skinnedMeshRendererEar, skinnedMeshRendererHeadFace;

        [SerializeField] protected Material[] bodyMaterialList, hairMaterialList, earMaterialList, eyesMaterialList;
        [Space]
        [SerializeField] protected Boolean autoblink;
        [SerializeField] protected Vector2 blinkStartEndTimeRange = new Vector2(3,6);
        [SerializeField] protected float blinkDuration = 0.15f;
        [SerializeField] protected float blinkClosedDuration = 0.5f;




        public Animator animator { get; protected set; }
      
        protected int _currentEmotionIndex, _hairColorIndex, _eyeColorIndex, _currentPoseIndex=-1;

        protected int _emotionIndexMax = 10;
        protected float _transitionDuration = 0.25f; 

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            ResetBlendShapes();
            if (autoblink)
            {
                StartCoroutine(AutoBlink());
            }
        }

        public void NextEmotion()
        {
            _currentEmotionIndex++;
            if (_currentEmotionIndex > _emotionIndexMax)
                _currentEmotionIndex = 0;

            SetEmotion(_currentEmotionIndex);
        }

        protected virtual void SetEmotion(int emoIndex)
        {
            ResetBlendShapes();
            switch (emoIndex)
            {
                case 0:
                    ResetBlendShapes();
                    DemoScenes.Instance.SetDescriptionText("Emotion: None");
                    break;
                case 1:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Happy");
                    SetBlendShapeWeightFace(18, 10f);
                    SetBlendShapeWeightFace(20, 35f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    break;
                case 2:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Wink left");
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(2, 100f);
                    break;
                case 3:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Wink right");
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(3, 100f);
                    break;
                case 4:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Excited Curious Neko");
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(5, 100f);
                    SetBlendShapeWeightFace(34, 100f);
                    SetBlendShapeWeightFace(35, 100f);
                    SetBlendShapeWeightFace(42, 100f);
                    break;
                case 5:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Surprised");
                    SetBlendShapeWeightFace(4, 100f);
                    SetBlendShapeWeightFace(5, 100f);
                    SetBlendShapeWeightFace(10, 100f);
                    SetBlendShapeWeightFace(18, 50f);
                    break;
                case 6:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Serious");
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(9, 100f);
                    break;
                case 7:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Mad");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(9, 100f);
                    SetBlendShapeWeightFace(11, 100f);
                    SetBlendShapeWeightFace(12, 100f);
                    break;
                case 8:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Angry");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(9, 100f);
                    SetBlendShapeWeightFace(11, 100f);
                    SetBlendShapeWeightFace(12, 100f);
                    SetBlendShapeWeightFace(15, 100f);
                    SetBlendShapeWeightFace(22, 100f);
                    break;
                case 9:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Sad");
                    SetBlendShapeWeightFace(16, 100f);
                    SetBlendShapeWeightFace(38, 100f);
                    SetBlendShapeWeightFace(39, 100f);
                    break;
                case 10:
                    DemoScenes.Instance.SetDescriptionText("Emotion: Puff Cheeks Tongue Out");
                    SetBlendShapeWeightFace(6, 100f);
                    SetBlendShapeWeightFace(7, 100f);
                    SetBlendShapeWeightFace(8, 100f);
                    SetBlendShapeWeightFace(9, 100f);
                    SetBlendShapeWeightFace(11, 100f);
                    SetBlendShapeWeightFace(12, 100f);
                    SetBlendShapeWeightFace(45, 100f);
                    SetBlendShapeWeightFace(52, 100f);
                    SetBlendShapeWeightFace(53, 60f);
                    break;

            }
        }
        private IEnumerator AutoBlink()
        {
            while (autoblink)
            {
                float timeUntilNextBlink = UnityEngine.Random.Range(blinkStartEndTimeRange[0], blinkStartEndTimeRange[1]);
                
                yield return new WaitForSeconds(timeUntilNextBlink);

                SetBlendShapeWeightFace(2, 100f, blinkDuration);
                SetBlendShapeWeightFace(3, 100f, blinkDuration);

                // Wait briefly while eyes are closed
                yield return new WaitForSeconds(blinkClosedDuration);

                // Open eyes (animate eyelid blend shapes back to 0%)
                SetBlendShapeWeightFace(2, 0f, blinkDuration);
                SetBlendShapeWeightFace(3, 0f, blinkDuration);
            }
        }

        private void ResetBlendShapes()
        {
            for (int i = 0; i < skinnedMeshRendererHeadFace.sharedMesh.blendShapeCount; i++)
            {
                skinnedMeshRendererHeadFace.SetBlendShapeWeight(i, 0f);
            }
        }


        public virtual void ToggleHairColor()
        {
            _hairColorIndex++;
            if (_hairColorIndex >= hairMaterialList.Length)
                _hairColorIndex = 0;

            Material[] hairMaterials = skinnedMeshRendererHair.materials;
            Material[] hairBaseMaterials = skinnedMeshRendererHairBase.materials;
            Material[] earMaterials = skinnedMeshRendererEar.materials;

            hairMaterials[0] = hairMaterialList[_hairColorIndex];
            earMaterials[0] = earMaterialList[_hairColorIndex];

            skinnedMeshRendererHair.materials = hairMaterials;
            skinnedMeshRendererHairBase.materials = hairMaterials; //match hair color
            skinnedMeshRendererEar.materials = earMaterials;

            DemoScenes.Instance.DescriptionText.SetText(hairMaterialList[_hairColorIndex].name);
        }



        public void ToggleEyeColor()
        {
            _eyeColorIndex++;
            if (_eyeColorIndex >= eyesMaterialList.Length)
                _eyeColorIndex = 0;

            skinnedMeshRendererHeadFace.material = eyesMaterialList[_eyeColorIndex];

            DemoScenes.Instance.DescriptionText.SetText(eyesMaterialList[_eyeColorIndex].name);

        }

        public void ToggleNekoEars()
        {
            skinnedMeshRendererEar.gameObject.SetActive(!skinnedMeshRendererEar.gameObject.activeSelf);
        }

        public void NextPose()
        {
            _currentPoseIndex++;
            if (_currentPoseIndex >= poses.Length)
            {
                _currentPoseIndex = 0;
            }
            AnimationClip nextPose = poses[_currentPoseIndex];
            this.animator.CrossFade(nextPose.name, _transitionDuration);
            DemoScenes.Instance.DescriptionText.SetText("Pose Animation: " + nextPose.name);
        }


        public void AddMaterialToRenderer(SkinnedMeshRenderer renderer, Material materialToAdd)
        {
            Material[] currentMaterials = renderer.materials;
            Material[] newMaterials = new Material[currentMaterials.Length + 1];
            currentMaterials.CopyTo(newMaterials, 0);
            newMaterials[currentMaterials.Length] = materialToAdd;
            renderer.materials = newMaterials;
        }

        public void RemoveLastMaterialFromRenderer(SkinnedMeshRenderer renderer)
        {
            if (renderer.materials.Length > 1)
            {
                Material[] materials = renderer.materials;
                Array.Resize(ref materials, materials.Length - 1);
                renderer.materials = materials;
            }
        }


        protected void SetBlendShapeWeightFace(int blendShapeIndex, float targetWeight, float duration=.25f)
        {
            StartCoroutine(AnimateBlendShape(blendShapeIndex, targetWeight, duration));

        }

        private IEnumerator AnimateBlendShape(int blendShapeIndex, float targetWeight, float duration)
        {
            float startTime = Time.time;
            float startWeight = skinnedMeshRendererHeadFace.GetBlendShapeWeight(blendShapeIndex);

            while (Time.time < startTime + duration)
            {
                float elapsed = Time.time - startTime;
                float normalizedTime = elapsed / duration;

                float weight = Mathf.Lerp(startWeight, targetWeight, normalizedTime);
                skinnedMeshRendererHeadFace.SetBlendShapeWeight(blendShapeIndex, weight);

                yield return null;
            }

            skinnedMeshRendererHeadFace.SetBlendShapeWeight(blendShapeIndex, targetWeight);
        }


        public virtual void NextClothing() { }

        public virtual void ToggleHeadGear() { }
        public virtual void ToggleFaceGear() { }
    }
}
