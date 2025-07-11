using NodeInspector;
using UnityEngine;

namespace PaintCraft.Tools.Filter.ChangePoint{
	/// <summary>
	/// Remove duplicate last input positions.
	/// if we received the same position from Input. just remove last so we will have just one point with that position
	/// </summary>
    [NodeMenuItem("ChangePoint/RemoveDuplicateLastInputPositions")]
    public class RemoveDuplicateLastInputPositions : FilterWithNextNode {
		#region implemented abstract members of FilterWithNextNode

		public override bool FilterBody (BrushContext brushLineContext)
		{
            if (brushLineContext.BasePoints.Count > 1 
                && brushLineContext.BasePoints.Last.Value.Position == brushLineContext.BasePoints.Last.Previous.Value.Position){
                brushLineContext.RemoveLastBaseNodePointAndReturnToPull();	            
	            return false;
            }

			return true;
		}

		#endregion



	}
}