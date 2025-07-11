using System.Collections.Generic;
using UnityEngine;
using NodeInspector;
using UnityEngine.Assertions;

// ReSharper disable TooWideLocalVariableScope

namespace PaintCraft.Tools.Filters.Interpolators
{
	[NodeMenuItem("Interpolators/LinearInterpolator")]
	public class LinearInterpolator : FilterWithNextNode
	{
		public SpacingProperty Spacing;



		#region implemented abstract members of FilterWithNextNode

		public override bool FilterBody(BrushContext brushLineContext)
		{
			float spacing = Spacing.GetSpacingValue(brushLineContext);

			if (spacing <= 0.0f)
			{
				Debug.LogError("Spacing must be greater than 0 (setup on canvas)");
				return false;
			}

			if (brushLineContext.BasePoints.Count < 1)
			{
				return false;
			}


			if (brushLineContext.IsFirstPointInLine)
			{
				//handle single input point
				LinkedListNode<Point> clone = brushLineContext.GetFirstBasePointNormalClone();
				clone.Value.Status = PointStatus.ReadyToApply;
				brushLineContext.Points.AddLast(clone);
				return true;
			}


			LinkedListNode<Point> lastCopiedToCanvasPoint = brushLineContext.Points.First;
			Assert.IsNotNull(lastCopiedToCanvasPoint, "can't find previous point");

			LinkedListNode<Point> firstPointFromList = brushLineContext.BasePoints.Last.Previous;

			Assert.IsNotNull(firstPointFromList, "error here, can't find start base point");
			List<LinkedListNode<Point>> interpolatedPoints
				= GetInterpolatedPoints(spacing, lastCopiedToCanvasPoint.Value, firstPointFromList);

			for (int i = 0; i < interpolatedPoints.Count; i++)
			{
				brushLineContext.Points.AddLast(interpolatedPoints[i]);
			}
			return true;
		}


		static readonly List<LinkedListNode<Point>> _interpolatedPointsResult = new List<LinkedListNode<Point>>();
		public static List<LinkedListNode<Point>> GetInterpolatedPoints(float spacing, Point startPoint, LinkedListNode<Point> firstPointFromList)
		{

			_interpolatedPointsResult.Clear();
			Vector2 segmentVector;
			float segmentSpacing, timeDiff, velocityDiff, scaleDiff, rotationDiff;

			Vector2 sizeDiff, offsetDiff;
			LinkedListNode<Point> interpolatedNewNode, segmentEndPoint;
			LinkedListNode<Point> segmentStartPoint = firstPointFromList;
			float relativeFirst, relativeStep;
			while (segmentStartPoint != null && segmentStartPoint.Next != null)
			{
				segmentEndPoint = segmentStartPoint.Next;
				bool hasPoint = GetFirstPointAndStep(spacing, startPoint.Position, segmentStartPoint.Value.Position,
					segmentEndPoint.Value.Position, out relativeFirst, out relativeStep);
				if (hasPoint)
				{
					//find first position of interpolated point
					segmentVector = segmentEndPoint.Value.Position - segmentStartPoint.Value.Position;
					timeDiff = segmentEndPoint.Value.Time - segmentStartPoint.Value.Time;
					velocityDiff = segmentEndPoint.Value.Velocity - segmentStartPoint.Value.Velocity;
					scaleDiff = segmentEndPoint.Value.Scale - segmentStartPoint.Value.Scale;
					offsetDiff = segmentEndPoint.Value.Offset - segmentStartPoint.Value.Offset;
					sizeDiff = segmentEndPoint.Value.Size - segmentStartPoint.Value.Size;
					rotationDiff = segmentEndPoint.Value.Rotation - segmentStartPoint.Value.Rotation;
					segmentSpacing = relativeFirst;
					while (segmentSpacing <= 1.0f)
					{
						interpolatedNewNode = BrushContext.GetNormalCloneFromBasePoint(segmentEndPoint);
						interpolatedNewNode.Value.Position = segmentStartPoint.Value.Position + segmentSpacing * segmentVector;
						interpolatedNewNode.Value.Time = segmentStartPoint.Value.Time + segmentSpacing * timeDiff;
						interpolatedNewNode.Value.Velocity = segmentStartPoint.Value.Velocity + segmentSpacing * velocityDiff;
						interpolatedNewNode.Value.Scale = segmentStartPoint.Value.Scale + segmentSpacing * scaleDiff;
						interpolatedNewNode.Value.Offset = segmentStartPoint.Value.Offset + segmentSpacing * offsetDiff;
						interpolatedNewNode.Value.Size = segmentStartPoint.Value.Size + segmentSpacing * sizeDiff;
						interpolatedNewNode.Value.Rotation = segmentStartPoint.Value.Rotation + segmentSpacing * rotationDiff;
						segmentSpacing += relativeStep;
						_interpolatedPointsResult.Add(interpolatedNewNode);
						startPoint = interpolatedNewNode.Value;
					}
				}
				segmentStartPoint = segmentEndPoint;
			}
			return _interpolatedPointsResult;
		}


		#endregion


		protected override void FilterFinalizer(BrushContext brushLineContext)
		{
			brushLineContext.RemoveAllBasePointsExceptLast(1);
			brushLineContext.RemoveAllAppliedToCanvasPointsExceptLast();
			brushLineContext.RemoveAllTempPoints();
		}


		#region varables used by GetFirstPointAndStep function		
		static float _f2_lenght;
		static float _f2_sinAB;
		static float _f2_cosAB;
		static float _f2_u;
		static float _f2_v;
		static float _f2_distanceProjection;
		static float _f2_closePoint;
		static float _f2_farPoint;
		static float _f2_resultPoint;
		#endregion

		public static bool GetFirstPointAndStep(float distance, Vector2 previousPointPosition, Vector2 vectorStart, Vector2 vectorEnd,
			out float relativeFirstPoint, out float relativeStep)
		{
			relativeFirstPoint = 0;
			relativeStep = 0;

			_f2_lenght = Vector2.Distance(vectorStart, vectorEnd);
			_f2_sinAB = (vectorEnd.y - vectorStart.y) / _f2_lenght;
			_f2_cosAB = (vectorEnd.x - vectorStart.x) / _f2_lenght;
			// projection prevPoint to segment
			_f2_u = _f2_sinAB * (previousPointPosition.y - vectorStart.y) + _f2_cosAB * (previousPointPosition.x - vectorStart.x);
			_f2_v = _f2_sinAB * (previousPointPosition.x - vectorStart.x) - _f2_cosAB * (previousPointPosition.y - vectorStart.y);

			if (Mathf.Abs(_f2_v) > distance)
			{
				return false;
			}
			_f2_distanceProjection = Mathf.Sqrt(distance * distance - _f2_v * _f2_v);
			_f2_closePoint = _f2_u - _f2_distanceProjection;
			_f2_farPoint = _f2_u + _f2_distanceProjection;
			_f2_resultPoint = 0;
			if (_f2_closePoint >= 0 && _f2_closePoint <= _f2_lenght)
			{
				_f2_resultPoint = _f2_closePoint;
			}
			else if (_f2_farPoint >= 0 && _f2_farPoint <= _f2_lenght)
			{
				_f2_resultPoint = _f2_farPoint;
			}
			else
			{
				return false;
			}

			relativeFirstPoint = _f2_resultPoint / _f2_lenght;
			relativeStep = distance / _f2_lenght;
			return true;
		}

	}
}