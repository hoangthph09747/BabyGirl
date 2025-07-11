using UnityEngine;
using NodeInspector;
using System.Collections.Generic;
using PaintCraft.Utils;


namespace PaintCraft.Tools.Filters.MaterialFilter
{
	/// <summary>
	/// Bind lineconfig texture to _TileTex of point material
	/// </summary>
	[NodeMenuItem("Material/BindTileTexToLineConfig")]
	public class BindTileTexToLineConfig : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point => point.Material.SetTexture("_TileTex", brushLineContext.LineConfig.Texture));
			return true;
		}
		#endregion

	}
}