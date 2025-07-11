using PaintCraft.Tools;
using NodeInspector;


namespace PaintCraft.Tools.Filters.ChangePoint{
	
	/// <summary>
	/// Set first base point status ready to apply
	/// </summary>
	[NodeMenuItem("ChangePoint/DrawFirstPoint")]
    public class DrawFirstPoint : FilterWithNextNode {        
		#region implemented abstract members of FilterWithNextNode
        public override bool FilterBody (BrushContext brushLineContext)
		{
			if (brushLineContext.BasePoints.First.Value.IsFirstBasePointFromInput){
				if (brushLineContext.BasePoints.First.Value.Status != PointStatus.CopiedToCanvas){
					if (brushLineContext.BasePoints.Count > 1){
						brushLineContext.BasePoints.First.Value.Status = PointStatus.ReadyToApply;
					} else {
						brushLineContext.BasePoints.First.Value.Status = PointStatus.Temporary;
					}
				}
			}
            return true;
		}
		#endregion
	}
}
