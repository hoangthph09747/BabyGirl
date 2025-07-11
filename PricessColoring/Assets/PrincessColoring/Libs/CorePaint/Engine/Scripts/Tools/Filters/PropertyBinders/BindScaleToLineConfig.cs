using NodeInspector;
using PaintCraft.Utils;

namespace PaintCraft.Tools.Filters.PropertyBinders
{

	/// <summary>
	/// Get scale from line config and set it to Point
	/// </summary>
	[NodeMenuItem("PropertyBinders/BindScaleToLineConfig")]
	public class BindScaleToLineConfig : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point => point.Scale = brushLineContext.LineConfig.Scale);
			return true;
		}
		#endregion
	}
}