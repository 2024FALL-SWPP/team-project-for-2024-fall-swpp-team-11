using UnityEngine;
namespace NekoLegends
{
    [ExecuteInEditMode]
    public class GrassMeshRenderer : MonoBehaviour
    {
        public Camera grassRenderCamera;
        // layer to render
        [SerializeField]
        LayerMask layer;

        // objects to render
        [SerializeField]
        Renderer[] renderers;
        // unity terrain to render
        [SerializeField]
        Terrain[] terrains;
        // map resolution
        public int resolution = 512;

        // padding the total size
        private float gassPadding = 2.85f;
        private bool RealTimeDiffuse;
        RenderTexture tempTex;

        private float repeatRate = 5f;
        private Bounds bounds;

        void GetBounds()
        {

            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    if (bounds.size.magnitude < 0.1f)
                    {
                        bounds = new Bounds(renderer.transform.position, Vector3.zero);
                    }
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            if (terrains.Length > 0)
            {
                foreach (Terrain terrain in terrains)
                {
                    if (bounds.size.magnitude < 0.1f)
                    {
                        bounds = new Bounds(terrain.transform.position, Vector3.zero);
                    }
                    Vector3 terrainCenter = terrain.GetPosition() + terrain.terrainData.bounds.center;
                    Bounds worldBounds = new Bounds(terrainCenter, terrain.terrainData.bounds.size);
                    bounds.Encapsulate(worldBounds);
                }
            }
        }

        void OnEnable()
        {
            // reset bounds
            bounds = new Bounds(transform.position, Vector3.zero);
            tempTex = new RenderTexture(resolution, resolution, 24);
            GetBounds();
            SetUpCam();
            DrawDiffuseMap();
        }


        void Start()
        {
            GetBounds();
            SetUpCam();
            DrawDiffuseMap();
            if (RealTimeDiffuse)
            {
                InvokeRepeating("UpdateTex", 1f, repeatRate);
            }

        }


        void UpdateTex()
        {
            grassRenderCamera.enabled = true;
            grassRenderCamera.targetTexture = tempTex;
            Shader.SetGlobalTexture("_TerrainDiffuse", tempTex);
        }
        public void DrawDiffuseMap()
        {
            DrawToMap("_TerrainDiffuse");
        }

        void DrawToMap(string target)
        {
            grassRenderCamera.enabled = true;
            grassRenderCamera.targetTexture = tempTex;
            grassRenderCamera.depthTextureMode = DepthTextureMode.Depth;

            Shader.SetGlobalFloat("_OrthographicCamSizeTerrain", grassRenderCamera.orthographicSize);
            Shader.SetGlobalVector("_OrthographicCamPosTerrain", grassRenderCamera.transform.position);
            grassRenderCamera.Render();
            Shader.SetGlobalTexture(target, tempTex);

            grassRenderCamera.enabled = false;
        }

        void SetUpCam()
        {
            if (grassRenderCamera == null)
            {
                grassRenderCamera = GetComponentInChildren<Camera>();
            }

            float size = bounds.size.magnitude;
            grassRenderCamera.cullingMask = layer;
            grassRenderCamera.orthographicSize = size / gassPadding;
            grassRenderCamera.transform.parent = null;
            grassRenderCamera.transform.position = bounds.center + new Vector3(0, bounds.extents.y + 5f, 0);
            grassRenderCamera.transform.parent = gameObject.transform;
            
        }

    }
}