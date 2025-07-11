using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace PaintCraft.Tools
{
	public partial class BrushContext
	{

		/// <summary>
		/// Make action on each point which has pointStatus
		/// </summary>
		/// <param name="action">Action which perform on every point</param>
		/// <param name="pointStatus">only perform action which has Type</param>
		/// <param name="pointType">is it base or interpolated point</param>
		public void ForEachUncopiedToCanvasPoint(PointType pointType, Action<Point> action)
		{
			if ((pointType & PointType.InterpolatedPoint) == PointType.InterpolatedPoint)
			{
				IterateNodePoint(Points.Last, PointStatus.NotCopiedToCanvas, action);
			}
			if ((pointType & PointType.BasePoint) == PointType.BasePoint)
			{
				IterateNodePoint(BasePoints.Last, PointStatus.NotCopiedToCanvas, action);
			}
		}


		private static void IterateNodePoint(LinkedListNode<Point> nodePoint, PointStatus pointStatus, Action<Point> action)
		{
			while (nodePoint != null && (nodePoint.Value.Status & pointStatus) == nodePoint.Value.Status)
			{
				action(nodePoint.Value);
				nodePoint = nodePoint.Previous;
			}
		}
	}
}