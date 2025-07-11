using System.Collections.Generic;
using NodeInspector;
using PaintCraft.Utils;

namespace PaintCraft.Tools.Filters.PropertyBinders
{

	/// <summary>
	/// Set point material the same as default canvas color
	/// </summary>
	[NodeMenuItem("PropertyBinders/BindColorToDefaultCanvasColor")]
	public class BindColorToDefaultCanvasColor : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point => point.PointColor.Color = brushLineContext.Canvas.DefaultBGColor);

			return true;
		}
		#endregion
	}
}
