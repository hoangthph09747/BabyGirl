using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using PaintCraft.Tools;
using PaintCraft.Canvas;
using UnityEngine.EventSystems;


namespace PaintCraft.Controllers
{
    public abstract class InputController : MonoBehaviour
    {
        public CanvasController Canvas;

        Dictionary<int, BrushContext> contextByLineId = new Dictionary<int, BrushContext>();

        public Dictionary<int, BrushContext> ContextByLineId
        {
            get { return contextByLineId; }
        }

        /// <summary>
        /// Donts the allow interaction. This check works only if you use line methods with screen coordinates
        /// </summary>
        /// <returns><c>true</c>, if allow interaction was donted, <c>false</c> otherwise.</returns>
        /// <param name="worldPosition">Input position.</param>
        public abstract bool DontAllowInteraction(Vector2 worldPosition);

        Vector2 previousPosition;

        protected bool _automakeSnapshotOnLineEnd = true;


        /// <summary>
        /// Begins the line.
        /// </summary>
        /// <param name="lineConfig">Line config.</param>
        /// <param name="lineId">Line identifier.</param>
        /// <param name="inputPosition">Input position (in world space).</param>
        /// <param name="interactionAllowCheck">Set it to true if this input called from camera and then override DontAllowInteraction method to prevent event handling if cursor e.g. on top of UI element</param>
        public void BeginLine(LineConfig lineConfig, int lineId, Vector2 inputPosition,
            bool interactionAllowCheck = false)
        {
            //if (GameManager.instance.typePen == TypePen.Stamp)
            //    return;
           // Debug.Log("Touch Began");
            if (EventSystem.current == null)
            {
                Debug.LogError("you have to add event system to the scene. e.g. from Unity UI");
                return;
            }

            if (finalSnapshotInProgress)
            {
                return;
            }

            if (interactionAllowCheck && DontAllowInteraction(inputPosition))
            {
                return; // handle on different camera or ignore
            }

            if (ContextByLineId.ContainsKey(lineId))
            {
                // Debug.Log("enline 2");
                EndLine(lineId, inputPosition);
                // EndLine2(lineId, inputPosition);
            }

            AnalyticsWrapper.CustomEvent("TouchBegan", new Dictionary<string, object>
            {
                {"HandlerName", gameObject.name},
                {"ToolName", lineConfig.Brush.name},
                {"TouchId", lineId} /*,
                { "TotalTouch", e.Touches.Count}*/
            });


            BrushContext bc = new BrushContext(Canvas, lineConfig, this);
            if (ContextByLineId.Count == 0)
            {
                // if(GameManager.instance.drawAble)
                StoreStateBeforeSnapshot();
            }

            ContextByLineId.Add(lineId, bc);
            bc.ResetBrushContext();

            bc.AddPoint(inputPosition);
            bc.ApplyFilters(false);
            previousPosition = inputPosition;
            lineTerminated = false;
        }


        bool lineTerminated = false;

        public void ContinueLine(int lineId, Vector2 inputPosition, bool checkCoordInRect = true)
        {
            // if (GameManager.instance.typePen == TypePen.Stamp)
            //   return;

            if (!contextByLineId.ContainsKey(lineId) || finalSnapshotInProgress)
            {
                // Debug.Log("finalSnapshotInProgress");
                return; //initiated on different camera
            }

            BrushContext bc = contextByLineId[lineId];
            if (checkCoordInRect)
            {
                if (!Canvas.isCoordWithinRect(inputPosition))
                {
                    // Debug.Log("isCoordWithinRect false");
                    if (lineTerminated)
                    {
                        return;
                    }
                    else
                    {
                        // Debug.Log("EndLine 10");
                        EndLine(lineId, previousPosition);
                        // EndLineOneTouch(lineId, previousPosition);
                        lineTerminated = true;
                    }
                }
                else
                {
                    if (lineTerminated)
                    {
                        // Debug.Log("isCoordWithinRect true");
                        BeginLine(bc.LineConfig, lineId, inputPosition, false);
                        return;
                    }
                }
            }

            bc.AddPoint(inputPosition);
            bc.ApplyFilters(false);
            previousPosition = inputPosition;
        }


        public void EndLine(int lineId, Vector2 inputPosition)
        {
            // if (GameManager.instance.typePen == TypePen.Stamp)
            //   return;

            if (!contextByLineId.ContainsKey(lineId) || finalSnapshotInProgress)
            {
                return; //initiated on different camera
            }

            BrushContext bc = contextByLineId[lineId];

            AnalyticsWrapper.CustomEvent("TouchEnbded", new Dictionary<string, object>
            {
                {"HandlerName", gameObject.name},
                {"ToolName", bc.LineConfig.Brush.name},
                {"TouchId", lineId} /*,
                { "TotalTouch", e.Touches.Count}*/
            });

            contextByLineId.Remove(lineId);

            if (ContextByLineId.Count == 0 && _automakeSnapshotOnLineEnd)
            {
                StartCoroutine(MakeSnapshot());
            }

        }

        //use bucket, stamp
        public void EndLineOneTouch(int lineId, Vector2 inputPosition)
        {

            if (!contextByLineId.ContainsKey(lineId) || finalSnapshotInProgress)
            {
                return; //initiated on different camera
            }

            BrushContext bc = contextByLineId[lineId];

            AnalyticsWrapper.CustomEvent("TouchEnbded", new Dictionary<string, object>
            {
                {"HandlerName", gameObject.name},
                {"ToolName", bc.LineConfig.Brush.name},
                {"TouchId", lineId} /*,
                { "TotalTouch", e.Touches.Count}*/
            });


            bc.AddPoint(inputPosition);

            bc.ApplyFilters(true);

            contextByLineId.Remove(lineId);

            //ngocdu memmory comment
            //ngocdu undo redo
            if (ContextByLineId.Count == 0 && _automakeSnapshotOnLineEnd)
            {
                StartCoroutine(MakeSnapshot());
            }

        }

        SnapshotCommand snapCommand;

        public void StoreStateBeforeSnapshot()
        {
            //Debug.Log("StoreStateBeforeSnapshot");
            snapCommand = new SnapshotCommand(Canvas.UndoManager);
            snapCommand.BeforeCommand();
        }

        //ngocdu
        public void CreateSnapshot()
        {
            // Debug.Log("MakeSnapshot 3");
            StartCoroutine(MakeSnapshot());
        }

        bool finalSnapshotInProgress = false;

        protected IEnumerator MakeSnapshot()
        {
            yield return null;
            // Debug.Log("MakeSnapshot");
            finalSnapshotInProgress = true;
            snapCommand.AfterCommand();
            Canvas.UndoManager.AddNewCommandToHistory(snapCommand);
            //Canvas.SaveChangesToDisk();
            finalSnapshotInProgress = false;
        }
    }
}