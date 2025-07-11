using UnityEngine;
using System.Collections.Generic;
using NodeInspector;
using PaintCraft.Utils;


namespace PaintCraft.Tools.Filters.ChangePoint{
	
    [NodeMenuItem("ChangePoint/SetFixedAlpha")]
    public class SetFixedAlpha : FilterWithNextNode {
	    [EnumFlags]
	    public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;
	    [Range(0f,1f)]
		public float Alpha;
		#region implemented abstract members of FilterWithNextNode

		public override bool FilterBody (BrushContext brushLineContext)
		{
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point => point.PointColor.Alpha = Alpha);			
			return true;
		}

		#endregion


	}
}