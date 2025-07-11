using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PaintCraft.Tools.Filter.ChangePoint;
using UnityEngine;
using NodeInspector;

namespace PaintCraft.Tools.Filters.Interpolators
{
	/// <summary>
	/// Spline interpolation. Use Catmul-Rom interpolation, until we have 4 points, use linear temp interpolation
	/// </summary>
	[NodeMenuItem("Interpolators/SplineInterpolator")]
	public class SplineInterpolator : FilterWithNextNode
	{
		public SpacingProperty Spacing;
		[NonSerialized]
		private LinearInterpolator linearInterpolator;
		[NonSerialized]
		private IgnorePositionsCloseThanSpacing ignorePositionsCloseThanSpacing;

		[NonSerialized]
		bool initialized = false;
		void InitIfRequired()
		{
			if (initialized)
			{
				return;
			}
			linearInterpolator = ScriptableObject.CreateInstance<LinearInterpolator>();
			linearInterpolator.Spacing = Spacing;
			ignorePositionsCloseThanSpacing = ScriptableObject.CreateInstance<IgnorePositionsCloseThanSpacing>();
			ignorePositionsCloseThanSpacing.Spacing = Spacing;
			initialized = true;
		}


		LinkedListNode<Point> P1, P2, P3, P4;
		int numberOfBasePoints;

		float diffX, diffY, maxD, step;
		//Point p1,p2,p3,p4;
		//Point previousBasePoint;

		Vector2 diff;
		#region implemented abstract members of FilterWithNextNode

		public override bool FilterBody(BrushContext brushLineContext)
		{
			InitIfRequired();
			if (!ignorePositionsCloseThanSpacing.FilterBody(brushLineContext))
			{
				return false;
			}

			//1st. we need to take base points.
			SetupBrushPoints(brushLineContext);
			switch (numberOfBasePoints)
			{
				case 0:
					Debug.LogWarning("No input base points here");
					return false;
				case 1:
					HandleOnePoint(brushLineContext, brushLineContext.IsLastPointInLine);
					break;
				case 2:
					HandleTwoPoints(brushLineContext, brushLineContext.IsLastPointInLine);
					break;
				case 3:
					HandleThreePoints(brushLineContext, brushLineContext.IsLastPointInLine);
					break;
				case 4:
					HandleFourPoints(brushLineContext, brushLineContext.IsLastPointInLine);
					break;
			}
			if (numberOfBasePoints > 1)
			{
				ApplyLinearInterpolation(brushLineContext);
			}
			return true;
		}

		void ApplyLinearInterpolation(BrushContext brushLineContext)
		{
			if (brushLineContext.Points.Count < 2)
			{
				Debug.LogWarning("not enough points for linear interpolation");
				return;
			}
			float spacing = Spacing.GetSpacingValue(brushLineContext);
			LinkedListNode<Point> firstPoint = brushLineContext.Points.First;
			LinkedListNode<Point> firstPointFromList = firstPoint;
			if (firstPoint.Value.Status == PointStatus.CopiedToCanvas)
			{
				firstPointFromList = firstPoint.Next;
			}
			List<LinkedListNode<Point>> interpolatedPoints
				= LinearInterpolator.GetInterpolatedPoints(spacing, firstPoint.Value, firstPointFromList);


			// clear all normal points
			while (brushLineContext.Points.Count > 1) //don't remove first in case if linear interpolation return 0 array
			{
				brushLineContext.RemoveLastNodePointAndReturnToPull();
			}

			// add interpolated points
			for (int i = 0; i < interpolatedPoints.Count; i++)
			{
				brushLineContext.Points.AddLast(interpolatedPoints[i]);
			}

		}



		protected override void FilterFinalizer(BrushContext brushLineContext)
		{
			brushLineContext.RemoveAllAppliedToCanvasPointsExceptLast();
			brushLineContext.RemoveAllBasePointsExceptLast(3);
			brushLineContext.RemoveAllTempPoints();
		}

		void HandleOnePoint(BrushContext brushLineContext, bool drawEnded)
		{
			AddBaseCloneToPointsEnd(P4, brushLineContext, !drawEnded);
		}

		void AddBaseCloneToPointsEnd(LinkedListNode<Point> point, BrushContext brushLineContext, bool isTempPoint)
		{
			//brushLineContext.Points.AddLast(point);
			LinkedListNode<Point> renderPoint = BrushContext.GetPointNode();
			renderPoint.Value.CopyFrom(point.Value);
			renderPoint.Value.IsBasePoint = false;
			renderPoint.Value.Status = isTempPoint ? PointStatus.Temporary : PointStatus.ReadyToApply;
			brushLineContext.Points.AddLast(renderPoint);
		}


		void HandleTwoPoints(BrushContext brushLineContext, bool drawEnded)
		{
			AddBaseCloneToPointsEnd(P3, brushLineContext, !drawEnded);
			diff = P4.Value.Position - P3.Value.Position;
			maxD = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) * 3.0f;
			step = 1.0f / maxD;
			for (float i = step; i < 1.0f; i += step)
			{
				AddInterpolatedPoint(P3.Value, P3.Value, P4.Value, P4.Value, i, brushLineContext, !drawEnded);
			}
			AddBaseCloneToPointsEnd(P4, brushLineContext, !drawEnded);
		}

		void HandleThreePoints(BrushContext brushLineContext, bool drawEnded)
		{
			AddBaseCloneToPointsEnd(P2, brushLineContext, false);
			// P2->P3 as readyToApply (it's begining of the line
			diff = P3.Value.Position - P2.Value.Position;
			maxD = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) * 3.0f;
			step = 1.0f / maxD;
			for (float i = step; i < 1.0f; i += step)
			{
				AddInterpolatedPoint(P2.Value, P2.Value, P3.Value, P4.Value, i, brushLineContext, false);
			}
			AddBaseCloneToPointsEnd(P3, brushLineContext, false);
			DrawP3P4TempOrReadysegment(brushLineContext, drawEnded);
		}

		void HandleFourPoints(BrushContext brushLineContext, bool drawEnded)
		{

			//P1-P2 should be already there (from previous iteration)
			//brushLineContext.Points.AddLast(P1);

			//P2->P3 readyToApply here it's a middle segment
			AddBaseCloneToPointsEnd(P2, brushLineContext, false);
			diff = P3.Value.Position - P2.Value.Position;
			maxD = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) * 3.0f;
			step = 1.0f / maxD;
			for (float i = step; i < 1.0f; i += step)
			{
				AddInterpolatedPoint(P1.Value, P2.Value, P3.Value, P4.Value, i, brushLineContext, false);
			}
			AddBaseCloneToPointsEnd(P3, brushLineContext, false);

			DrawP3P4TempOrReadysegment(brushLineContext, drawEnded);
		}

		void DrawP3P4TempOrReadysegment(BrushContext brushLineContext, bool drawEnded)
		{
			//P3->P4 temp if !drawEnd
			diff = P4.Value.Position - P3.Value.Position;
			maxD = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) * 3.0f;
			step = 1.0f / maxD;
			for (float i = step; i < 1.0f; i += step)
			{
				AddInterpolatedPoint(P2.Value, P3.Value, P4.Value, P4.Value, i, brushLineContext, !drawEnded);
			}
			AddBaseCloneToPointsEnd(P4, brushLineContext, !drawEnded);
		}


		float t0, t1, t2, t3;
		void AddInterpolatedPoint(Point _p0, Point _p1, Point _p2, Point _p3, float t, BrushContext brushLineContext, bool isTempPoint)
		{
			t0 = ((-t + 2.0f) * t - 1.0f) * t * 0.5f;
			t1 = (((3.0f * t - 5.0f) * t) * t + 2.0f) * 0.5f;
			t2 = ((-3.0f * t + 4.0f) * t + 1.0f) * t * 0.5f;
			t3 = ((t - 1f) * t * t) * 0.5f;
			LinkedListNode<Point> point = BrushContext.GetPointNode();
			point.Value.Status = isTempPoint ? PointStatus.Temporary : PointStatus.ReadyToApply;
			point.Value.Position.x = t0 * _p0.Position.x + t1 * _p1.Position.x + t2 * _p2.Position.x + t3 * _p3.Position.x;
			point.Value.Position.y = t0 * _p0.Position.y + t1 * _p1.Position.y + t2 * _p2.Position.y + t3 * _p3.Position.y;


			point.Value.Time = t0 * _p0.Time + t1 * _p1.Time + t2 * _p2.Time + t3 * _p3.Time;
			point.Value.Velocity = t0 * _p0.Velocity + t1 * _p1.Velocity + t2 * _p2.Velocity + t3 * _p3.Velocity;
			point.Value.Scale = t0 * _p0.Scale + t1 * _p1.Scale + t2 * _p2.Scale + t3 * _p3.Scale;

			point.Value.Size.x = t0 * _p0.Size.x + t1 * _p1.Size.x + t2 * _p2.Size.x + t3 * _p3.Size.x;
			point.Value.Size.y = t0 * _p0.Size.y + t1 * _p1.Size.y + t2 * _p2.Size.y + t3 * _p3.Size.y;

			point.Value.Offset.x = t0 * _p0.Offset.x + t1 * _p1.Offset.x + t2 * _p2.Offset.x + t3 * _p3.Offset.x;
			point.Value.Offset.y = t0 * _p0.Offset.y + t1 * _p1.Offset.y + t2 * _p2.Offset.y + t3 * _p3.Offset.y;


			point.Value.Rotation = t0 * _p0.Rotation + t1 * _p1.Rotation + t2 * _p2.Rotation + t3 * _p3.Rotation;
			point.Value.Material = _p0.Material;
			brushLineContext.Points.AddLast(point);
		}


		void SetupBrushPoints(BrushContext brushLineContext)
		{
			int pointId = 4;
			P1 = P2 = P3 = P4 = null;
			LinkedListNode<Point> node = brushLineContext.BasePoints.Last;
			while (node != null && pointId > 0)
			{
				switch (pointId--)
				{
					case 4: P4 = node; break;
					case 3: P3 = node; break;
					case 2: P2 = node; break;
					case 1: P1 = node; break;
				}
				node = node.Previous;
			}
			numberOfBasePoints = 4 - pointId;
			P4.Value.Status = brushLineContext.IsLastPointInLine ? PointStatus.ReadyToApply : PointStatus.Temporary;
			if (numberOfBasePoints > 1)
			{
				P3.Value.Status = PointStatus.ReadyToApply;
			}

		}

		#endregion
	}

}