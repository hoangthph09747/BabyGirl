
using System.Collections.Generic;
using NodeInspector;

namespace PaintCraft.Tools.Filter.ChangePoint
{
    [NodeMenuItem("ChangePoint/RemoveAllTempPoints")]
    public class RemoveAllTempPoints : FilterWithNextNode
    {
        public override bool FilterBody(BrushContext brushLineContext)
        {
            LinkedListNode<Point> point = brushLineContext.Points.Last;

            while (point != null && point.Value.Status == PointStatus.Temporary)
            {
                brushLineContext.RemoveLastNodePointAndReturnToPull();
                point = point.Previous;
            }            
            return true;
        }
    }
}