// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundManagerInternalsStatistics {

        public bool statisticsExpandBase = true;
        public bool statisticsExpandInstances = true;
        public SoundManagerStatisticsSorting statisticsSorting = SoundManagerStatisticsSorting.Voices;

        public bool statisticsShowVoices = true;
        public bool statisticsShowVolume = true;
        public bool statisticsShowActive = false;
        public bool statisticsShowDisabled = false;
        public bool statisticsShowPlays = false;

        [NonSerialized]
        public int statisticsVoicesPlayed;
        [NonSerialized]
        public int statisticsMaxSimultaneousVoices;
    }
}
#endif