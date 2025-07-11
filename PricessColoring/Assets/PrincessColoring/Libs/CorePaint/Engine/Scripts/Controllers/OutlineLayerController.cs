using UnityEngine;
using PaintCraft.Utils;


namespace PaintCraft.Controllers
{
	public class OutlineLayerController : MonoBehaviour
	{
		CanvasController canvas;
		private Material material;
		public int layerNormal = 25;
		public int layerGiltter = 26;
		public void Init(CanvasController canvas)
		{
			this.canvas = canvas;
			UpdateMeshSize();
		}

		void UpdateMeshSize()
		{
			MeshFilter mf = GOUtil.CreateComponentIfNoExists<MeshFilter>(gameObject);
			Mesh mesh = MeshUtil.CreatePlaneMesh(canvas.Width, canvas.Height);
			mf.mesh = mesh;
			MeshRenderer mr = GOUtil.CreateComponentIfNoExists<MeshRenderer>(gameObject);
			material = new Material(Shader.Find("Unlit/Transparent"));
			material.mainTexture = canvas.OutlineTexture;
			mr.material = material;

			SetLayer();
		}

		public void SetNewSize()
		{
			MeshFilter mf = GOUtil.CreateComponentIfNoExists<MeshFilter>(gameObject);
			MeshUtil.ChangeMeshSize(mf.mesh, canvas.Width, canvas.Height);
			material.mainTexture = canvas.OutlineTexture;
		}

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
	}
}
