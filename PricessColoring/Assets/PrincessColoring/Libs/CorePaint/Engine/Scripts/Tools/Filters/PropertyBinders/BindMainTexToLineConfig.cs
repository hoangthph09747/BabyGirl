using UnityEngine;
using NodeInspector;
using System.Collections.Generic;
using PaintCraft.Utils;


namespace PaintCraft.Tools.Filters.MaterialFilter
{
	/// <summary>
	/// Take LineConfig Texture and  Set point _MainTex
	/// </summary>
	[NodeMenuItem("Material/BindMainTexToLineConfig")]
	public class BindMainTexToLineConfig : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point => point.Material.SetTexture("_MainTex", brushLineContext.LineConfig.Texture));
			return true;
		}
		#endregion
	}
}