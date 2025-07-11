using NodeInspector;
using PaintCraft.Utils;

namespace PaintCraft.Tools.Filter.ChangePoint{
	/// <summary>
	/// Iterate starting from FIRST POINT
	/// Remove all aplied points and left just one which has status CopiedToCanvas
	/// </summary>
    [NodeMenuItem("ChangePoint/RemoveAppliedPointsExceptLast")]
    public class RemoveAppliedPointsExceptLast : FilterWithNextNode {
	    [EnumFlags]
	    public PointType PointType = PointType.BasePoint | PointType.InterpolatedPoint;
	    #region implemented abstract members of FilterWithNextNode

		public override bool FilterBody (BrushContext brushLineContext)
		{
			if ((PointType & PointType.InterpolatedPoint) == PointType.InterpolatedPoint)
			{				
				while (brushLineContext.Points.Count > 1 
				       && brushLineContext.Points.First.Value.Status == PointStatus.CopiedToCanvas 
				       && brushLineContext.Points.First.Next != null
				       && brushLineContext.Points.First.Next.Value.Status == PointStatus.CopiedToCanvas){
					brushLineContext.RemoveFirstNodePointAndReturnToPull();
				}	
			}
			if ((PointType & PointType.BasePoint) == PointType.BasePoint)
			{
				while (brushLineContext.BasePoints.Count > 1 
				       && brushLineContext.BasePoints.First.Value.Status == PointStatus.CopiedToCanvas 
				       && brushLineContext.BasePoints.First.Next != null
				       && brushLineContext.BasePoints.First.Next.Value.Status == PointStatus.CopiedToCanvas){
					brushLineContext.RemoveFirstBaseNodePointAndReturnToPull();
				}
			}			
			return true;
		}

		#endregion



	}
}