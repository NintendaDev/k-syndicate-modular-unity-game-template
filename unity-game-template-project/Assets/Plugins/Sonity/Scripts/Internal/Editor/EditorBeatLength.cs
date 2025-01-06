// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

	public static class EditorBeatLength {

		public static float BpmToSecondsPerBar(float bpm) {
			return 60f / bpm * 4f;
		}

		public static float BeatToDivision(BeatLength beatLength) {
			if (beatLength == BeatLength.FourBars) {
				return 4f;
			} else if (beatLength == BeatLength.TwoBars) {
				return 2f;
			} else if (beatLength == BeatLength.OneBar) {
				return 1f;
			} else if (beatLength == BeatLength.Half) {
				return 0.5f;
			} else if (beatLength == BeatLength.Quarter) {
				return 0.25f;
			} else if (beatLength == BeatLength.QuarterTriplet) {
				return 0.25f * 2f / 3f;
			} else if (beatLength == BeatLength.Eighth) {
				return 0.125f;
			} else if (beatLength == BeatLength.EighthTriplet) {
				return 0.125f * 2f / 3f;
			} else if (beatLength == BeatLength.Sixteenth) {
				return 0.0625f;
			} else if (beatLength == BeatLength.SixteenthTriplet) {
				return 0.0625f * 2f / 3f;
			} else if (beatLength == BeatLength.ThirtySecond) {
				return 0.03125f;
			} else if (beatLength == BeatLength.ThirtySecondTriplet) {
				return 0.03125f * 2f / 3f;
			}
			return 1f;
		}
	}
}
#endif