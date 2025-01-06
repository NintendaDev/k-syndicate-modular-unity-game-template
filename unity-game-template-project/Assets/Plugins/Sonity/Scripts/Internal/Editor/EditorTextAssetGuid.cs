// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorTextAssetGuid {

        public static string assetGuidLabel = $"Asset GUID";

        public static string assetGuidTooltip =
        $"The ID of an asset is cached in the editor because in some cases when using adressables a single scriptable object might be instantiated multiple times." + "\n" +
        "\n" +
        $"This might lead to problems where a simple \"Object == Object\" check will return false, but a \"ID == ID\" check will return true." + "\n" +
        "\n" +
        $"If you are upgrading Sonity and use adressable assets you probably want to select all Sonity scriptable objects and run:" + "\n" +
        "\n" +
        $"\"Tools/Sonity 🔊/Set Selected Assets as Dirty (Force Reserialize for Resave)\" which will force the assets to cache the GUIDs." + "\n" +
        "\n" +
        $"You can search t:SoundEvent, t:SoundContainer, t:SoundTag, t:SoundPolyGroup, t:SoundDataGroup, t:SoundMix, t:SoundPhysicsCondition to find all objects." + "\n" +
        "\n" +
        $"If no GUID is cached then it will use the InstanceID in builds." + "\n" +
        "\n" +
        $"The free trial version doesn't have full functionality." + EditorTrial.trialTooltip;
    }
}
#endif