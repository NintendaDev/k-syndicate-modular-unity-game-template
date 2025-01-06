// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public static class EditorColor {

        /// <summary>
        /// Sets the alpha value of a color
        /// </summary>
        public static Color ChangeAlpha(Color color, float alpha) {
            color.a = alpha;
            return color;
        }

        /// <summary>
        /// Changes the hue with the selected bipolar offset
        /// </summary>
        public static Color ChangeHue(Color color, float hueOffset) {
            float colorA = color.a;
            Color.RGBToHSV(color, out float colorH, out float colorS, out float colorV);
            colorH += hueOffset;
            while (colorH < 0f) {
                colorH += 1f;
            }
            while (colorH > 1f) {
                colorH -= 1f;
            }
            Color outputColor = Color.HSVToRGB(colorH, colorS, colorV);
            outputColor.a = colorA;
            return outputColor;
        }

        /// <summary>
        /// Changes the lightness with the selected bipolar offset
        /// </summary>
        public static Color ChangeValue(Color color, float lightnessOffset) {
            float colorA = color.a;
            Color.RGBToHSV(color, out float colorH, out float colorS, out float colorV);
            colorV += lightnessOffset;
            Color outputColor = Color.HSVToRGB(colorH, colorS, colorV);
            outputColor.a = colorA;
            return outputColor;
        }

        /// <summary>
        /// Changes the saturation with the selected bipolar offset
        /// </summary>
        public static Color ChangeSaturation(Color color, float saturationOffset) {
            float colorA = color.a;
            Color.RGBToHSV(color, out float colorH, out float colorS, out float colorV);
            colorS += saturationOffset;
            Color outputColor = Color.HSVToRGB(colorH, colorS, colorV);
            outputColor.a = colorA;
            return outputColor;
        }

        // Grey
        private static Color timelinePlaybackPosition = new Color(0.2f, 0.2f, 0.2f);
        public static Color GetTimelinePlaybackPosition(float alpha) {
            Color outputColor = timelinePlaybackPosition;
            outputColor.a = alpha;
            return outputColor;
        }

        // Purple
        private static Color soundTagColor = new Color(0.619f, 0f, 1f);
        public static Color GetSoundTag(float alpha) {
            Color outputColor = soundTagColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Lighter Hard Orange Red
        private static Color eventColor = new Color(1f, 0.8f, 0f);
        public static Color GetSoundEvent(float alpha) {
            Color outputColor = eventColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Lime green
        private static Color debugColor = new Color(0.375f, 1f, 0f);
        public static Color GetDebug(float alpha) {
            Color outputColor = debugColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Orange
        private static Color editorToolsColor = new Color(1f, 0.5f, 0f);
        public static Color GetEditorTools(float alpha) {
            Color outputColor = editorToolsColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Light Blue
        private static Color soundManagerStatisticsColor = new Color(0f, 0.964f, 1f);
        public static Color GetSoundManagerStatistics(float alpha) {
            Color outputColor = soundManagerStatisticsColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Light Grey
        private static Color disabledColor = new Color(0.9f, 0.9f, 0.9f);
        public static Color GetDisabled(float alpha) {
            Color outputColor = disabledColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Red
        private static Color settingsColor = new Color(1f, 0.117f, 0.478f);
        public static Color GetSettings(float alpha) {
            Color outputColor = settingsColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Green
        private static Color distanceColor = new Color(0.517f, 0.854f, 0.270f);
        public static Color GetDistance(float alpha) {
            Color outputColor = distanceColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Orange Red
        private static Color intensityColor = new Color(0.894f, 0.729f, 0.231f);
        public static Color GetIntensityMin(float alpha) {
            Color outputColor = ChangeHue(intensityColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetIntensityMax(float alpha) {
            Color outputColor = intensityColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Green
        private static Color pitchColor = new Color(0.517f, 0.854f, 0.270f);
        public static Color GetPitchMax(float alpha) {
            Color outputColor = pitchColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetPitchMin(float alpha) {
            Color outputColor = ChangeHue(pitchColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Blue
        private static Color delayColor = new Color(0f, 0.117f, 1f);
        public static Color GetDelay(float alpha) {
            Color outputColor = delayColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Grenish Blue
        private static Color fadeInColor = new Color(0f, 1f, 0.25f);
        public static Color GetFadeIn(float alpha) {
            Color outputColor = fadeInColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Mint Blue
        private static Color fadeOutColor = new Color(0f, 1f, 0.75f);
        public static Color GetFadeOut(float alpha) {
            Color outputColor = fadeOutColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Orange Yellow
        private static Color volumeColor = new Color(1f, 0.705f, 0f);
        public static Color GetVolumeMax(float alpha) {
            Color outputColor = volumeColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetVolumeMin(float alpha) {
            Color outputColor = ChangeHue(volumeColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Turquoise
        private static Color stereoPanColor = new Color(0f, 1f, 1f);
        public static Color GetStereoPanMax(float alpha) {
            Color outputColor = stereoPanColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetStereoPanMin(float alpha) {
            Color outputColor = ChangeHue(stereoPanColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Pink
        private static Color reverbZoneMixColor = new Color(1f, 0f, 0.75f);
        public static Color GetReverbZoneMixColorMax(float alpha) {
            Color outputColor = reverbZoneMixColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetReverbZoneMixColorMin(float alpha) {
            Color outputColor = ChangeHue(reverbZoneMixColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Light Blue
        private static Color spatialBlendColor = new Color(0f, 0.964f, 1f);
        public static Color GetSpatialBlendMax(float alpha) {
            Color outputColor = spatialBlendColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetSpatialBlendMin(float alpha) {
            Color outputColor = ChangeHue(spatialBlendColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Turquoise
        private static Color spatialSpreadColor = new Color(0f, 1f, 0.658f);
        public static Color GetSpatialSpreadMax(float alpha) {
            Color outputColor = spatialSpreadColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetSpatialSpreadMin(float alpha) {
            Color outputColor = ChangeHue(spatialSpreadColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Red
        private static Color distortionColor = new Color(1f, 0.149f, 0f);
        public static Color GetDistortionMax(float alpha) {
            Color outputColor = distortionColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetDistortionMin(float alpha) {
            Color outputColor = ChangeHue(distortionColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Blue
        private static Color lowpassFrequencyColor = new Color(0f, 0.117f, 1f);
        public static Color GetLowpassFrequencyMax(float alpha) {
            Color outputColor = lowpassFrequencyColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetLowpassFrequencyMin(float alpha) {
            Color outputColor = ChangeHue(lowpassFrequencyColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Blue
        private static Color lowpassAmountColor = new Color(0f, 0.117f, 1f);
        public static Color GetLowpassAmountMax(float alpha) {
            Color outputColor = lowpassAmountColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetLowpassAmountMin(float alpha) {
            Color outputColor = ChangeHue(lowpassAmountColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Pink
        private static Color highpassFrequencyColor = new Color(1f, 0.117f, 0.478f);
        public static Color GetHighpassFrequencyMax(float alpha) {
            Color outputColor = highpassFrequencyColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetHighpassFrequencyMin(float alpha) {
            Color outputColor = ChangeHue(highpassFrequencyColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // Pink
        private static Color highpassAmountColor = new Color(1f, 0.117f, 0.478f);
        public static Color GetHighpassAmountMax(float alpha) {
            Color outputColor = highpassAmountColor;
            outputColor.a = alpha;
            return outputColor;
        }

        public static Color GetHighpassAmountMin(float alpha) {
            Color outputColor = ChangeHue(highpassAmountColor, -0.16f);
            outputColor.a = alpha;
            return outputColor;
        }

        // White
        private static Color physicsConditionsColor = new Color(1f, 1f, 1f);
        public static Color GetPhysicsConditionsColor(float alpha) {
            Color outputColor = physicsConditionsColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Yellow
        private static Color physicsColor = new Color(1f, 0.67f, 0.08f);
        public static Color GetPhysics(float alpha) {
            Color outputColor = physicsColor;
            outputColor.a = alpha;
            return outputColor;
        }

        // Purple
        private static Color presetColor = new Color(0.7f, 0.3f, 1f);
        public static Color GetPreset(float alpha) {
            Color outputColor = presetColor;
            outputColor.a = alpha;
            return outputColor;
        }

    }
}
#endif