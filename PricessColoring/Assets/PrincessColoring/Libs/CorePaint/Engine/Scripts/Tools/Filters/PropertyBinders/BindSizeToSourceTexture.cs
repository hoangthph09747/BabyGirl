using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintCraft.Tools;
using NodeInspector;
using PaintCraft.Utils;


namespace PaintCraft.Tools.Filters.PropertyBinders
{
	/// <summary>
	/// Set point size the same as point material main texture size
	/// </summary>
	[NodeMenuItemAttribute("PropertyBinders/BindSizeToMainTexture")]
	public class BindSizeToSourceTexture : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
				point =>
				{
					point.Size.x = point.Material.mainTexture.width;
					point.Size.y = point.Material.mainTexture.height;
				});

			return true;
		}
		#endregion

	}
}
