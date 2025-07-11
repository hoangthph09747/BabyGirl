using NodeInspector;

namespace PaintCraft.Tools.Filters.ChangePoint
{
	[NodeMenuItem("ChangePoint/CloneLastBasePointToPoints")]
	public class CloneLastBasePointToPoints : FilterWithNextNode
	{
		public override bool FilterBody(BrushContext brushLineContext)
		{
			brushLineContext.Points.AddLast(brushLineContext.GetLastBasePointNormalClone());
			return true;
		}
	}
}