using PaintCraft.Controllers;
using System.Collections.Generic;
using UnityEngine;
using PaintCraft.Tools;
using UnityEngine.Assertions;

namespace PaintCraft.Tools
{
	public partial class BrushContext
	{
		//todo: maybe remove this prop ?
		public CanvasController Canvas { get; private set; }
		public Brush Brush
		{
			get
			{
				return LineConfig.Brush;
			}
		}
		public LineConfig LineConfig { get; private set; }
		public LinkedList<Point> Points { get; private set; }
		public LinkedList<Point> BasePoints { get; private set; }

		public Material Material { get; private set; }

		static Queue<LinkedListNode<Point>> PointPool = new Queue<LinkedListNode<Point>>();
		public InputController SourceInputHandler { get; private set; }

		public Vector2 FirstPointPixelPosition;
		public Vector2 FirstPointUVPosition;

		public Vector2 ClippingMaskOffset;

		//this value is true only for first point. usually used in regional line to specify regions.
		public bool IsFirstPointInLine;

		//this value is true only for latest point.
		public bool IsLastPointInLine;

		//if we have multitouch this method will show is this point made by final fingerId
		public bool IsFinalAcrossAllTouches
		{
			get
			{
				int maxTouchId = int.MinValue;
				foreach (var key in SourceInputHandler.ContextByLineId.Keys)
				{
					if (key > maxTouchId)
					{
						maxTouchId = key;
					}
				}
				return SourceInputHandler.ContextByLineId[maxTouchId] == this;
			}
		}


		public BrushContext(CanvasController canvas, LineConfig lineConfig, InputController sourceInputHandler)
		{
			this.Canvas = canvas;
			this.SourceInputHandler = sourceInputHandler;
			Points = new LinkedList<Point>();
			BasePoints = new LinkedList<Point>();
			LineConfig = lineConfig;
		}


		/// <summary>
		/// return clone of the last base point
		/// </summary>
		/// <returns>clone of the last base point</returns>
		public LinkedListNode<Point> GetLastBasePointNormalClone()
		{
			return GetNormalCloneFromBasePoint(BasePoints.Last);
		}


		/// <summary>
		/// retrun clone of the first base point
		/// </summary>
		/// <returns>clone of the first base point</returns>
		public LinkedListNode<Point> GetFirstBasePointNormalClone()
		{
			return GetNormalCloneFromBasePoint(BasePoints.First);
		}


		/// <summary>
		/// This method get point and return deep copy clone (like shallow copy but point contains just struct)
		/// </summary>
		/// <param name="basePoint"></param>
		/// <returns>clone of the point</returns>
		public static LinkedListNode<Point> GetNormalCloneFromBasePoint(LinkedListNode<Point> basePoint)
		{
			LinkedListNode<Point> baseClone = BrushContext.GetPointNode();
			baseClone.Value.CopyFrom(basePoint.Value);
			baseClone.Value.IsBasePoint = false;
			return baseClone;
		}

		static public LinkedListNode<Point> GetPointNode()
		{
			LinkedListNode<Point> result = null;
			if (PointPool.Count > 0)
			{
				result = PointPool.Dequeue();
				result.Value.Reset();
			}
			else
			{
				result = new LinkedListNode<Point>(new Point());
			}
			return result;
		}

		private bool IsDistanceBetweenLastTwoBasePointLessThan(float distance)
		{
			if (Points.Count > 1)
			{
				float vectDistance = Vector2.Distance(BasePoints.Last.Value.Position, BasePoints.Last.Previous.Value.Position);
				return vectDistance < distance;
			}
			return false;
		}

		/// <summary>
		/// Removes the last point if distance to previous less than.
		/// </summary>
		/// <returns><c>true</c>,if latest point has been removed <c>false</c> otherwise.</returns>
		/// <param name="distance">Distance.</param>
		public bool RemoveLastBasePointIfDistanceToPreviousBasePointLessThan(float distance)
		{
			if (IsDistanceBetweenLastTwoBasePointLessThan(distance))
			{
				RemoveLastNodePointAndReturnToPull();
				return true;
			}
			return false;
		}


		public void RemoveLastNodePointAndReturnToPull()
		{
			if (Points.Count > 0)
			{
				ReleasePointNode(Points.Last);
				Points.RemoveLast();
			}
			else
			{
				Debug.LogWarning("points list is empty");
			}
		}

		public void RemoveLastBaseNodePointAndReturnToPull()
		{
			if (BasePoints.Count > 0)
			{
				ReleasePointNode(BasePoints.Last);
				BasePoints.RemoveLast();
			}
			else
			{
				Debug.LogWarning("base points list is empty");
			}
		}

		public void RemoveFirstNodePointAndReturnToPull()
		{
			if (Points.Count > 0)
			{
				ReleasePointNode(Points.First);
				Points.RemoveFirst();
			}
		}

		public void RemoveFirstBaseNodePointAndReturnToPull()
		{
			if (BasePoints.Count > 0)
			{
				ReleasePointNode(BasePoints.First);
				BasePoints.RemoveFirst();
			}
		}

		public void RemoveAllAppliedToCanvasPointsExceptLast()
		{
			LinkedListNode<Point> point = Points.First;
			while (point != null && point.Value.Status == PointStatus.CopiedToCanvas
				   && point.Next != null && point.Next.Value.Status == PointStatus.CopiedToCanvas)
			{
				point = point.Next;
				RemoveFirstNodePointAndReturnToPull();
			}
		}

		public void RemoveAllTempPoints()
		{
			LinkedListNode<Point> point = Points.Last;
			while (point != null && point.Value.Status == PointStatus.Temporary)
			{
				point = point.Previous;
				RemoveLastNodePointAndReturnToPull();
			}
		}


		public void RemoveAllBasePointsExceptLast(int numberOfPointsToLeave)
		{
			Assert.IsTrue(numberOfPointsToLeave > 0);
			while (BasePoints.Count > numberOfPointsToLeave)
			{
				RemoveFirstBaseNodePointAndReturnToPull();
			}
		}



		/// <summary>
		/// reset point and basepoint lists
		/// </summary>
		private void Reset()
		{
			ClearPointList(Points);
			ClearPointList(BasePoints);
		}

		/// <summary>
		/// clear linked list and put internal points back to common pool
		/// </summary>
		/// <param name="list"></param>
		private void ClearPointList(LinkedList<Point> list)
		{
			while (list.Last != null)
			{
				ReleasePointNode(list.Last);
				list.RemoveLast();
			}
		}

		/// <summary>
		/// reset point values and put it back to pool
		/// </summary>
		/// <param name="node"></param>
		private static void ReleasePointNode(LinkedListNode<Point> node)
		{
			node.Value.Reset();
			PointPool.Enqueue(node);
		}


		public override string ToString()
		{
			string result = " ---- " + Time.timeSinceLevelLoad.ToString() + " ----- \n";
			LinkedListNode<Point> node = Points.First;

			while (node != null)
			{
				//if (node.Value.IsBasePoint){
				result += node.Value.ToString() + "\n";
				//}
				node = node.Next;
			}
			return result;
		}



		#region Drawing implementation

		float pointTime, previousPointTime, timeDiff;
		Vector2 previouPointPosition;
		int pointId = 0;
		bool firstPointSetup = false;


		public void ResetBrushContext()
		{
			Reset();
			pointId = 0;
			pointTime = Time.realtimeSinceStartup;
			firstPointSetup = true;
		}


		public void AddPoint(Vector2 worldPoint)
		{
			if (firstPointSetup)
			{
				firstPointSetup = false;
				FirstPointPixelPosition = worldPoint - (Vector2)Canvas.transform.position;
				FirstPointUVPosition = new Vector2((worldPoint.x - Canvas.transform.position.x) / Canvas.Width + 0.5f
					, (worldPoint.y - Canvas.transform.position.y) / Canvas.Height + 0.5f);
				IsFirstPointInLine = true;
			}
			else
			{
				IsFirstPointInLine = false;
			}

			var node = GetPointNode();
			node.Value.BasePointId = pointId++;
			node.Value.IsBasePoint = true;
			node.Value.Position = worldPoint;
			node.Value.Scale = 1.0f;
			node.Value.Size = Brush.BaseSize;
			node.Value.Time = Time.realtimeSinceStartup - pointTime;
			if (node.Value.BasePointId == 0)
			{
				node.Value.Velocity = 0.0f;
			}
			else
			{
				timeDiff = node.Value.Time - previousPointTime;
				node.Value.Velocity = Vector2.Distance(previouPointPosition, worldPoint) / Mathf.Clamp(timeDiff, 0.0f, 0.05f);
			}
			BasePoints.AddLast(node);

			previouPointPosition = worldPoint;
			previousPointTime = node.Value.Time;
		}

		public void ApplyFilters(bool lastPoint)
		{
			IsLastPointInLine = lastPoint;
			Brush.StartFilter.Apply(this);
		}
		#endregion
	}
}