using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeInspector;
using PaintCraft.Utils;
using UnityEngine.Serialization;


namespace PaintCraft.Tools.Filters.Randomizers
{
	/// <summary>
	/// Set random offset to point _TileTex texture
	/// </summary>
	[NodeMenuItemAttribute("Material/RandomTileTexOffsetOnBegining")]
	public class RandomTileTexOffsetOnBegining : FilterWithNextNode
	{
		[EnumFlags]
		public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;

		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody(BrushContext brushLineContext)
		{
			if (brushLineContext.IsFirstPointInLine
				&& brushLineContext.BasePoints.First.Value.IsBasePoint
				&& brushLineContext.BasePoints.First.Value.BasePointId == 0)
			{
				Vector2 offset = new Vector2(Random.value * 100.0f, Random.value * 100.0f);
				brushLineContext.ForEachUncopiedToCanvasPoint(PointType,
					point => point.Material.SetTextureOffset("_TileTex", offset));
			}


			return true;
		}
		#endregion
	}
}