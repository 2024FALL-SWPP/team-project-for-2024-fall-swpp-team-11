
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace NekoLegends
{
    [ExecuteInEditMode]
    public class GrassShaderRenderer : MonoBehaviour
    {
        [Header("Auto Update Ensures Grass in Editor Stay Visible (But may be slow).")]
        public bool autoUpdate;
        private Camera m_MainCamera;


        [Space]
        public GrassSettings grassSettings;
        GrassShaderInteractor[] interactors;
        [SerializeField, HideInInspector]
        List<GrassData> grassData = new List<GrassData>();
        List<int> grassVisibleIDList = new List<int>();
        private bool m_Initialized;
        private ComputeBuffer m_SourceVertBuffer;
        private ComputeBuffer m_DrawBuffer;
        private ComputeBuffer m_ArgsBuffer;
        private ComputeShader m_InstantiatedComputeShader;
        private ComputeBuffer m_VisibleIDBuffer;
        [SerializeField] Material m_InstantiatedMaterial;
        private int m_IdGrassKernel;
        private int m_DispatchSize;
        uint threadGroupSize;
        private const int SOURCE_VERT_STRIDE = sizeof(float) * (3 + 3 + 2 + 3);
        private const int DRAW_STRIDE = sizeof(float) * (3 + 3 + ((3 + 2) * 3));
        Bounds bounds;
        private uint[] argsBufferReset = new uint[5]
       {
        0,          1,          0,          0,          0          };

        GrassCulling cullingTree;
        List<Bounds> BoundsListVis = new List<Bounds>();
        List<GrassCulling> leaves = new List<GrassCulling>();
        Plane[] cameraFrustumPlanes = new Plane[6];
        float cameraOriginalFarPlane;
        List<int> empty = new List<int>();
        Vector3 m_cachedCamPos;
        Quaternion m_cachedCamRot;
        bool m_fastMode;
        int shaderID;
        int maxBufferSize = 2500000;

        public List<GrassData> SetGrassPaintedDataList
        {
            get { return grassData; }
            set { grassData = value; }
        }

#if UNITY_EDITOR
        SceneView view;
        void OnDestroy()
        {
           SceneView.duringSceneGui -= this.OnScene;
        }

        void OnScene(SceneView scene)
        {
            view = scene;
            if (!Application.isPlaying)
            {
                if (view.camera != null)
                {
                    m_MainCamera = view.camera;
                }
            }
            else
            {
                m_MainCamera = Camera.main;
            }
        }

        private void OnValidate()
        {
                        if (!Application.isPlaying)
            {
                if (view != null)
                {
                    m_MainCamera = view.camera;
                }
            }
            else
            {
                m_MainCamera = Camera.main;
            }
        }
#endif



        private void OnEnable()
        {
            if (m_Initialized)
            {
                OnDisable();
            }

            MainSetup(true);
        }

        void MainSetup(bool full)
        {
#if UNITY_EDITOR

            SceneView.duringSceneGui += this.OnScene;
            if (!Application.isPlaying)
            {
                if (view != null)
                {
                    m_MainCamera = view.camera;
                }

            }
#endif
            if (Application.isPlaying)
            {
                m_MainCamera = Camera.main;
            }

            if (grassData.Count == 0)
            {
                return;
            }

            if (grassSettings.shaderToUse == null || grassSettings.materialToUse == null)
            {
                Debug.LogWarning("Missing Grass Shader/Material in grass Settings", this);
                return;
            }
            PopulateEmptyList(grassData.Count);
            m_Initialized = true;m_InstantiatedComputeShader = Instantiate(grassSettings.shaderToUse);
            m_InstantiatedMaterial = Instantiate(grassSettings.materialToUse);
            int numSourceVertices = grassData.Count;
            int maxBladesPerVertex = Mathf.Max(1, grassSettings.allowedBladesPerVertex);
            int maxSegmentsPerBlade = Mathf.Max(1, grassSettings.allowedSegmentsPerBlade);
            int maxBladeTriangles = maxBladesPerVertex * ((maxSegmentsPerBlade - 1) * 2 + 1);
            m_SourceVertBuffer = new ComputeBuffer(numSourceVertices, SOURCE_VERT_STRIDE, ComputeBufferType.Structured, ComputeBufferMode.Immutable);
            m_SourceVertBuffer.SetData(grassData);
            m_DrawBuffer = new ComputeBuffer(maxBufferSize, DRAW_STRIDE, ComputeBufferType.Append);
            m_ArgsBuffer = new ComputeBuffer(1, argsBufferReset.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            m_VisibleIDBuffer = new ComputeBuffer(grassData.Count, sizeof(int), ComputeBufferType.Structured); 
            m_IdGrassKernel = m_InstantiatedComputeShader.FindKernel("Main");
            m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_SourceVertices",
            m_SourceVertBuffer);
            m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_DrawTriangles", m_DrawBuffer);
            m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_IndirectArgsBuffer", m_ArgsBuffer);
            m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_VisibleIDBuffer", m_VisibleIDBuffer);
            m_InstantiatedMaterial.SetBuffer("_DrawTriangles", m_DrawBuffer);
            m_InstantiatedComputeShader.SetInt("_NumSourceVertices", numSourceVertices);
            shaderID = Shader.PropertyToID("_PositionsMoving");
            m_InstantiatedComputeShader.GetKernelThreadGroupSizes(m_IdGrassKernel,
            out threadGroupSize, out _, out _);
            m_DispatchSize = Mathf.CeilToInt(grassData.Count / threadGroupSize);
            SetGrassDataBase(full);
            if (full)
            {

                UpdateBounds();

            }
            SetupQuadTree(full);
        }

        void UpdateBounds()
        {
            bounds = new Bounds(grassData[0].position, Vector3.one);

            for (int i = 0; i < grassData.Count; i++)
            {
                Vector3 target = grassData[i].position;

                bounds.Encapsulate(target);
            }
        }

        void SetupQuadTree(bool full)
        {
            if (full)
            {
                cullingTree = new GrassCulling(bounds, grassSettings.cullingTreeDepth);
                cullingTree.RetrieveAllLeaves(leaves);
                for (int i = 0; i < grassData.Count; i++)
                {
                    cullingTree.FindLeaf(grassData[i].position, i);
                }
                cullingTree.ClearEmpty();
            }
            else
            {
                GrassFastList(grassData.Count);
                m_VisibleIDBuffer.SetData(grassVisibleIDList);
            }
        }

        void GrassFastList(int count)
        {
            grassVisibleIDList = Enumerable.Range(0, count).ToArray().ToList();
        }

        void PopulateEmptyList(int count)
        {
            empty = new List<int>(count);
            empty.InsertRange(0, Enumerable.Repeat(-1, count));
        }

        void GetFrustumData()
        {
            if (m_MainCamera == null)
            {
                return;
            }
            if (m_cachedCamRot == m_MainCamera.transform.rotation && m_cachedCamPos == m_MainCamera.transform.position && Application.isPlaying)
            {
                return;
            }
            cameraOriginalFarPlane = m_MainCamera.farClipPlane;
            m_MainCamera.farClipPlane = grassSettings.maxDrawDistance;
            GeometryUtility.CalculateFrustumPlanes(m_MainCamera, cameraFrustumPlanes);
            m_MainCamera.farClipPlane = cameraOriginalFarPlane;
            if (!m_fastMode)
            {
                BoundsListVis.Clear();
                m_VisibleIDBuffer.SetData(empty);
                grassVisibleIDList.Clear();
                cullingTree.RetrieveLeaves(cameraFrustumPlanes, BoundsListVis, grassVisibleIDList);
                m_VisibleIDBuffer.SetData(grassVisibleIDList);
            }

                        m_cachedCamPos = m_MainCamera.transform.position;
            m_cachedCamRot = m_MainCamera.transform.rotation;
        }

        private void OnDisable()
        {
            if (m_Initialized)
            {
                if (Application.isPlaying)
                {
                    Destroy(m_InstantiatedComputeShader);
                    Destroy(m_InstantiatedMaterial);
                }
                else
                {
                    DestroyImmediate(m_InstantiatedComputeShader);
                    DestroyImmediate(m_InstantiatedMaterial);
                }
                m_SourceVertBuffer?.Release();
                m_DrawBuffer?.Release();
                m_ArgsBuffer?.Release();
                m_VisibleIDBuffer?.Release();
            }
            m_Initialized = false;
        }

        private void Update()
        {
            if (!Application.isPlaying && autoUpdate && !m_fastMode)
            {
                OnDisable();
                OnEnable();
            }

            if (!m_Initialized)
            {
              return;
            }
            GetFrustumData();
            SetGrassDataUpdate();
            m_DrawBuffer.SetCounterValue(0);
            m_ArgsBuffer.SetData(argsBufferReset);
            m_DispatchSize = Mathf.CeilToInt(grassVisibleIDList.Count / threadGroupSize);
            if (grassVisibleIDList.Count > 0)
            {
                m_DispatchSize += 1;
            }
            if (m_DispatchSize > 0)
            {
                m_InstantiatedComputeShader.Dispatch(m_IdGrassKernel, m_DispatchSize, 1, 1);
                Graphics.DrawProceduralIndirect(m_InstantiatedMaterial, bounds, MeshTopology.Triangles,
                m_ArgsBuffer, 0, null, null, grassSettings.castShadow, true, gameObject.layer);
            }
        }

        private void SetGrassDataBase(bool full)
        {
            m_InstantiatedComputeShader.SetFloat("_Time", Time.time);
            m_InstantiatedComputeShader.SetFloat("_GrassRandomHeightMin", grassSettings.grassRandomHeightMin);
            m_InstantiatedComputeShader.SetFloat("_GrassRandomHeightMax", grassSettings.grassRandomHeightMax);
            m_InstantiatedComputeShader.SetFloat("_WindSpeed", grassSettings.windSpeed);
            m_InstantiatedComputeShader.SetFloat("_WindStrength", grassSettings.windStrength);


            if (full)
            {
                m_InstantiatedComputeShader.SetFloat("_MinFadeDist", grassSettings.minFadeDistance);
                m_InstantiatedComputeShader.SetFloat("_MaxFadeDist", grassSettings.maxDrawDistance);
                //interactors = (GrassShaderInteractor[])FindObjectsOfType(typeof(GrassShaderInteractor));
                interactors = Object.FindObjectsByType<GrassShaderInteractor>(FindObjectsSortMode.None);
            }
            else
            {
                if (grassData.Count > 200000)
                {
                    m_InstantiatedComputeShader.SetFloat("_MinFadeDist", 40f);
                    m_InstantiatedComputeShader.SetFloat("_MaxFadeDist", 50f);
                }
                else
                {
                    m_InstantiatedComputeShader.SetFloat("_MinFadeDist", grassSettings.minFadeDistance);
                    m_InstantiatedComputeShader.SetFloat("_MaxFadeDist", grassSettings.maxDrawDistance);
                }

            }
            m_InstantiatedComputeShader.SetFloat("_InteractorStrength", grassSettings.affectStrength);
            m_InstantiatedComputeShader.SetFloat("_BladeRadius", grassSettings.bladeRadius);
            m_InstantiatedComputeShader.SetFloat("_BladeForward", grassSettings.bladeForwardAmount);
            m_InstantiatedComputeShader.SetFloat("_BladeCurve", Mathf.Max(0, grassSettings.bladeCurveAmount));
            m_InstantiatedComputeShader.SetFloat("_BottomWidth", grassSettings.bottomWidth);
            m_InstantiatedComputeShader.SetInt("_MaxBladesPerVertex", grassSettings.allowedBladesPerVertex);
            m_InstantiatedComputeShader.SetInt("_MaxSegmentsPerBlade", grassSettings.allowedSegmentsPerBlade);
            m_InstantiatedComputeShader.SetFloat("_MinHeight", grassSettings.MinHeight);
            m_InstantiatedComputeShader.SetFloat("_MinWidth", grassSettings.MinWidth);
            m_InstantiatedComputeShader.SetFloat("_MaxHeight", grassSettings.MaxHeight);
            m_InstantiatedComputeShader.SetFloat("_MaxWidth", grassSettings.MaxWidth);
            m_InstantiatedMaterial.SetColor("_TopTint", grassSettings.topTint);
            m_InstantiatedMaterial.SetColor("_BottomTint", grassSettings.bottomTint);
        }

        public void Reset()
        {
            m_fastMode = false;
            OnDisable();
            MainSetup(true);
        }

        public void ResetFaster()
        {
            m_fastMode = true;
            OnDisable();
            MainSetup(false);
        }
        private void SetGrassDataUpdate()
        {
            m_InstantiatedComputeShader.SetFloat("_Time", Time.time);
            m_InstantiatedComputeShader.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
            if (interactors.Length > 0)
            {
                Vector4[] positions = new Vector4[interactors.Length];

                for (int i = 0; i < interactors.Length; i++)
                {
                    positions[i] = new Vector4(interactors[i].transform.position.x, interactors[i].transform.position.y, interactors[i].transform.position.z,
                    interactors[i].radius);
                }
                m_InstantiatedComputeShader.SetVectorArray(shaderID, positions);
                m_InstantiatedComputeShader.SetFloat("_InteractorsLength", interactors.Length);
            }
            if (m_MainCamera != null)
            {
                m_InstantiatedComputeShader.SetVector("_CameraPositionWS", m_MainCamera.transform.position);
            }
#if UNITY_EDITOR
            else if (view != null)
            {
                m_InstantiatedComputeShader.SetVector("_CameraPositionWS", view.camera.transform.position);
            }
#endif
        }
        void OnDrawGizmos()
        {
            if (grassSettings)
            {
                if (grassSettings.drawBounds)
                {
                    Gizmos.color = new Color(0, 1, 0, 0.3f);
                    for (int i = 0; i < BoundsListVis.Count; i++)
                    {
                        Gizmos.DrawWireCube(BoundsListVis[i].center, BoundsListVis[i].size);
                    }
                    Gizmos.color = new Color(1, 0, 0, 0.3f);
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }

        }
    }
    [System.Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct GrassData
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 length;
        public Vector3 color;
    }
}