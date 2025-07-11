using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using NodeInspector;
using PaintCraft.Utils;

namespace PaintCraft.Tools.Filters.MaterialFilter
{
	public enum TextureName
	{
		Main,
		Tile
	}


	/// <summary>
	/// Set point texture ratio property the same as pageconfig size
	/// </summary>
	[NodeMenuItemAttribute("Material/SyncTextureRatioWithCanvas")]
	public class SyncTextureRatioWithCanvas : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;
		public TextureName TextureNameInShader = TextureName.Main;
		public float HorizontalScale = 1.0f;
		public float VerticalScale = 1.0f;
		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			string textureName = TextureNameInShader == TextureName.Main ? "_MainTex" : "_TileTex";

			Texture materialTexture;
			float ratioX;
			float ratioY;
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point =>
				{
					materialTexture = point.Material.GetTexture(textureName);
					Assert.IsNotNull(materialTexture, textureName + " texture must be set");
					ratioX = brushLineContext.Canvas.Width / (float)materialTexture.width / HorizontalScale;
					ratioY = brushLineContext.Canvas.Height / (float)materialTexture.height / VerticalScale;
					point.Material.SetTextureScale(textureName, new Vector2(ratioX, ratioY));
				});
			return true;
		}
		#endregion

	}
}