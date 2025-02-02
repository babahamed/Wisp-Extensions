﻿using System;
using UnityEngine;

namespace WispExtensions
{
    public static class WispRectTransform
    {
        public static Vector3 GetAnchoredPostionIn(this RectTransform ParamMe, RectTransform ParamTarget)
        {
            Transform originalParent = ParamMe.parent;

            ParamMe.SetParent(ParamTarget);

            Vector3 result = ParamMe.anchoredPosition3D;

            if (originalParent != null)
                ParamMe.SetParent(originalParent);

            return result;
        }

        public static Vector2 GetMyPositionInAnotherRectTransform(this RectTransform ParamMe, RectTransform ParamTarget)
        {
            Vector2 localPoint;
            Vector2 fromPivotDerivedOffset = new Vector2(ParamMe.rect.width * ParamMe.pivot.x + ParamMe.rect.xMin, ParamMe.rect.height * ParamMe.pivot.y + ParamMe.rect.yMin);
            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, ParamMe.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ParamTarget, screenP, null, out localPoint);
            Vector2 pivotDerivedOffset = new Vector2(ParamTarget.rect.width * ParamTarget.pivot.x + ParamTarget.rect.xMin, ParamTarget.rect.height * ParamTarget.pivot.y + ParamTarget.rect.yMin);
            return ParamTarget.anchoredPosition + localPoint - pivotDerivedOffset;
        }

        // For a RectTransform in a Canvas set to Screen Space - Overlay mode, the camera parameter should be null.
        public static Vector2 GetMousePositionInRectTransform(RectTransform ParamRectTransform, Camera ParamCamera = null)
        {
            Vector2 result;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(ParamRectTransform, Input.mousePosition, ParamCamera, out result);

            return result;
        }

        public static Vector2 GetMousePositionInMe(this RectTransform ParamMe, Camera ParamCamera = null)
        {
            return WispRectTransform.GetMousePositionInRectTransform(ParamMe, ParamCamera);
        }

        public static Vector2 GetMousePosRelativeToMe(this RectTransform ParamMe)
        {
            return Input.mousePosition - ParamMe.position;
        }

        public static Vector2 GetMousePosInMeHack(this RectTransform ParamMe)
        {
            GameObject go = new GameObject("MyGO", typeof(RectTransform));

            RectTransform canvasRT = WispVisualComponent.GetMainCanvas().GetComponent<RectTransform>();
            RectTransform goRT = go.GetComponent<RectTransform>();

            goRT.SetParent(canvasRT);
            goRT.AnchorTo("center-center");
            goRT.PivotAround("center-center");
            goRT.anchoredPosition = Vector2.zero;

            goRT.SetParent(ParamMe);
            Vector2 result = new Vector2(goRT.anchoredPosition.x, goRT.anchoredPosition.y);

            MonoBehaviour.Destroy(go);
            return result;
        }

        public static WispRectTransformAnchorSettings GetAnchorSettings(this RectTransform ParamMe)
        {
            return new WispRectTransformAnchorSettings(ParamMe);
        }

        public static void SetAnchorSettings(this RectTransform ParamMe, WispRectTransformAnchorSettings ParamSettings)
        {
            ParamMe.anchorMin = ParamSettings.anchorMin;
            ParamMe.anchorMax = ParamSettings.anchorMax;
            ParamMe.pivot = ParamSettings.pivot;
        }

        public static WispRectTransformGeometricSettings GetGeometricSettings(this RectTransform ParamMe)
        {
            return new WispRectTransformGeometricSettings(ParamMe);
        }

        public static void SetGeometricSettings(this RectTransform ParamMe, WispRectTransformGeometricSettings ParamSettings)
        {
            ParamMe.anchoredPosition3D = ParamSettings.anchoredPosition3D;
            ParamMe.sizeDelta = ParamSettings.sizeDelta;
            ParamMe.rotation = ParamSettings.rotation;
            ParamMe.localScale = ParamSettings.localScale;
        }

        /// <summary>
        /// <para>Set anchor position.</para>
        /// <para>Example : AnchorTo("left-top"); to anchor to the Top Left of the parent RectTransform.</para>
        /// <para>Another Example : AnchorTo("center-center"); to anchor to the Center of the parent RectTransform.</para>
        /// <para>X options : right,center and left.</para>
        /// <para>Y options : top,center and Bottom.</para>
        /// </summary>
        public static void AnchorTo(this RectTransform ParamMe, string ParamAnchor)
        {
            string[] anchors = ParamAnchor.Split('-');

            if (anchors.Length != 2)
            {
                WispVisualComponent.LogError("Invalid anchoring parameters.");
                return;
            }

            float x = 0.5f;
            float y = 0.5f;

            if (anchors[0] == "right")
                x = 1f;
            else if (anchors[0] == "center")
                x = 0.5f;
            else if (anchors[0] == "left")
                x = 0f;
            else if (anchors[0] == "top")
                y = 1f;
            else if (anchors[0] == "center")
                y = 0.5f;
            else if (anchors[0] == "bottom")
                y = 0f;
            else
            {
                WispVisualComponent.LogError("Invalid anchoring parameters.");
                return;
            }

            if (anchors[1] == "top")
                y = 1f;
            else if (anchors[1] == "center")
                y = 0.5f;
            else if (anchors[1] == "bottom")
                y = 0f;
            else if (anchors[1] == "right")
                x = 1f;
            else if (anchors[1] == "center")
                x = 0.5f;
            else if (anchors[1] == "left")
                x = 0f;
            else
            {
                WispVisualComponent.LogError("Invalid anchoring parameters.");
                return;
            }

            ParamMe.anchorMin = new Vector2(x, y);
            ParamMe.anchorMax = new Vector2(x, y);
        }

        /// <summary>
        /// <para>Set pivot position.</para>
        /// <para>Example : AnchorTo("left-top"); to anchor to the Top Left of the parent RectTransform.</para>
        /// <para>Another Example : AnchorTo("center-center"); to anchor to the Center of the parent RectTransform.</para>
        /// <para>X options : right,center and left.</para>
        /// <para>Y options : top,center and Bottom.</para>
        /// </summary>
        public static void PivotAround(this RectTransform ParamMe, string ParamPivot)
        {
            string[] pivot = ParamPivot.Split('-');

            if (pivot.Length != 2)
            {
                WispVisualComponent.LogError("Invalid pivoting parameters.");
                return;
            }

            float x = 0.5f;
            float y = 0.5f;

            if (pivot[0] == "right")
                x = 1f;
            else if (pivot[0] == "center")
                x = 0.5f;
            else if (pivot[0] == "left")
                x = 0f;
            else if (pivot[0] == "top")
                y = 1f;
            else if (pivot[0] == "center")
                y = 0.5f;
            else if (pivot[0] == "bottom")
                y = 0f;
            else
            {
                WispVisualComponent.LogError("Invalid pivoting parameters.");
                return;
            }

            if (pivot[1] == "top")
                y = 1f;
            else if (pivot[1] == "center")
                y = 0.5f;
            else if (pivot[1] == "bottom")
                y = 0f;
            else if (pivot[1] == "right")
                x = 1f;
            else if (pivot[1] == "center")
                x = 0.5f;
            else if (pivot[1] == "left")
                x = 0f;
            else
            {
                WispVisualComponent.LogError("Invalid pivoting parameters.");
                return;
            }

            ParamMe.pivot = new Vector2(x, y);
        }

        /// <summary>
        /// ...
        /// </summary>
        public static void AnchorStyleExpanded(this RectTransform ParamMe, bool ParamMaximize = false)
        {
            ParamMe.anchorMin = new Vector2(0f, 0f);
            ParamMe.anchorMax = new Vector2(1f, 1f);
            ParamMe.pivot = new Vector2(0.5f, 0.5f);

            if (ParamMaximize)
            {
                ParamMe.SetRight(0);
                ParamMe.SetLeft(0);
                ParamMe.SetTop(0);
                ParamMe.SetBottom(0);
            }
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void SetLeft(this RectTransform ParamMe, float ParamLeft)
        {
            ParamMe.offsetMin = new Vector2(ParamLeft, ParamMe.offsetMin.y);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void SetRight(this RectTransform ParamMe, float ParamRight)
        {
            ParamMe.offsetMax = new Vector2(-ParamRight, ParamMe.offsetMax.y);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void SetTop(this RectTransform ParamMe, float ParamTop)
        {
            ParamMe.offsetMax = new Vector2(ParamMe.offsetMax.x, -ParamTop);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void SetBottom(this RectTransform ParamMe, float ParamBottom)
        {
            ParamMe.offsetMin = new Vector2(ParamMe.offsetMin.x, ParamBottom);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void TuneLeft(this RectTransform ParamMe, float ParamAmount)
        {
            ParamMe.offsetMin = new Vector2(ParamMe.offsetMin.x + ParamAmount, ParamMe.offsetMin.y);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void TuneRight(this RectTransform ParamMe, float ParamAmount)
        {
            ParamMe.offsetMax = new Vector2(ParamMe.offsetMax.x + ParamAmount, ParamMe.offsetMax.y);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void TuneTop(this RectTransform ParamMe, float ParamAmount)
        {
            ParamMe.offsetMax = new Vector2(ParamMe.offsetMax.x, ParamMe.offsetMax.y + ParamAmount);
        }

        /// <summary>
        /// Define a fucntion to call when the button is pressed.
        /// </summary>
        public static void TuneBottom(this RectTransform ParamMe, float ParamAmount)
        {
            ParamMe.offsetMin = new Vector2(ParamMe.offsetMin.x, ParamMe.offsetMin.y + ParamAmount);
        }

        public static float GetRight(this RectTransform ParamMe)
        {
            return ParamMe.offsetMax.x;
        }

        public static float GetBottom(this RectTransform ParamMe)
        {
            return ParamMe.offsetMin.y;
        }

        public static float GetLeft(this RectTransform ParamMe)
        {
            return ParamMe.offsetMin.x;
        }

        public static float GetTop(this RectTransform ParamMe)
        {
            return ParamMe.offsetMax.y;
        }

        [Obsolete("This method is obsolet due to being in an experimental phase.")]
        public static Vector2 GetCentredPivotLocalPosition(this RectTransform ParamMe)
        {
            float widthOffset = 0f;
            float heightOffset = 0f;

            if (ParamMe.pivot.x < 0.5f)
            {
                if (ParamMe.localPosition.x > 0)
                    widthOffset = (ParamMe.pivot.x - 0.5f) * ParamMe.rect.width * -1;
                else
                    widthOffset = (ParamMe.pivot.x - 0.5f) * ParamMe.rect.width;

                if (ParamMe.pivot.x < 0)
                    widthOffset *= -1;
            }
            else if (ParamMe.pivot.x > 0.5f)
            {
                widthOffset = (ParamMe.pivot.x - 0.5f) * ParamMe.rect.width * -1;
            }

            // ----------------------------------------------------------------------------------

            if (ParamMe.pivot.y < 0.5f)
            {
                if (ParamMe.localPosition.y > 0)
                    heightOffset = (ParamMe.pivot.y - 0.5f) * ParamMe.rect.height;
                else
                    heightOffset = (ParamMe.pivot.y - 0.5f) * ParamMe.rect.height * -1;
            }
            else if (ParamMe.pivot.y > 0.5f)
            {
                heightOffset = (ParamMe.pivot.y - 0.5f) * ParamMe.rect.height * -1;
            }

            return ParamMe.localPosition + new Vector3(widthOffset, heightOffset, 0);
        }

        [Obsolete("This method is obsolet due to being in an experimental phase.")]
        public static Vector2[] GetCentredPivot_Corners_LocalPosition(this RectTransform ParamMe)
        {
            Vector2[] result = new Vector2[4];

            Vector2 center = ParamMe.GetCentredPivotLocalPosition();

            result[0] = center + new Vector2(-ParamMe.rect.width / 2, -ParamMe.rect.height / 2);
            result[1] = center + new Vector2(ParamMe.rect.width / 2, -ParamMe.rect.height / 2);
            result[2] = center + new Vector2(ParamMe.rect.width / 2, ParamMe.rect.height / 2);
            result[3] = center + new Vector2(-ParamMe.rect.width / 2, ParamMe.rect.height / 2);

            return result;
        }

        public static void SetSizeDeltaWidth(this RectTransform ParamMe, float ParamWidth)
        {
            ParamMe.sizeDelta = new Vector2(ParamWidth, ParamMe.rect.height);
        }

        public static void SetSizeDeltaHeight(this RectTransform ParamMe, float ParamHeight)
        {
            ParamMe.sizeDelta = new Vector2(ParamMe.sizeDelta.x, ParamHeight);
        }

        // From : https://answers.unity.com/questions/1100493/convert-recttransformrect-to-rect-world.html
        public static Rect GetWorldRect(RectTransform rt, Vector2 scale)
        {
            // Convert the rectangle to world corners and grab the top left
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 topLeft = corners[0];

            // Rescale the size appropriately based on the current Canvas scale
            Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

            return new Rect(topLeft, scaledSize);
        }

        // 0 is bottom-left
        // 1 is bottom-right
        // 2 is top-right
        // 3 is top-left
        public static Vector2[] GetRectCorners(Rect ParamRect)
        {
            Vector2[] corners = new Vector2[4];

            corners[0] = new Vector2(ParamRect.x, ParamRect.y);
            corners[1] = new Vector2(ParamRect.x + ParamRect.width, ParamRect.y);
            corners[2] = new Vector2(ParamRect.x + ParamRect.width, ParamRect.y + ParamRect.height);
            corners[3] = new Vector2(ParamRect.x, ParamRect.y + ParamRect.height);

            return corners;
        }

        public static void ShiftPosition(this RectTransform ParamMe, float ParamX, float ParamY)
        {
            ParamMe.anchoredPosition = new Vector2(ParamMe.anchoredPosition.x + ParamX, ParamMe.anchoredPosition.y + ParamY);
        }
    }

    public struct WispRectTransformAnchorSettings
    {
        public WispRectTransformAnchorSettings(Vector2 ParamMin, Vector2 ParamMax, Vector2 ParamPivot)
        {
            anchorMin = ParamMin;
            anchorMax = ParamMax;
            pivot = ParamPivot;
        }

        public WispRectTransformAnchorSettings(RectTransform ParamSource)
        {
            anchorMin = ParamSource.anchorMin;
            anchorMax = ParamSource.anchorMax;
            pivot = ParamSource.pivot;
        }

        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 pivot;
    }

    public struct WispRectTransformGeometricSettings
    {
        public WispRectTransformGeometricSettings(Vector3 ParamAnchoredPosition, Vector2 ParamSizeDelta, Quaternion ParamRotation, Vector3 ParamLocalScale)
        {
            anchoredPosition3D = ParamAnchoredPosition;
            sizeDelta = ParamSizeDelta;
            rotation = ParamRotation;
            localScale = ParamLocalScale;
        }

        public WispRectTransformGeometricSettings(RectTransform ParamSource)
        {
            anchoredPosition3D = ParamSource.anchoredPosition3D;
            sizeDelta = ParamSource.sizeDelta;
            rotation = ParamSource.rotation;
            localScale = ParamSource.localScale;
        }

        public Vector3 anchoredPosition3D;
        public Vector2 sizeDelta;
        public Quaternion rotation;
        public Vector3 localScale;
    }    
}