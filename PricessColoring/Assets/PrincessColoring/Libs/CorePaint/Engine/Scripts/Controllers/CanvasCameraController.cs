﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Collections;
using System;


namespace PaintCraft.Controllers
{
	[RequireComponent(typeof(Camera))]
	public class CanvasCameraController : MonoBehaviour
	{
		public Camera Camera { get; private set; }
		[NonSerialized]
		public bool Initialized = false;
		CanvasController canvasCtrl;

		CommandBuffer commandBuffer;
		public CommandBuffer CommandBuffer
		{
			get
			{
				return commandBuffer;
			}
		}

		public void Init(CanvasController canvas)
		{
            /*Camera = GetComponent<Camera>();
			if (Camera == null)
			{
				Camera = gameObject.AddComponent<Camera>();
			}

			commandBuffer = new CommandBuffer();
			commandBuffer.name = "paintcraft canvas";
			Camera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);


			Camera.orthographic = true;
			Camera.clearFlags = CameraClearFlags.Color;
			Camera.backgroundColor = Color.black;
			Camera.orthographicSize = (float)canvas.Height / 2.0f;
			Camera.aspect = (float)canvas.Width / (float)canvas.Height;


			Camera.clearFlags = CameraClearFlags.Nothing;
			canvasCtrl = canvas;
			Camera.targetTexture = canvasCtrl.BackLayerController.RenderTexture;
			Initialized = true;*/
            Camera = GetComponent<Camera>();
            if (Camera == null)
            {
                Camera = gameObject.AddComponent<Camera>();
            }


            Camera.orthographic = true;
            Camera.clearFlags = CameraClearFlags.Nothing;
            Camera.backgroundColor = Color.black;
            Camera.orthographicSize = (float)canvas.Height / 2.0f;
            Camera.aspect = (float)canvas.Width / (float)canvas.Height;

            canvasCtrl = canvas;
            if (canvasCtrl.BackLayerController != null && canvasCtrl.BackLayerController.RenderTexture != null)
            {
                Camera.targetTexture = canvasCtrl.BackLayerController.RenderTexture;
            }
            Initialized = true;
			//Debug.Log("CanvasCameraController initialized with Camera: ");
        }

		public void SetNewSize()
		{
			Camera.targetTexture = canvasCtrl.BackLayerController.RenderTexture;
			Camera.targetTexture = canvasCtrl.BackLayerController.RenderTexture;
			Camera.orthographicSize = (float)canvasCtrl.Height / 2.0f;
			Camera.aspect = (float)canvasCtrl.Width / (float)canvasCtrl.Height;
		}


		bool clearCanvasBGTexture = false;
		System.Action onClearDone;
		public void ClearRenderTexture()
		{
			clearCanvasBGTexture = true;
		}

		public void ClearRenderTextureWithAction(System.Action onClearDone)
		{
			clearCanvasBGTexture = true;
			this.onClearDone = onClearDone;
		}

		void OnPostRender()
		{
			if (clearCanvasBGTexture)
			{
				clearCanvasBGTexture = false;
				if(canvasCtrl)
				ClearCanvasBGTexture();
			}

		}


		void OnEnable()
		{
			StartCoroutine(ClearCommandBuffer());
		}

		void OnDisable()
		{
			StopCoroutine(ClearCommandBuffer());
		}

		IEnumerator ClearCommandBuffer()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();
				if(commandBuffer != null)
				commandBuffer.Clear();
			}
		}

		void ClearCanvasBGTexture()
		{
			AnalyticsWrapper.CustomEvent("ClearCanvas", new Dictionary<string, object>());
			RenderTexture currentRT = RenderTexture.active;
			if (Camera == null)
			{
				Camera = GetComponent<Camera>();
				if (Camera == null)
				{
					Camera = gameObject.AddComponent<Camera>();
				}
			}
			RenderTexture.active = Camera.targetTexture;
			if (canvasCtrl.PageConfig.StartImageTexture == null)
			{
				//Camera.targetTexture.DiscardContents();
				GL.Clear(false, true, canvasCtrl.DefaultBGColor);
			}
			else
			{
				Graphics.Blit(canvasCtrl.PageConfig.StartImageTexture, Camera.targetTexture);
			}
			RenderTexture.active = currentRT;
			if (onClearDone != null)
			{
				onClearDone();
				onClearDone = null;
			}
		}
	}
}
