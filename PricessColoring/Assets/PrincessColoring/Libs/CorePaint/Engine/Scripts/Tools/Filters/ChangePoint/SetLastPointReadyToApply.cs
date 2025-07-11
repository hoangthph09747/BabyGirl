using NodeInspector;
using UnityEngine.Serialization;


namespace PaintCraft.Tools.Filters.ChangePoint{
	/// <summary>
	/// Set last base point Ready to apply
	/// </summary>
    [NodeMenuItem("ChangePoint/SetLastPointReadyToApply")]	
    public class SetLastPointReadyToApply : FilterWithNextNode {
		#region implemented abstract members of FilterWithNextNode
		public override bool FilterBody (BrushContext brushLineContext)
		{
			brushLineContext.BasePoints.Last.Value.Status = PointStatus.ReadyToApply;
			return true;
		}
		#endregion
		

	}
}