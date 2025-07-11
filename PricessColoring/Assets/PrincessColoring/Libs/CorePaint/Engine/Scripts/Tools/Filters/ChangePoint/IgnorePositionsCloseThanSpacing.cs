using NodeInspector;


namespace PaintCraft.Tools.Filter.ChangePoint
{
	/// <summary>
	/// Check distance to previous BasePoint less than Spacing
	/// </summary>
	[NodeMenuItem("ChangePoint/IgnorePositionsCloseThanSpacing")]
	public class IgnorePositionsCloseThanSpacing : FilterWithNextNode
	{
		public SpacingProperty Spacing;
		#region implemented abstract members of FilterWithNextNode

		public override bool FilterBody(BrushContext brushLineContext)
		{
			bool latestPointRemoved = brushLineContext.RemoveLastBasePointIfDistanceToPreviousBasePointLessThan(Spacing.GetSpacingValue(brushLineContext));
			return !latestPointRemoved;
		}

		#endregion


	}
}