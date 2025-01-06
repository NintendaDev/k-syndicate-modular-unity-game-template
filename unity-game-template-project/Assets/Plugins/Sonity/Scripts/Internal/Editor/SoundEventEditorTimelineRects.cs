// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {
    public class SoundEventEditorTimelineRects {

        public SoundEventEditorTimeline parent;

        public void Initialize(SoundEventEditorTimeline soundEventEditorTimeline) {
            parent = soundEventEditorTimeline;
        }

        public Rect RectAddLayoutOffset(Rect rectMesh) {
            rectMesh.xMin += parent.data.layoutRectangle.xMin;
            rectMesh.xMax += parent.data.layoutRectangle.xMin;
            rectMesh.yMin += parent.data.layoutRectangle.yMin;
            rectMesh.yMax += parent.data.layoutRectangle.yMin;
            return rectMesh;
        }

        public Rect RectClampXToLayoutWidth(Rect rect) {
            rect.xMin = Mathf.Clamp(rect.xMin, 0f, parent.data.layoutRectangle.width);
            rect.xMax = Mathf.Clamp(rect.xMax, 0f, parent.data.layoutRectangle.width);
            return rect;
        }

        public void CalculateRects(int i) {

            // Set Base Rect Mesh
            parent.data.layerBaseRectMesh[i].xMin = parent.parent.mTarget.internals.data.timelineSoundContainerData[i].delay + parent.data.horizontalOffset;
            parent.data.layerBaseRectMesh[i].xMax = (parent.parent.mTarget.internals.data.timelineSoundContainerData[i].delay + parent.GetItemLength(i) + parent.data.horizontalOffset);
            parent.data.layerBaseRectMesh[i].yMin = i * parent.data.itemHeight + parent.data.itemHeightSpace + parent.data.itemTopSpace + parent.data.gridTopSpace;
            parent.data.layerBaseRectMesh[i].yMax = (i + 1) * parent.data.itemHeight - parent.data.itemHeightSpace + parent.data.gridTopSpace;
            
            // Width Zoom Scale
            parent.data.layerBaseRectMesh[i].xMin = parent.data.layerBaseRectMesh[i].xMin / parent.data.widthZoomScale;
            parent.data.layerBaseRectMesh[i].xMax = parent.data.layerBaseRectMesh[i].xMax / parent.data.widthZoomScale;

            // Set Background Rect Layout
            parent.data.layerBaseRectLayout[i] = RectAddLayoutOffset(parent.data.layerBaseRectMesh[i]);

            // Set Background Rect Mesh
            parent.data.layerBackgroundRectMesh[i] = parent.data.layerBaseRectMesh[i];

            // Clamp Sides to Layout Rect
            parent.data.layerBackgroundRectMesh[i] = RectClampXToLayoutWidth(parent.data.layerBackgroundRectMesh[i]);

            // Set Volume Rect Mesh
            parent.data.layerVolumeRectMesh[i] = parent.data.layerBaseRectMesh[i];
            parent.data.layerVolumeRectMesh[i].xMin = Mathf.Clamp(parent.data.layerVolumeRectMesh[i].xMin, 0f, parent.data.layerVolumeRectMesh[i].xMax);
            parent.data.layerVolumeRectMesh[i].yMin += parent.data.layerVolumeRectMesh[i].height * (1f - parent.parent.mTarget.internals.data.timelineSoundContainerData[i].EditorGetVolumeLinear());

            // Clamp Left and Right Side
            parent.data.layerVolumeRectMesh[i] = RectClampXToLayoutWidth(parent.data.layerVolumeRectMesh[i]);

            // Set Volume Rect Layout
            parent.data.layerVolumeRectLayout[i] = RectAddLayoutOffset(parent.data.layerVolumeRectLayout[i]);

            // Set Layer Volume Handle Rect Mesh
            parent.data.layerVolumeHandleRectMesh[i] = parent.data.layerBaseRectMesh[i];
            parent.data.layerVolumeHandleRectMesh[i].xMin = Mathf.Clamp(parent.data.layerVolumeHandleRectMesh[i].xMin, 0f, parent.data.layerVolumeHandleRectMesh[i].xMax);
            parent.data.layerVolumeHandleRectMesh[i].yMin += parent.data.layerBaseRectMesh[i].height * (1f - parent.parent.mTarget.internals.data.timelineSoundContainerData[i].EditorGetVolumeLinear());
            parent.data.layerVolumeHandleRectMesh[i].yMax += parent.data.layerBaseRectMesh[i].height * -parent.parent.mTarget.internals.data.timelineSoundContainerData[i].EditorGetVolumeLinear() + parent.data.handleWidthVolumeMesh;
            parent.data.layerVolumeHandleRectMesh[i].yMin = Mathf.Clamp(parent.data.layerVolumeHandleRectMesh[i].yMin, 0f, parent.data.layerBaseRectMesh[i].yMax - parent.data.handleWidthVolumeMesh);
            parent.data.layerVolumeHandleRectMesh[i].yMax = Mathf.Clamp(parent.data.layerVolumeHandleRectMesh[i].yMax, 0f, parent.data.layerBaseRectMesh[i].yMax);

            // Clamp Left and Right Side
            parent.data.layerVolumeHandleRectMesh[i] = RectClampXToLayoutWidth(parent.data.layerVolumeHandleRectMesh[i]);

            // Set Layer Volume Handle Rect Layout
            parent.data.layerVolumeHandleRectLayout[i] = RectAddLayoutOffset(parent.data.layerVolumeHandleRectMesh[i]);

            // Set Layer Loop Rect Mesh
            parent.data.layerLoopRectMesh[i] = parent.data.layerVolumeRectMesh[i];
            parent.data.layerLoopRectMesh[i].xMin = parent.data.layerVolumeRectMesh[i].xMax;
            parent.data.layerLoopRectMesh[i].xMax = parent.data.layoutRectangle.xMax;
        }
    }
}
#endif