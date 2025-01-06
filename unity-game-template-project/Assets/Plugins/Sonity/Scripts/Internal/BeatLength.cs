// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    // Cant write forward slashes because then it just makes sub hierarchies
    // Doesnt work in Unity 2019.1.0
#if UNITY_2019_2_OR_NEWER
    public enum BeatLength {
		[InspectorName("4 Bars")] FourBars = 0,
		[InspectorName("2 Bars")] TwoBars = 1,
		[InspectorName("1 Bar")] OneBar = 2,
		[InspectorName("1\\2")] Half = 3,
		[InspectorName("1\\4")] Quarter = 4,
		[InspectorName("1\\4T")] QuarterTriplet = 5,
		[InspectorName("1\\8")] Eighth = 6,
		[InspectorName("1\\8T")] EighthTriplet = 7,
		[InspectorName("1\\16")] Sixteenth = 8,
		[InspectorName("1\\16T")] SixteenthTriplet = 9,
		[InspectorName("1\\32")] ThirtySecond = 10,
		[InspectorName("1\\32T")] ThirtySecondTriplet = 11,
	}
#else
	// Code for older because InspectorName doesnt exist
	public enum BeatLength {
		FourBars = 0,
		TwoBars = 1,
		OneBar = 2,
		Half = 3,
		Quarter = 4,
		QuarterTriplet = 5,
		Eighth = 6,
		EighthTriplet = 7,
		Sixteenth = 8,
		SixteenthTriplet = 9,
		ThirtySecond = 10,
		ThirtySecondTriplet = 11,
	}
#endif
}