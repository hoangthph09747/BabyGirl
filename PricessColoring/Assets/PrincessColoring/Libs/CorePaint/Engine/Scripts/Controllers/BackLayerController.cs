using UnityEngine;
using PaintCraft.Utils;


namespace PaintCraft.Controllers
{
	public class BackLayerController : MonoBehaviour
	{

		public RenderTexture RenderTexture { get; private set; }
		public CanvasController canvas;

        public Texture tex, maskTexture;
        public Texture2D texture2D;
        RenderTexture renderTexture;
        bool beginStartGiltter = false;
        public int layerNormal = 25;
        public int layerGiltter = 26;
        private void Start()
        {
            mat = gameObject.GetComponent<Renderer>().material;
        }

        public void Init(CanvasController canvas)
		{
			this.canvas = canvas;
			UpdateMeshSize();
		}
        public void LogState()
        {
            Debug.Log("=== BackLayerController State ===");
            Debug.Log($"RenderTexture (property): {RenderTexture}");
            Debug.Log($"canvas: {canvas}");
            Debug.Log($"tex: {tex}");
            Debug.Log($"maskTexture: {maskTexture}");
            Debug.Log($"texture2D: {texture2D}");
            Debug.Log($"renderTexture (field): {renderTexture}");
            Debug.Log($"beginStartGiltter: {beginStartGiltter}");
            Debug.Log($"layerNormal: {layerNormal}");
            Debug.Log($"layerGiltter: {layerGiltter}");
            Debug.Log("=================================");
        }
        //void UpdateMeshSize()
        //{
        //	MeshFilter mf = GOUtil.CreateComponentIfNoExists<MeshFilter>(gameObject);
        //	Mesh mesh = MeshUtil.CreatePlaneMesh(canvas.Width, canvas.Height);
        //	mf.mesh = mesh;
        //	MeshRenderer mr = GOUtil.CreateComponentIfNoExists<MeshRenderer>(gameObject);

        //	string shaderName = canvas.DefaultBGColor.a == 1.0 ? "Unlit/Texture" : "Unlit/Transparent";
        //	mr.material = new Material(Shader.Find(shaderName));
        //	RenderTexture = TextureUtil.SetupRenderTextureOnMaterial(mr.material, canvas.RenderTextureSize.x, canvas.RenderTextureSize.y);
        //}

        void SetLayer()
        {
            if (canvas.typePen == TypePen.Glow)
            {
               // gameObject.layer = layerGlow;
            }
            else if (canvas.typePen == TypePen.Giltter)
            {
                gameObject.layer = layerGiltter;
            }
            else if (canvas.typePen == TypePen.Color)
            {
                gameObject.layer = layerNormal;
            }
        }

        void UpdateMeshSize()
        {
            MeshFilter mf = GOUtil.CreateComponentIfNoExists<MeshFilter>(gameObject);
            Mesh mesh = MeshUtil.CreatePlaneMesh(canvas.Width, canvas.Height);
            mf.mesh = mesh;
            MeshRenderer mr = GOUtil.CreateComponentIfNoExists<MeshRenderer>(gameObject);

            //string shaderName = canvas.DefaultBGColor.a == 1.0 ? "Unlit/Texture" : "Unlit/Transparent";
            //ngocdu
          string shaderName = canvas.DefaultBGColor.a == 1.0 ? "MK/Glow/Selective/Transparent/Diffuse" : "Unlit/Transparent";

            //ngocdu
           if (canvas.typePen == TypePen.Giltter)
            {
                //if (ReadSystems.instance.isGoodDevice == false)
                //{
                //    shaderName = "Spaventacorvi/Glitter/Glitter B - Rough Textured";
                //}
                //else
                {
                    shaderName = "Spaventacorvi/Glitter/GlitterB-OldData";
                }
            }
            else if (canvas.typePen == TypePen.Color)
            {
                //shaderName = "Unlit/Texture";
                shaderName = "Unlit/Transparent";

                //if(LoadSceneManager.Instance?.nameMinigame == NameMinigame.DIY)
                //{
                //    shaderName = "Unlit/DIY";
                //}    
            }
#if UNITY_EDITOR || UNITY_ANDROID

            mr.material = new Material(Shader.Find(shaderName));
#else

            mr.material = new Material(Shader.Find("Unlit/Transparent"));
#endif
            RenderTexture = TextureUtil.SetupRenderTextureOnMaterial(mr.material, canvas.RenderTextureSize.x, canvas.RenderTextureSize.y);
#if UNITY_EDITOR || UNITY_ANDROID
            if (canvas.typePen == TypePen.Giltter )
            {
                //------------------giltter-----------------
                ////Spaventacorvi/Glitter/ 
                ////set glliter texture
                //mr.material.SetTexture("vwxwww", mr.material.mainTexture);
                //set mask texture
                mr.material.SetTexture("wvvxxv", mr.material.mainTexture);
                Texture textureGlliter = Resources.Load("GiltterMap/Gitller/10") as Texture;
                mr.material.SetTexture("vvxxww", textureGlliter);

                //set Specular power to 0
                mr.material.SetFloat("vxvvvw", 0);

                //setGlitter power to 0
                mr.material.SetFloat("vwwxwx", 0);

                mr.material.SetFloat("_isTranparent", 1);
                mr.material.SetFloat("wvvxxw", 2.5f);

                //ngocdu

                //Texture textureNoise = Resources.Load("Glitter_map_04_129") as Texture;
                //mr.material.SetTexture("_NoiseTex", textureNoise);

                tex = (mr.material.GetTexture("wvvxxv"));
                texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);

                // gameObject.AddComponent<BoxCollider>();

                renderTexture = new RenderTexture(texture2D.width, texture2D.height, 32);

                maskTexture = mr.material.GetTexture("wvvxxv");

                ShowGiltterTexture();
            }
#endif
            //if (LoadSceneManager.Instance?.nameMinigame == NameMinigame.DIY)
            //    mr.material.SetTexture("_MaskTex", DIY.GameManager_DIY.Instance.maskTex);

            //ngocdu FPS
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;

            gameObject.AddComponent<MeshCollider>();

            SetLayer();
        }

        public void ShowGiltterTexture()
        {
            if (canvas.typePen == TypePen.Giltter)
            {
                // MyDebug.Debug_Log("ShowGiltterTexture");
                MeshRenderer mr = GOUtil.CreateComponentIfNoExists<MeshRenderer>(gameObject);
                //Spaventacorvi/Glitter/ 
                //set glliter Specular
                Texture textureSpecular = Resources.Load("GiltterMap/Smoke_5") as Texture;
                mr.material.SetTexture("vwxwww", textureSpecular);

                //set mask texture
                //mr.material.SetTexture("wvvxxv", mr.material.mainTexture);

                //set glliter texture
                Texture textureGlliter = Resources.Load("GiltterMap/Gitller/10") as Texture;
                mr.material.SetTexture("vvxxww", textureGlliter);

                //set Specular power to 1.5
                mr.material.SetFloat("vxvvvw", 1.5f);

                //setGlitter power to 10
                mr.material.SetFloat("vwwxwx", 4);

                //set Shininess to 0
                mr.material.SetFloat("wvwvww", 0);

                //set Fake light to 0
                mr.material.SetFloat("vvxwwx", 0.1f);

                mr.material.SetFloat("_isMask", 0);

                //ChangeOffset();
                //colorTween = mat.DOColor(new Color(0.4f, 0.4f, 0.4f, 0), "vwxvww", 0.5f).SetLoops(1).OnComplete(ChangeColor);

                beginStartGiltter = true;
            }

        }

        public void SetNewSize()
		{
			canvas.CanvasCameraController.Camera.targetTexture = null;
			RenderTexture = TextureUtil.UpdateRenderTextureSize(RenderTexture, canvas.RenderTextureSize.x, canvas.RenderTextureSize.y);
			GetComponent<MeshRenderer>().material.mainTexture = RenderTexture;
			canvas.CanvasCameraController.Camera.targetTexture = RenderTexture;
			MeshUtil.ChangeMeshSize(GetComponent<MeshFilter>().mesh, canvas.RenderTextureSize.x, canvas.RenderTextureSize.y);
		}

        float offset;
        Vector2 offsetVector = Vector2.zero;
        Material mat;
        void Update()
        {
            if (beginStartGiltter)
            {
                offset += Time.deltaTime * 0.02f;
                if (offset >= 5)
                {
                    offset = 0;
                }
                offsetVector.x = offset;
                mat.SetTextureOffset("vvxxww", offsetVector);
            }
        }
    }
}
