// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sonity.Internal {

	public static class EditorDragAndDropArea {

		public static void DrawDragAndDropAreaCustomEditor<T>(DragAndDropAreaInfo dragAreaInfo, System.Action<T[]> OnPerformedDragCallback = null) where T : UnityEngine.Object {
			
			// Change color and create Drag Area
			Color originalGUIColor = GUI.color;
			if (EditorGUIUtility.isProSkin) {
				GUI.color = dragAreaInfo.colorDarkOutline;
			} else {
                GUI.color = dragAreaInfo.colorLightOutline;
            }
			EditorGUILayout.BeginVertical(GUI.skin.box);
			if (EditorGUIUtility.isProSkin) {
				GUI.color = dragAreaInfo.colorDarkBackground;
			} else {
                GUI.color = dragAreaInfo.colorLightBackground;
            }
			Rect dragArea = GUILayoutUtility.GetRect(dragAreaInfo.dragAreaWidth, dragAreaInfo.dragAreaHeight, GUILayout.ExpandWidth(true));

			// Fix the color so that when darkskin the text is white
			Color defaultGuiColor = GUI.color;
			GUIStyle guiStyleColor = new GUIStyle();
			guiStyleColor.fontStyle = FontStyle.Bold;
			guiStyleColor.alignment = TextAnchor.MiddleCenter;
			if (EditorGUIUtility.isProSkin) {
				guiStyleColor.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
			}

			GUI.Box(dragArea, new GUIContent(dragAreaInfo.dragAreaText, "Here you can drag and drop " + dragAreaInfo.draggedObjectTypeName + "s or folders with " + dragAreaInfo.draggedObjectTypeName + "s in them."), guiStyleColor);

			// If the current Editor Event is a DragAndDrop event.
			Event eventCurrent = Event.current;
			switch (eventCurrent.type) {
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!dragArea.Contains(eventCurrent.mousePosition)) {
						// Early Out in case the drop is made outside the drag area.
						break;
					}

					// Change mouse cursor icon to the "Copy" icon
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

					// If mouse is released 
					if (eventCurrent.type == EventType.DragPerform) {
						DragAndDrop.AcceptDrag();
						var draggedTypeObjects = GetDraggedObjects<T>();
						OnPerformedDragCallback?.Invoke(draggedTypeObjects);
					}

					Event.current.Use();
					break;
			}

			EditorGUILayout.EndVertical();
			GUI.color = originalGUIColor;
		}

		private static T[] GetDraggedObjects<T>() where T : UnityEngine.Object {
			List<T> draggedTypeObjects = new List<T>();

			foreach (var draggedObject in DragAndDrop.objectReferences) {
				// "DefaultAsset" is a folder in the Unity Editor
				if (draggedObject is DefaultAsset) {
					string folderPath = AssetDatabase.GetAssetPath(draggedObject);
					T[] assetsInDraggedFolders = GetAllAssetsOfTypeInDirectory<T>(folderPath);
					foreach (var asset in assetsInDraggedFolders) {
						if (draggedTypeObjects.Contains(asset)) {
							// Asset in Dragged Folder exists already
							continue;
						}

						draggedTypeObjects.Add(asset);
					}
					// Go to next index in the "DragAndDrop.objectReferences"
					continue;
				}
				// Dragged asset is a "normal" asset, ie. not a Folder
				T draggedAsset = draggedObject as T;
				if (draggedAsset == null || draggedTypeObjects.Contains(draggedAsset)) {
					// Dragged Asset is not castable to the type you wanted or already exists in the selection list
					continue;
				}
				draggedTypeObjects.Add(draggedAsset);
			}
			return draggedTypeObjects.ToArray();
		}

		public class DragAndDropAreaInfo {
			public string dragAreaText {
				get => $"Drop {draggedObjectTypeName}s/Folders Here";
            }

			public string draggedObjectTypeName = "";
			public float dragAreaWidth = 0f;
			public float dragAreaHeight = 15f;

			public Color colorDarkOutline = Color.black;
			public Color colorDarkBackground = Color.white;

            public Color colorLightOutline = new Color(0.9f, 0.9f, 0.9f, 0.75f);
            public Color colorLightBackground = new Color(1f, 1f, 1f, 1f);

            public DragAndDropAreaInfo(string draggedObjectTypeName) {
				this.draggedObjectTypeName = draggedObjectTypeName;
			}
		}

		public static T[] GetAllAssetsOfTypeInDirectory<T>(string path) where T : UnityEngine.Object {
			List<T> assetsToGet = new List<T>();

			string absolutePath = $"{Application.dataPath}/{path.Remove(0, 7)}";
			string[] fileEntries = Directory.GetFiles(absolutePath);

			foreach (string fileName in fileEntries) {
				string sanitizedFileName = fileName.Replace('\\', '/');
				int index = sanitizedFileName.LastIndexOf('/');
				string localPath = path;
				if (index > 0) {
					localPath += sanitizedFileName.Substring(index);
				}

				T assetOfType = AssetDatabase.LoadAssetAtPath<T>(localPath);
				if (assetOfType != null) {
                    assetsToGet.Add(assetOfType);
                }
			}
			return assetsToGet.ToArray();
		}
	}
}
#endif