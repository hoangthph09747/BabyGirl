using UnityEngine;
using PaintCraft.Tools;
using System.Collections.Generic;
using PaintCraft.Utils;
using NodeInspector;
using System;


namespace PaintCraft.Tools.Filters.MaterialFilter{

    [Obsolete("please use RenderSwatchWithPointMaterial")]
    [NodeMenuItem("Renderer/RenderSwatch (Obsolete, use RenderSwatchWithPointMaterial)")]
    public class RenderSwatch : FilterWithNextNode {
        public Material NormalMaterial;
        public Material RegionMaterial;        


		int lastUsedFrame = -1;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody (BrushContext brushLineContext)
		{
			LinkedListNode<Point> point = null;

			if (Time.frameCount != lastUsedFrame){
				FlushUsedMeshes();
			}
			point = brushLineContext.Points.Last;
			Mesh mesh;
            Material mat;

							


			while (point != null ){
				if (!point.Value.IsBasePoint){
                    Vector3 pointPosition = point.Value.Position;
                    pointPosition.z = brushLineContext.Canvas.transform.position.z + brushLineContext.Canvas.BrushOffset;
                    mesh = GetMesh(brushLineContext, point.Value);
                    mat = MaterialCache.GetMaterial(brushLineContext, NormalMaterial, RegionMaterial);

					//ngocdu bucket partern color
					//if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
					//{
					//	if (DrawPictureControllerASMR.Instance.typePen == TypePen.Parttern)
					//	{
					//		if (mat.HasTexture("_TileTex"))
					//			mat.SetTexture("_TileTex", brushLineContext.LineConfig.Texture);
					//	}
					//	else
     //                   {
					//		if (mat.HasTexture("_TileTex"))
					//			mat.SetTexture("_TileTex", null);
					//	}							
					//}

					//if (mat.HasTexture("_TileTex"))
					if (mat.HasProperty("_TileTex") )
						mat.SetTexture("_TileTex", brushLineContext.LineConfig.Texture);

					if (point.Value.Status == PointStatus.ReadyToApply){
                        /*   Graphics.DrawMesh(mesh, pointPosition, Quaternion.Euler(0,0, point.Value.Rotation), mat, 
                               brushLineContext.Canvas.BrushLayerId, brushLineContext.Canvas.CanvasCameraController.Camera);  */
                        // Tạo Matrix4x4 từ position, rotation và scale
                        Matrix4x4 matrix = Matrix4x4.TRS(
                            pointPosition,
                            Quaternion.Euler(0, 0, point.Value.Rotation),
                            Vector3.one // scale = 1 nếu không có scale riêng
                        );

                        // MaterialPropertyBlock rỗng (nếu không dùng properties riêng)
                        MaterialPropertyBlock mpb = null;

                        // Gọi DrawMesh mới
                        Graphics.DrawMesh(
                            mesh,
                            matrix,
                            mat,
                            brushLineContext.Canvas.BrushLayerId,
                            brushLineContext.Canvas.CanvasCameraController.Camera,
                            0,      // submeshIndex = 0
                            mpb     // MaterialPropertyBlock (null nếu không cần)
                        );


                        point.Value.Status = PointStatus.CopiedToCanvas;                                 
                        usedMeshes.Enqueue(mesh);
					} else if (point.Value.Status == PointStatus.Temporary){                                                						
                        Graphics.DrawMesh(mesh, pointPosition, Quaternion.Euler(0,0, point.Value.Rotation), mat, 
                            brushLineContext.Canvas.TempRenderLayerId);
						usedMeshes.Enqueue(mesh);
					}
				}

				point = point.Previous;
			}

            if (brushLineContext.IsLastPointInLine){
                meshPool.Clear();
                usedMeshes.Clear();
            }
			return true;
		}

        Queue<Mesh> meshPool = new Queue<Mesh>();
        Queue<Mesh> usedMeshes = new Queue<Mesh>();
		Mesh GetMesh(BrushContext brushLineContext, Point point){
			float width = point.Size.x * point.Scale;
			float height = point.Size.y * point.Scale;
			width = Mathf.Max(brushLineContext.Brush.MinSize.x, width);
			height = Mathf.Max(brushLineContext.Brush.MinSize.y, height);
			Mesh result;
			if (meshPool.Count > 0){
				result = meshPool.Dequeue();
				MeshUtil.ChangeMeshSize(result, width, height);
			} else {
                result = MeshUtil.CreatePlaneMesh(width, height);
            }
			MeshUtil.ChangeMeshColor(result,  point.PointColor.Color);
            MeshUtil.UpdateMeshUV2(result, width, height, point.Position, point.Rotation, (float)brushLineContext.Canvas.Width, (float)brushLineContext.Canvas.Height, brushLineContext.Canvas.transform.position);
			return result;

		}

		void FlushUsedMeshes(){            
			while (usedMeshes.Count > 0){
				meshPool.Enqueue(usedMeshes.Dequeue());
			}
            lastUsedFrame = Time.frameCount;
		}

		#endregion
	}
}
