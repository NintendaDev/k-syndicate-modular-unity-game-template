// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundEventInstanceDictionaryValue {
        // Is used to check if the SoundEvent has been unloaded by eg adressables
        public SoundEventBase soundEvent;
        // The int is the transform instance ID
        public Dictionary<int, SoundEventInstance> instanceDictionary = new Dictionary<int, SoundEventInstance>();
        public Stack<SoundEventInstance> unusedInstanceStack = new Stack<SoundEventInstance>();
    }

    public class CheckedDictionaryValueSoundEvent {

        public bool nullChecked = false;
        public bool isNull = false;

        public bool savedMaxChecked = false;
        public float savedMaxLength = 0f;

        public bool savedMinLengthChecked = false;
        // Is MinLengthWithoutPitchOnlyFirstSoundContainer
        public float savedMinLength = 0f;

        public bool triggerOnPlayIsInfiniteLoop = false;
        public bool triggerOnPlayIsInfiniteLoopChecked = false;
        public bool triggerOnStopIsInfiniteLoop = false;
        public bool triggerOnStopIsInfiniteLoopChecked = false;
        // TriggerOnTail should be allowed to loop for e.g. music

        public int statisticsNumberOfPlays = 0;
    }

    public class CheckedDictionaryValueSoundDataGroup {
        public bool isInfiniteLoopChecked = false;
        public bool isInfiniteLoop = false;
    }

    public class CheckedDictionaryValueSoundMix {
        public bool isInfiniteLoopChecked = false;
        public bool isInfiniteLoop = false;
    }

    public class CheckedDictionaryValueSoundPhysicsCondition {
        public bool isInfiniteLoopChecked = false;
        public bool isInfiniteLoop = false;
    }

    [Serializable]
    public class SoundManagerInternals {

        public SoundManagerInternalsSettings settings = new SoundManagerInternalsSettings();
#if UNITY_EDITOR
        public SoundManagerInternalsEditorTools editorTools = new SoundManagerInternalsEditorTools();
        public SoundManagerInternalsDebug debug = new SoundManagerInternalsDebug();
        public SoundManagerInternalsStatistics statistics = new SoundManagerInternalsStatistics();
#endif
        [NonSerialized]
        public bool onApplicationIsPaused = false;
        [NonSerialized]
        public bool isGoingToDelete = false;
        [NonSerialized]
        public bool applicationIsQuitting = false;

        [NonSerialized]
        public Dictionary<long, SoundEventInstanceDictionaryValue> soundEventDictionary = new Dictionary<long, SoundEventInstanceDictionaryValue>();
        [NonSerialized]
        public SoundManagerVoicePool voicePool = new SoundManagerVoicePool();
        [NonSerialized]
        public SoundManagerVoiceEffectPool voiceEffectPool = new SoundManagerVoiceEffectPool();

        [NonSerialized]
        public Dictionary<long, CheckedDictionaryValueSoundEvent> checkedDictionarySoundEvent = new Dictionary<long, CheckedDictionaryValueSoundEvent>();
        [NonSerialized]
        public Dictionary<long, CheckedDictionaryValueSoundDataGroup> checkedDictionarySoundDataGroup = new Dictionary<long, CheckedDictionaryValueSoundDataGroup>();
        [NonSerialized]
        public Dictionary<long, CheckedDictionaryValueSoundMix> checkedDictionarySoundMix = new Dictionary<long, CheckedDictionaryValueSoundMix>();
        [NonSerialized]
        public Dictionary<long, CheckedDictionaryValueSoundPhysicsCondition> checkedDictionarySoundPhysicsCondition = new Dictionary<long, CheckedDictionaryValueSoundPhysicsCondition>();

        // Chached Objects
        [NonSerialized]
        public Transform cachedSoundManagerTransform;
        [NonSerialized]
        public GameObject cachedSoundEventPoolGameObject;
        [NonSerialized]
        public Transform cachedSoundEventPoolTransform;
        [NonSerialized]
        public GameObject cachedVoicePoolGameObject;
        [NonSerialized]
        public Transform cachedVoicePoolTransform;
        [NonSerialized]
        public GameObject cachedMusicGameObject;
        [NonSerialized]
        public Transform cachedMusicTransform;
        [NonSerialized]
        public GameObject cached2DGameObject;
        [NonSerialized]
        public Transform cached2DTransform;
        [NonSerialized]
        public AudioListener cachedAudioListener;
        [NonSerialized]
        public Transform cachedAudioListenerTransform;
        [NonSerialized]
        public AudioListenerDistanceBase cachedAudioListenerDistance;
        [NonSerialized]
        public Transform cachedAudioListenerDistanceTransform;

        public void AwakeCheck() {
            FindAudioListener(true);
            FindAudioListenerDistance(true);
            if (settings.dontDestroyOnLoad) {
                // DontDestroyOnLoad only works for root GameObjects
                SoundManagerBase.Instance.gameObject.transform.parent = null;
                UnityEngine.Object.DontDestroyOnLoad(SoundManagerBase.Instance.gameObject);
            }
            if (cachedSoundEventPoolGameObject == null) {
                cachedSoundEventPoolGameObject = new GameObject($"SonitySoundEventPool");
                cachedSoundEventPoolTransform = cachedSoundEventPoolGameObject.transform;
                cachedSoundEventPoolTransform.parent = cachedSoundManagerTransform;
            }
            if (cachedVoicePoolGameObject == null) {
                cachedVoicePoolGameObject = new GameObject("SonityVoicePool");
                cachedVoicePoolTransform = cachedVoicePoolGameObject.transform;
                cachedVoicePoolTransform.parent = cachedSoundManagerTransform;
                // Preload Voices on Awake
                voicePool.CreateVoice(settings.voicePreload, true);
            }
            if (cachedMusicGameObject == null) {
                cachedMusicGameObject = new GameObject("SonityMusic");
                cachedMusicTransform = cachedMusicGameObject.transform;
                cachedMusicTransform.parent = cachedSoundManagerTransform;
            }
            if (cached2DGameObject == null) {
                cached2DGameObject = new GameObject("Sonity2D");
                cached2DTransform = cached2DGameObject.transform;
                cached2DTransform.parent = cachedSoundManagerTransform;
            }
            cachedSoundManagerTransform.position = Vector3.zero;
            cachedSoundEventPoolTransform.position = Vector3.zero;
            cachedVoicePoolTransform.position = Vector3.zero;
            cachedMusicTransform.position = Vector3.zero;
            cached2DTransform.position = Vector3.zero;

#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
            // Adressables preload AudioMixer
            if (settings.adressableAudioMixerUse) {
                settings.LoadAdressableAudioMixer();
            }
#endif
        }

        public void Destroy() {
            isGoingToDelete = true;
            InternalOnDestroyForceStopEverything();
            if (cachedSoundEventPoolGameObject != null) {
                UnityEngine.Object.Destroy(cachedSoundEventPoolGameObject);
            }
            if (cachedMusicGameObject != null) {
                UnityEngine.Object.Destroy(cachedMusicGameObject);
            }
            if (cached2DGameObject != null) {
                UnityEngine.Object.Destroy(cached2DGameObject);
            }
        }

        public void FindAudioListener(bool isAwake = false) {
            if (cachedAudioListener == null || !cachedAudioListener.isActiveAndEnabled) {
#if UNITY_2022_3_OR_NEWER && !SONITY_DLL_RUNTIME
                // FindObjectsSortMode.InstanceID is slower but is more consistent
                AudioListener[] audioListenerList = UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.InstanceID);
#else
                AudioListener[] audioListenerList = UnityEngine.Object.FindObjectsOfType<AudioListener>();
#endif
                foreach (AudioListener audioListener in audioListenerList) {
                    if (audioListener != null && audioListener.isActiveAndEnabled) {
                        // Finds the first enabled AudioListener
                        cachedAudioListener = audioListener;
                        break;
                    }
                }
                if (cachedAudioListener == null || !cachedAudioListener.isActiveAndEnabled) {
                    // On awake with domain reload disabled, the AudioListener is not activeAndEnabled
                    if (!isAwake) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning(
                                $"Sonity.{nameof(NameOf.SoundManager)} could find no active {nameof(AudioListener)} in the scene." +
                                $"If you are creating one in runtime, make sure it is created in Awake()."
                                );
                        }
                    }
                } else {
                    cachedAudioListenerTransform = cachedAudioListener.transform;
                    AudioListener.pause = settings.globalPause;
#if SONITY_ENABLE_VOLUME_INCREASE
                    AudioListener.volume = settings.globalVolumeRatio * VolumeScale.volumeIncrease60dbAudioListenerRatioMax;
#else
                    AudioListener.volume = settings.globalVolumeRatio;
#endif
                }
            }
        }

        private void FindAudioListenerDistance(bool isAwake = false) {
            if (settings.overrideListenerDistance) {
                if (cachedAudioListenerDistance == null || !cachedAudioListenerDistance.isActiveAndEnabled) {
#if UNITY_2022_3_OR_NEWER && !SONITY_DLL_RUNTIME
                    // FindObjectsSortMode.InstanceID is slower but is more consistent
                    AudioListenerDistanceBase[] audioListenerDistanceList = UnityEngine.Object.FindObjectsByType<AudioListenerDistanceBase>(FindObjectsSortMode.InstanceID);
#else
                    AudioListenerDistanceBase[] audioListenerDistanceList = UnityEngine.Object.FindObjectsOfType<AudioListenerDistanceBase>();
#endif
                    foreach (AudioListenerDistanceBase audioListenerDistance in audioListenerDistanceList) {
                        if (audioListenerDistance != null && audioListenerDistance.isActiveAndEnabled) {
                            // Finds the first enabled AudioListenerDistance
                            cachedAudioListenerDistance = audioListenerDistance;
                            break;
                        }
                    }
                    if (cachedAudioListenerDistance == null || !cachedAudioListenerDistance.isActiveAndEnabled) {
                        // On awake with domain reload disabled, the AudioListenerDistance is not activeAndEnabled
                        if (!isAwake) {
                            if (ShouldDebug.Warnings()) {
                                Debug.LogWarning(
                                    $"Sonity.{nameof(NameOf.SoundManager)} could find no active {nameof(NameOf.AudioListenerDistance)} in the scene." +
                                    $"If you are creating one in runtime, make sure it is created in Awake()."
                                    );
                            }
                        }
                    } else {
                        cachedAudioListenerDistanceTransform = cachedAudioListenerDistance.transform;
                    }
                }
            }
        }

        public float GetDistanceToAudioListener(Vector3 position) {
            if (settings.overrideListenerDistance && settings.overrideListenerDistanceAmount > 0f) {
                if (settings.overrideListenerDistanceAmount < 100f) {
                    if (cachedAudioListener == null || !cachedAudioListener.isActiveAndEnabled) {
                        FindAudioListener();
                        if (cachedAudioListener == null) {
                            return 0f;
                        }
                    }
                    if (cachedAudioListenerDistance == null) {
                        FindAudioListenerDistance();
                        if (cachedAudioListenerDistance == null) {
                            return 0f;
                        }
                    }
                    // Position between the AudioListener and Audio Listener Distance
                    return Vector3.Distance(position, Vector3.Lerp(cachedAudioListenerTransform.position, cachedAudioListenerDistanceTransform.position, settings.overrideListenerDistanceAmount * 0.01f));
                } else {
                    if (cachedAudioListenerDistance == null) {
                        FindAudioListenerDistance();
                        if (cachedAudioListenerDistance == null) {
                            return 0f;
                        }
                    }
                    // Position at the AudioListenerDistance
                    return Vector3.Distance(position, cachedAudioListenerDistanceTransform.position);
                }
            } else {
                if (cachedAudioListener == null || !cachedAudioListener.isActiveAndEnabled) {
                    FindAudioListener();
                    if (cachedAudioListener == null) {
                        return 0f;
                    }
                }
                // Position at the AudioListener
                return Vector3.Distance(position, cachedAudioListenerTransform.position);
            }
        }

        public float GetAngleToAudioListener(Vector3 position) {
            if (cachedAudioListener == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning(
                        $"Sonity.{nameof(NameOf.SoundManager)} could find no {nameof(AudioListener)} in the scene. " +
                        $"If you are creating one in runtime, make sure it is created in Awake()."
                        );
                }
            } else {
                return AngleAroundAxis.Get(position - cachedAudioListenerTransform.position, cachedAudioListenerTransform.forward, Vector3.up);
            }
            return 0f;
        }

        // Managed update has its own temp variables so nothing else can change them while its running
        [NonSerialized]
        private SoundEventInstance managedUpdateTempInstance;
        [NonSerialized]
        private Dictionary<long, SoundEventInstanceDictionaryValue>.Enumerator managedUpdateTempInstanceDictionaryValueEnumerator;
        [NonSerialized]
        private Dictionary<int, SoundEventInstance>.Enumerator managedUpdateTempInstanceEnumerator;

        public void ManagedUpdate() {

            // Update time value based on the selected time scale
            SoundTimeScale.UpdateTime(settings.soundTimeScale);

            // Managed Update of the SoundEvent Instances
            managedUpdateTempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (managedUpdateTempInstanceDictionaryValueEnumerator.MoveNext()) {

                // To Delete SoundEvent Instances (unloaded by eg adressables)
                if (managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.soundEvent == null) {
                    toDeleteAdded = true;
                    toDeleteInstances.Add(managedUpdateTempInstanceDictionaryValueEnumerator);
                }

                managedUpdateTempInstanceEnumerator = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                while (managedUpdateTempInstanceEnumerator.MoveNext()) {
                    managedUpdateTempInstance = managedUpdateTempInstanceEnumerator.Current.Value;
                    managedUpdateTempInstance.ManagedUpdate();
                }
            }

            // To Delete SoundEvent Instances
            if (toDeleteAdded) {
                toDeleteAdded = false;
                for (int i = 0; i < toDeleteInstances.Count; i++) {
                    managedUpdateTempInstanceEnumerator = toDeleteInstances[i].Current.Value.instanceDictionary.GetEnumerator();
                    while (managedUpdateTempInstanceEnumerator.MoveNext()) {
                        managedUpdateTempInstance = managedUpdateTempInstanceEnumerator.Current.Value;
                        managedUpdateTempInstance.PoolAllVoices(false, false, false);
                        UnityEngine.Object.Destroy(managedUpdateTempInstance);
                        UnityEngine.Object.Destroy(managedUpdateTempInstance.gameObject);
                    }
                    toDeleteInstances[i].Current.Value.instanceDictionary.Clear();
                    foreach (SoundEventInstance instance in toDeleteInstances[i].Current.Value.unusedInstanceStack) {
                        UnityEngine.Object.Destroy(instance);
                        UnityEngine.Object.Destroy(instance.gameObject);
                    }
                    toDeleteInstances[i].Current.Value.unusedInstanceStack.Clear();
                    soundEventDictionary.Remove(toDeleteInstances[i].Current.Key);
                    toDeleteInstances[i].Dispose();
                }
                toDeleteInstances.Clear();
            }

            // Waiting to Play (for TriggerOnTail)
            if (toPlayOnTailAdded) {
                toPlayOnTailAdded = false;
                for (int i = 0; i < toPlayOnTailInstances.Length; i++) {
                    if (toPlayOnTailInstances[i] != null) {
                        if (toPlayOnTailInstances[i].managedUpdateWaitingToPlayOnTail) {
                            toPlayOnTailInstances[i].managedUpdateWaitingToPlayOnTail = false;
                            toPlayOnTailInstances[i].TriggerOnTail();
                        }
                        toPlayOnTailInstances[i] = null;
                    }
                }
            }

            // Waiting to pool
            if (toPoolAdded) {
                toPoolAdded = false;
                for (int i = 0; i < toPoolInstances.Length; i++) {
                    if (toPoolInstances[i] != null) {
                        if (toPoolInstances[i].waitingForPooling) {
                            toPoolInstances[i].waitingForPooling = false;
                            // Nullcheck for deleting SoundEvents
                            if (toPoolInstances[i].soundEvent != null) {
                                // Move instance to stack
                                soundEventDictionary[toPoolInstances[i].soundEvent.GetAssetID()].instanceDictionary.Remove(toPoolInstances[i].ownerTransformInstanceID);
                                soundEventDictionary[toPoolInstances[i].soundEvent.GetAssetID()].unusedInstanceStack.Push(toPoolInstances[i]);
                                toPoolInstances[i].gameObject.SetActive(false);
                            }
                        }
                        toPoolInstances[i] = null;
                    }
                }
            }

            // Stops voices after a certain time
            for (int i = 0; i < voicePool.voiceIndexStopQueue.Count; i++) {
                if (voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].GetState() == VoiceState.Pause) {
                    if (voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].stopTime < SoundTimeScale.GetTimeRuntime()) {
                        voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].SetState(VoiceState.Stop, true);
                        voicePool.voicePool[voicePool.voiceIndexStopQueue.Peek()].cachedGameObject.SetActive(false);
                        voicePool.voiceIndexStopQueue.Dequeue();
                    } else {
                        break;
                    }
                } else {
                    voicePool.voiceIndexStopQueue.Dequeue();
                }
            }
        }

        // To Delete SoundEvent Instances
        [NonSerialized]
        private bool toDeleteAdded = false;
        [NonSerialized]
        private List<Dictionary<long, SoundEventInstanceDictionaryValue>.Enumerator> toDeleteInstances = new List<Dictionary<long, SoundEventInstanceDictionaryValue>.Enumerator>();

        [NonSerialized]
        private bool toPlayOnTailAdded = false;
        [NonSerialized]
        private SoundEventInstance[] toPlayOnTailInstances = new SoundEventInstance[0];

        public void AddManagedUpdateToPlayOnTail(SoundEventInstance soundEventInstance) {
            toPlayOnTailAdded = true;
            for (int i = 0; i < toPlayOnTailInstances.Length; i++) {
                // Find empty slot
                if (toPlayOnTailInstances[i] == null) {
                    toPlayOnTailInstances[i] = soundEventInstance;
                    return;
                } else {
                    // Done slot
                    if (!toPlayOnTailInstances[i].managedUpdateWaitingToPlayOnTail) {
                        toPlayOnTailInstances[i] = soundEventInstance;
                        return;
                    }
                }
            }
            // If all used, add new slot
            Array.Resize(ref toPlayOnTailInstances, toPlayOnTailInstances.Length + 1);
            toPlayOnTailInstances[toPlayOnTailInstances.Length - 1] = soundEventInstance;
        }

        [NonSerialized]
        private bool toPoolAdded = false;
        [NonSerialized]
        private SoundEventInstance[] toPoolInstances = new SoundEventInstance[0];

        public void AddManagedUpdateToPool(SoundEventInstance soundEventInstance) {
            toPoolAdded = true;
            for (int i = 0; i < toPoolInstances.Length; i++) {
                // Find empty slot
                if (toPoolInstances[i] == null) {
                    toPoolInstances[i] = soundEventInstance;
                    return;
                } else {
                    // Done slot
                    if (!toPoolInstances[i].waitingForPooling) {
                        toPoolInstances[i] = soundEventInstance;
                        return;
                    }
                }
            }
            // If all used, add new slot
            Array.Resize(ref toPoolInstances, toPoolInstances.Length + 1);
            toPoolInstances[toPoolInstances.Length - 1] = soundEventInstance;
        }

        [NonSerialized]
        private SoundEventInstance tempInstance;
        [NonSerialized]
        private SoundEventInstanceDictionaryValue tempInstanceDictionaryValue;
        [NonSerialized]
        private Dictionary<long, SoundEventInstanceDictionaryValue>.Enumerator tempInstanceDictionaryValueEnumerator;
        [NonSerialized]
        private Dictionary<int, SoundEventInstance>.Enumerator tempInstanceEnumerator;
        [NonSerialized]
        private CheckedDictionaryValueSoundEvent checkedDictionaryValueSoundEvent;
        [NonSerialized]
        private CheckedDictionaryValueSoundDataGroup checkedDictionaryValueSoundDataGroup;
        [NonSerialized]
        private CheckedDictionaryValueSoundMix checkedDictionaryValueSoundMix;
        [NonSerialized]
        private CheckedDictionaryValueSoundPhysicsCondition checkedDictionaryValueSoundPhysicsCondition;

        public void InternalPlay(
            SoundEventBase soundEvent, SoundEventPlayType playType, Transform owner, Vector3? positionVector, Transform positionTransform,
            SoundEventModifier soundEventModifierSoundPicker, SoundEventModifier soundEventModifierSoundTag, 
            SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {

            if (!settings.disablePlayingSounds && !applicationIsQuitting) {
                // If polyphony should be limited globally
                if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedGlobally) {
                    // Play only has the owner as position
                    if (playType == SoundEventPlayType.Play) {
                        positionTransform = owner;
                        playType = SoundEventPlayType.PlayAtTransform;
                    }
                    // If its played by music or 2D don't override owner, because then StopAllMusic won't work.
                    if (owner != cachedMusicTransform && owner != cached2DTransform) {
                        owner = SoundManagerBase.Instance.Internals.cachedSoundManagerTransform;
                    }
                }

                if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                    // Checks if the SoundEvent should retrigger itself
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                        if (!tempInstance.gameObject.activeSelf) {
                            tempInstance.gameObject.SetActive(true);
                        }
                        tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                        return;
                    } else {
                        // Checks if there is a unused instance to use
                        if (tempInstanceDictionaryValue.unusedInstanceStack.Count > 0) {
                            tempInstance = tempInstanceDictionaryValue.unusedInstanceStack.Pop();
                            tempInstanceDictionaryValue.instanceDictionary.Add(owner.GetInstanceID(), tempInstance);
                            if (!tempInstance.gameObject.activeSelf) {
                                tempInstance.gameObject.SetActive(true);
                            }
                            tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                            return;
                        }
                        // Create a new instance
                        GameObject tempGameObject = new GameObject();
                        tempGameObject.AddComponent<SoundEventInstance>();
                        tempInstance = tempGameObject.GetComponent<SoundEventInstance>();
                        tempInstance.Initialize(soundEvent);
                        tempInstance.GetTransform().parent = cachedSoundEventPoolTransform;
                        tempInstanceDictionaryValue.instanceDictionary.Add(owner.GetInstanceID(), tempInstance);
                        tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                        return;
                    }
                } else {
                    // Nullcheck SoundEvent
                    if (!checkedDictionarySoundEvent.TryGetValue(soundEvent.GetAssetID(), out checkedDictionaryValueSoundEvent)) {
                        checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                        checkedDictionarySoundEvent.Add(soundEvent.GetAssetID(), checkedDictionaryValueSoundEvent);
                    }
                    if (!checkedDictionaryValueSoundEvent.nullChecked) {
                        checkedDictionaryValueSoundEvent.nullChecked = true;
                        checkedDictionaryValueSoundEvent.isNull = soundEvent.IsNull();
#if UNITY_EDITOR
                        // So the problem can be fixed in runtime
                        checkedDictionaryValueSoundEvent.nullChecked = false;
#endif
                    }
                    if (checkedDictionaryValueSoundEvent.isNull) {
                        return;
                    }
                    // Create a new instance
                    GameObject tempGameObject = new GameObject();
                    tempGameObject.AddComponent<SoundEventInstance>();
                    tempInstance = tempGameObject.GetComponent<SoundEventInstance>();
                    tempInstance.Initialize(soundEvent);
                    tempInstance.GetTransform().parent = cachedSoundEventPoolTransform;
                    tempInstanceDictionaryValue = new SoundEventInstanceDictionaryValue();
                    tempInstanceDictionaryValue.soundEvent = soundEvent;
                    tempInstanceDictionaryValue.instanceDictionary.Add(owner.GetInstanceID(), tempInstance);
                    soundEventDictionary.Add(soundEvent.GetAssetID(), tempInstanceDictionaryValue);
                    tempInstance.Play(playType, owner, positionVector, positionTransform, soundEventModifierSoundPicker, soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                    return;
                }
            }
        }

        public void InternalStop(SoundEventBase soundEvent, Transform owner, bool allowFadeOut) {
            if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedPerOwner) {
                if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                        tempInstance.PoolAllVoices(allowFadeOut, true, false);
                        return;
                    }
                }
            } else if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedGlobally) {
                // If limited globally then it uses the SoundManager transform
                if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(SoundManagerBase.Instance.Internals.cachedSoundManagerTransform.GetInstanceID(), out tempInstance)) {
                        tempInstance.PoolAllVoices(allowFadeOut, true, false);
                        return;
                    }
                }
            }
        }

        public void InternalStopAtPosition(SoundEventBase soundEvent, Transform position, bool allowFadeOut) {
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                tempInstanceEnumerator = tempInstanceDictionaryValue.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PoolVoicesWithPositionTransform(position, allowFadeOut);
                }
            }
        }

        public void InternalStopAllAtOwner(Transform owner, bool allowFadeOut) {
            tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                if (tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    tempInstance.PoolAllVoices(allowFadeOut, true, false);
                }
            }
        }

        public void InternalStopEverywhere(SoundEventBase soundEvent, bool allowFadeOut) {
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                tempInstanceEnumerator = tempInstanceDictionaryValue.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PoolAllVoices(allowFadeOut, true, false);
                }
            }
        }

        public void InternalStopEverything(bool allowFadeOut) {
            tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                tempInstanceEnumerator = tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PoolAllVoices(allowFadeOut, true, false);
                }
            }
        }

        public void InternalOnDestroyForceStopEverything() {
            try {
                tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
                while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                    tempInstanceEnumerator = tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                    while (tempInstanceEnumerator.MoveNext()) {
                        tempInstanceEnumerator.Current.Value.PoolAllVoices(false, true, true);
                    }
                }
            } catch {

            }
        }

        public void InternalSetPauseUnpause(SoundEventBase soundEvent, Transform owner, bool pause, bool forcePause = false) {
            if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedPerOwner) {
                if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                        tempInstance.PauseUnpauseNormal(pause, forcePause);
                        return;
                    }
                }
            } else if (soundEvent.internals.data.polyphonyMode == PolyphonyMode.LimitedGlobally) {
                // If limited globally then it uses the SoundManager transform
                if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                    if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(SoundManagerBase.Instance.Internals.cachedSoundManagerTransform.GetInstanceID(), out tempInstance)) {
                        tempInstance.PauseUnpauseNormal(pause, forcePause);
                        return;
                    }
                }
            }
        }

        public void InternalSetPauseUnpauseAllAtOwner(Transform owner, bool pause, bool forcePause) {
            tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                if (tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    tempInstance.PauseUnpauseNormal(pause, forcePause);
                }
            }
        }

        public void InternalSetPauseUnpauseEverywhere(SoundEventBase soundEvent, bool pause, bool forcePause) {
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                tempInstanceEnumerator = tempInstanceDictionaryValue.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PauseUnpauseNormal(pause, forcePause);
                }
            }
        }

        public void InternalSetPauseUnpauseEverything(bool pause, bool forcePause) {
            tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
            while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                tempInstanceEnumerator = tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                while (tempInstanceEnumerator.MoveNext()) {
                    tempInstanceEnumerator.Current.Value.PauseUnpauseNormal(pause, forcePause);
                }
            }
        }

        public void InternalSetGlobalPauseUnpause(bool pause) {
            if (AudioListener.pause != pause) {
                AudioListener.pause = pause;
                settings.globalPause = pause;
                tempInstanceDictionaryValueEnumerator = soundEventDictionary.GetEnumerator();
                while (tempInstanceDictionaryValueEnumerator.MoveNext()) {
                    tempInstanceEnumerator = tempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                    while (tempInstanceEnumerator.MoveNext()) {
                        tempInstanceEnumerator.Current.Value.PauseUnpauseGlobal(pause);
                    }
                }
            }
        }

        public bool InternalGetGlobalPaused() {
            return settings.globalPause;
        }

        public void InternalSetGlobalVolumeRatio(float volumeRatio) {
            volumeRatio = Mathf.Clamp(volumeRatio, 0f, 1f);
            if (settings.globalVolumeRatio != volumeRatio) {
                settings.globalVolumeRatio = volumeRatio;
                settings.globalVolumeDecibel = VolumeScale.ConvertRatioToDecibel(settings.globalVolumeRatio);
#if SONITY_ENABLE_VOLUME_INCREASE
                AudioListener.volume = settings.globalVolumeRatio * VolumeScale.volumeIncrease60dbAudioListenerRatioMax;
#else
                AudioListener.volume = settings.globalVolumeRatio;
#endif
            }
        }

        public void InternalSetGlobalVolumeDecibel(float volumeDecibel) {
            volumeDecibel = Mathf.Clamp(volumeDecibel, Mathf.NegativeInfinity, 0f);
            if (settings.globalVolumeDecibel != volumeDecibel) {
                settings.globalVolumeDecibel = volumeDecibel;
                settings.globalVolumeRatio = VolumeScale.ConvertDecibelToRatio(volumeDecibel);
#if SONITY_ENABLE_VOLUME_INCREASE
                AudioListener.volume = settings.globalVolumeRatio * VolumeScale.volumeIncrease60dbAudioListenerRatioMax;
#else
                AudioListener.volume = settings.globalVolumeRatio;
#endif
            }
        }

        public float InternalGetGlobalVolumeRatio() {
            return settings.globalVolumeRatio;
        }

        public float InternalGetGlobalVolumeDecibel() {
            return settings.globalVolumeDecibel;
        }

        public SoundEventState InternalGetSoundEventState(SoundEventBase soundEvent, Transform owner) {
            if (soundEvent == null || owner == null) {
                return SoundEventState.NotPlaying;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetSoundEventState();
                    }
                }
            }
            return SoundEventState.NotPlaying;
        }

        public float InternalGetLastPlayedClipLength(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetLastPlayedAudioSourceClipLength(pitchSpeed);
                    }
                }
            }
            return 0f;
        }

        public float InternalGetLastPlayedClipTimeSeconds(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetLastPlayedAudioSourceTimeSeconds(pitchSpeed);
                    }
                }
            }
            return 0f;
        }

        public float InternalGetLastPlayedClipTimeRatio(SoundEventBase soundEvent, Transform owner) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetLastPlayedAudioSourceTimeRatio();
                    }
                }
            }
            return 0f;
        }

        public void InternalGetSpectrumData(SoundEventBase soundEvent, Transform owner, ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            if (soundEvent == null || owner == null) {
                return;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        tempInstance.GetSpectrumData(ref samples, channel, window, spectrumDataFrom);
                    }
                }
            }
        }

        public AudioSource InternalGetLastPlayedAudioSource(SoundEventBase soundEvent, Transform owner) {
            if (soundEvent == null || owner == null) {
                return null;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetLastPlayedAudioSource();
                    }
                }
            }
            return null;
        }

        public float InternalGetMaxLength(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return 0f;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent.GetAssetID(), out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent.GetAssetID(), checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.savedMaxChecked) {
                checkedDictionaryValueSoundEvent.savedMaxChecked = true;
                checkedDictionaryValueSoundEvent.savedMaxLength = soundEvent.internals.GetMaxLengthWithPitchAndTimeline();
            }
            return checkedDictionaryValueSoundEvent.savedMaxLength;
        }

        public bool InternalCheckTriggerOnTailLengthTooShort(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return true;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent.GetAssetID(), out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent.GetAssetID(), checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.savedMinLengthChecked) {
                checkedDictionaryValueSoundEvent.savedMinLengthChecked = true;
                checkedDictionaryValueSoundEvent.savedMinLength = soundEvent.internals.GetMinLengthFirstSoundContainerWithoutPitch();
                if (checkedDictionaryValueSoundEvent.savedMinLength < soundEvent.internals.data.triggerOnTailLength) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundEvent.savedMinLengthChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)} \"" + soundEvent.name + "\" Trigger On Tail: Tail Length is longer than the shortest AudioClip on the first SoundContainer.", soundEvent);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool InternalCheckTriggerOnPlayIsInfiniteLoop(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return true;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent.GetAssetID(), out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent.GetAssetID(), checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoopChecked) {
                checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoopChecked = true;
                checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoop =
                    soundEvent.internals.data.GetIfInfiniteLoop(soundEvent, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnPlay);
                if (checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)} TriggerOnPlay: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                    return true;
                }
            }
            return checkedDictionaryValueSoundEvent.triggerOnPlayIsInfiniteLoop;
        }

        public bool InternalCheckTriggerOnStopIsInfiniteLoop(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return true;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent.GetAssetID(), out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent.GetAssetID(), checkedDictionaryValueSoundEvent);
            }
            if (!checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoopChecked) {
                checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoopChecked = true;
                checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoop =
                    soundEvent.internals.data.GetIfInfiniteLoop(soundEvent, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnStop);
                if (checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)} TriggerOnStop: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                    return true;
                }
            }
            return checkedDictionaryValueSoundEvent.triggerOnStopIsInfiniteLoop;
        }

        public bool InternalCheckSoundDataGroupIsInfiniteLoop(SoundDataGroupBase soundDataGroup) {
            if (soundDataGroup == null) {
                return true;
            }
            if (!checkedDictionarySoundDataGroup.TryGetValue(soundDataGroup.GetAssetID(), out checkedDictionaryValueSoundDataGroup)) {
                checkedDictionaryValueSoundDataGroup = new CheckedDictionaryValueSoundDataGroup();
                checkedDictionarySoundDataGroup.Add(soundDataGroup.GetAssetID(), checkedDictionaryValueSoundDataGroup);
            }
            if (!checkedDictionaryValueSoundDataGroup.isInfiniteLoopChecked) {
                checkedDictionaryValueSoundDataGroup.isInfiniteLoopChecked = true;
                checkedDictionaryValueSoundDataGroup.isInfiniteLoop = soundDataGroup.internals.GetIfInfiniteLoop(soundDataGroup, out SoundDataGroupBase infiniteInstigator, out SoundDataGroupBase infinitePrevious);
                if (checkedDictionaryValueSoundDataGroup.isInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundDataGroup.isInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundDataGroup)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
            }
            return checkedDictionaryValueSoundDataGroup.isInfiniteLoop;
        }

        public bool InternalCheckSoundMixIsInfiniteLoop(SoundMixBase soundMix) {
            if (soundMix == null) {
                return true;
            }
            if (!checkedDictionarySoundMix.TryGetValue(soundMix.GetAssetID(), out checkedDictionaryValueSoundMix)) {
                checkedDictionaryValueSoundMix = new CheckedDictionaryValueSoundMix();
                checkedDictionarySoundMix.Add(soundMix.GetAssetID(), checkedDictionaryValueSoundMix);
            }
            if (!checkedDictionaryValueSoundMix.isInfiniteLoopChecked) {
                checkedDictionaryValueSoundMix.isInfiniteLoopChecked = true;
                checkedDictionaryValueSoundMix.isInfiniteLoop = soundMix.internals.GetIfInfiniteLoop(soundMix, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious);
                if (checkedDictionaryValueSoundMix.isInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundMix.isInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundMix)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
            }
            return checkedDictionaryValueSoundMix.isInfiniteLoop;
        }

        public bool InternalCheckSoundPhysicsConditionIsInfiniteLoop(SoundPhysicsConditionBase condition) {
            if (condition == null) {
                return true;
            }
            if (!checkedDictionarySoundPhysicsCondition.TryGetValue(condition.GetAssetID(), out checkedDictionaryValueSoundPhysicsCondition)) {
                checkedDictionaryValueSoundPhysicsCondition = new CheckedDictionaryValueSoundPhysicsCondition();
                checkedDictionarySoundPhysicsCondition.Add(condition.GetAssetID(), checkedDictionaryValueSoundPhysicsCondition);
            }
            if (!checkedDictionaryValueSoundPhysicsCondition.isInfiniteLoopChecked) {
                checkedDictionaryValueSoundPhysicsCondition.isInfiniteLoopChecked = true;
                checkedDictionaryValueSoundPhysicsCondition.isInfiniteLoop = condition.internals.GetIfInfiniteLoop(condition, out SoundPhysicsConditionBase infiniteInstigator, out SoundPhysicsConditionBase infinitePrevious);
                if (checkedDictionaryValueSoundPhysicsCondition.isInfiniteLoop) {
#if UNITY_EDITOR
                    // So the problem can be fixed in runtime
                    checkedDictionaryValueSoundPhysicsCondition.isInfiniteLoopChecked = false;
#endif
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPhysicsCondition)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
            }
            return checkedDictionaryValueSoundPhysicsCondition.isInfiniteLoop;
        }

#if UNITY_EDITOR
        public int InternalStatisticsNumberOfPlays(SoundEventBase soundEvent, bool increment) {
            if (soundEvent == null) {
                return 0;
            }
            if (!checkedDictionarySoundEvent.TryGetValue(soundEvent.GetAssetID(), out checkedDictionaryValueSoundEvent)) {
                checkedDictionaryValueSoundEvent = new CheckedDictionaryValueSoundEvent();
                checkedDictionarySoundEvent.Add(soundEvent.GetAssetID(), checkedDictionaryValueSoundEvent);
            }
            if (increment) {
                checkedDictionaryValueSoundEvent.statisticsNumberOfPlays++;
            }
            return checkedDictionaryValueSoundEvent.statisticsNumberOfPlays;
        }
#endif

        public float InternalGetTimePlayed(SoundEventBase soundEvent, Transform owner) {
            if (soundEvent == null || owner == null) {
                return 0f;
            }
            if (soundEventDictionary.TryGetValue(soundEvent.GetAssetID(), out tempInstanceDictionaryValue)) {
                if (tempInstanceDictionaryValue.instanceDictionary.TryGetValue(owner.GetInstanceID(), out tempInstance)) {
                    if (tempInstance != null) {
                        return tempInstance.GetTimePlayed();
                    }
                }
            }
            return 0f;
        }

        public bool InternalGetContainsLoop(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                return false;
            }
            soundEvent.internals.GetContainsLoop();
            return false;
        }

        public void InternalLoadUnloadAudioData(SoundEventBase soundEvent, bool load) {
            if (soundEvent == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(null, null, null, null));
                }
            } else {
                if (soundEvent.internals.soundContainers.Length == 0) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} has no {nameof(NameOf.SoundContainer)}s." + DebugInfoString(soundEvent, null, null, null), soundEvent);
                    }
                } else {
                    for (int i = 0; i < soundEvent.internals.soundContainers.Length; i++) {
                        if (soundEvent.internals.soundContainers[i] == null) {
                            if (ShouldDebug.Warnings()) {
                                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} has a null {nameof(NameOf.SoundContainer)}." + DebugInfoString(soundEvent, null, null, null), soundEvent);
                            }
                        } else {
                            soundEvent.internals.soundContainers[i].internals.LoadUnloadAudioData(load, soundEvent.internals.soundContainers[i]);
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        [NonSerialized]
        private List<SoundEventBase> soloSoundEvents = new List<SoundEventBase>();

        public bool GetSoloEnabled() {
            for (int i = soloSoundEvents.Count - 1; i >= 0; i--) {
                if (soloSoundEvents[i] != null && soloSoundEvents[i].internals.data.soloEnable) {
                    return true;
                } else {
                    soloSoundEvents.RemoveAt(i);
                }
            }
            return false;
        }

        public bool GetIsInSolo(SoundEventBase soundEvent) {
            if (soloSoundEvents.Contains(soundEvent)) {
                return true;
            }
            return false;
        }

        public void AddSolo(SoundEventBase soundEvent) {
            if (!soloSoundEvents.Contains(soundEvent)) {
                soloSoundEvents.Add(soundEvent);
            }
        }
#endif

        public string DebugInfoString(SoundEventBase soundEvent, Transform transform, Transform position, Transform owner) {
            string tempString = "";
            bool previousNull = true;

            if (soundEvent != null) {
                previousNull = false;
                tempString += " \"" + soundEvent.name + $"\" ({nameof(NameOf.SoundEvent)})";
            } else {
                previousNull = true;
            }
            if (transform != null) {
                if (!previousNull) {
                    tempString += ",";
                }
                previousNull = false;
                tempString += " \"" + transform.name + $"\" (Transform)";
            } else {
                previousNull = true;
            }
            if (position != null) {
                if (!previousNull) {
                    tempString += ",";
                }
                previousNull = false;
                tempString += " \"" + position.name + $"\" (position Transform)";
            } else {
                previousNull = true;
            }
            if (owner != null) {
                if (!previousNull) {
                    tempString += ",";
                }
                previousNull = false;
                tempString += " \"" + owner.name + $"\" (owner Transform)";
            } else {
                previousNull = true;
            }
            // Add first At
            if (tempString != "") {
                tempString = " Using" + tempString + ".";
            }
            return tempString;
        }

        public void Play(SoundEventBase soundEvent, Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, null, null, null);
                }
            }
        }

        public void Play(SoundEventBase soundEvent, Transform owner, SoundTagBase localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, null, null, localSoundTag);
                }
            }
        }

        public void Play(SoundEventBase soundEvent, Transform owner, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, soundParameters, null, null);
                }
            }
        }

        public void Play(SoundEventBase soundEvent, Transform owner, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, null, null, soundParameters, null, localSoundTag);
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, null, null, null);
                    }
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position, SoundTagBase localSoundTag) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, null, null, localSoundTag);
                    }
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, soundParameters, null, null);
                    }
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Transform position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The position {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The owner {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, position, owner), soundEvent);
                    }
                } else {
                    if (soundEvent == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, position, owner), owner);
                        }
                    } else {
                        InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, null, null, soundParameters, null, localSoundTag);
                    }
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, null, null, null);
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position, SoundTagBase localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, null, null, localSoundTag);
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, soundParameters, null, null);
                }
            }
        }

        public void PlayAtPosition(SoundEventBase soundEvent, Transform owner, Vector3 position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(soundEvent, null, null, owner), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, null, null, owner), owner);
                    }
                } else {
                    InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, null, null, soundParameters, null, localSoundTag);
                }
            }
        }

        public void Stop(SoundEventBase soundEvent, Transform owner, bool allowFadeOut = true) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalStop(soundEvent, owner, allowFadeOut);
                }
            }
        }

        public void StopAtPosition(SoundEventBase soundEvent, Transform position, bool allowFadeOut = true) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(soundEvent, position, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, position, null, null), position);
                    }
                } else {
                    InternalStopAtPosition(soundEvent, position, allowFadeOut);
                }
            }
        }

        public void StopAllAtOwner(Transform owner, bool allowFadeOut = true) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(null, owner, null, null));
                }
            } else {
                InternalStopAllAtOwner(owner, allowFadeOut);
            }
        }

        public void StopEverywhere(SoundEventBase soundEvent, bool allowFadeOut = true) {
            if (soundEvent == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, null, null, null));
                }
            } else {
                InternalStopEverywhere(soundEvent, allowFadeOut);
            }
        }

        public void StopEverything(bool allowFadeOut = true) {
            InternalStopEverything(allowFadeOut);
        }

        public void Pause(SoundEventBase soundEvent, Transform owner, bool forcePause = false) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalSetPauseUnpause(soundEvent, owner, true, forcePause);
                }
            }
        }

        public void Unpause(SoundEventBase soundEvent, Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null." + DebugInfoString(soundEvent, owner, null, null), soundEvent);
                }
            } else {
                if (soundEvent == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null." + DebugInfoString(soundEvent, owner, null, null), owner);
                    }
                } else {
                    InternalSetPauseUnpause(soundEvent, owner, false, false);
                }
            }
        }

        public void PauseAllAtOwner(Transform owner, bool forcePause) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(null, owner, null, null));
                }
            } else {
                InternalSetPauseUnpauseAllAtOwner(owner, true, forcePause);
            }
        }

        public void UnpauseAllAtOwner(Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(Transform)} is null" + DebugInfoString(null, owner, null, null));
                }
            } else {
                InternalSetPauseUnpauseAllAtOwner(owner, false, false);
            }
        }

        public void PauseEverywhere(SoundEventBase soundEvent, bool forcePause) {
            if (soundEvent == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, null, null, null));
                }
            } else {
                InternalSetPauseUnpauseEverywhere(soundEvent, true, forcePause);
            }
        }

        public void UnpauseEverywhere(SoundEventBase soundEvent) {
            if (soundEvent == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)}: The {nameof(NameOf.SoundEvent)} is null" + DebugInfoString(soundEvent, null, null, null));
                }
            } else {
                InternalSetPauseUnpauseEverywhere(soundEvent, false, false);
            }
        }

        public void PauseEverything(bool forcePause = false) {
            InternalSetPauseUnpauseEverything(true, forcePause);
        }

        public void UnpauseEverything() {
            InternalSetPauseUnpauseEverything(false, false);
        }

        public void SetGlobalPause() {
            InternalSetGlobalPauseUnpause(true);
        }

        public void SetGlobalUnpause() {
            InternalSetGlobalPauseUnpause(false);
        }

        public bool GetGlobalPaused() {
            return InternalGetGlobalPaused();
        }

        public void SetGlobalVolumeRatio(float volumeRatio) {
            InternalSetGlobalVolumeRatio(volumeRatio);
        }

        public void SetGlobalVolumeDecibel(float volumeDecibel) {
            InternalSetGlobalVolumeDecibel(volumeDecibel);
        }

        public float GetGlobalVolumeRatio() {
            return InternalGetGlobalVolumeRatio();
        }

        public float GetGlobalVolumeDecibel() {
            return InternalGetGlobalVolumeDecibel();
        }

        public void Play2D(SoundEventBase soundEvent) {
            Play(soundEvent, cached2DTransform);
        }

        public void Play2D(SoundEventBase soundEvent, SoundTagBase localSoundTag) {
            Play(soundEvent, cached2DTransform, localSoundTag);
        }

        public void Play2D(SoundEventBase soundEvent, params SoundParameterInternals[] soundParameters) {
            Play(soundEvent, cached2DTransform, soundParameters);
        }

        public void Play2D(SoundEventBase soundEvent, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            Play(soundEvent, cached2DTransform, localSoundTag, soundParameters);
        }

        public void Play2DAtPosition(SoundEventBase soundEvent, Vector3 position) {
            PlayAtPosition(soundEvent, cached2DTransform, position);
        }

        public void Play2DAtPosition(SoundEventBase soundEvent, Transform position) {
            PlayAtPosition(soundEvent, cached2DTransform, position);
        }

        public void Stop2D(SoundEventBase soundEvent, bool allowFadeOut = true) {
            Stop(soundEvent, cached2DTransform, allowFadeOut);
        }

        public void StopAll2D(bool allowFadeOut = true) {
            StopAllAtOwner(cached2DTransform, allowFadeOut);
        }

        public void Pause2D(SoundEventBase soundEvent, bool forcePause = false) {
            Pause(soundEvent, cached2DTransform, forcePause);
        }

        public void Unpause2D(SoundEventBase soundEvent) {
            Unpause(soundEvent, cached2DTransform);
        }

        public void PauseAll2D(bool forcePause = false) {
            PauseAllAtOwner(cached2DTransform, forcePause);
        }

        public void UnpauseAll2D() {
            UnpauseAllAtOwner(cached2DTransform);
        }

        public SoundEventState Get2DSoundEventState(SoundEventBase soundEvent) {
            return InternalGetSoundEventState(soundEvent, cached2DTransform);
        }

        public float Get2DLastPlayedClipLength(SoundEventBase soundEvent, bool pitchSpeed) {
            return InternalGetLastPlayedClipLength(soundEvent, cached2DTransform, pitchSpeed);
        }

        public float Get2DLastPlayedClipTimeSeconds(SoundEventBase soundEvent, bool pitchSpeed) {
            return InternalGetLastPlayedClipTimeSeconds(soundEvent, cached2DTransform, pitchSpeed);
        }

        public float Get2DLastPlayedClipTimeRatio(SoundEventBase soundEvent) {
            return InternalGetLastPlayedClipTimeRatio(soundEvent, cached2DTransform);
        }

        public float Get2DTimePlayed(SoundEventBase soundEvent) {
            return InternalGetTimePlayed(soundEvent, cached2DTransform);
        }

        public Transform Get2DTransform() {
            return cached2DTransform;
        }

        public void PlayMusic(SoundEventBase soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true) {
            if (stopAllOtherMusic) {
                StopAllMusic(allowFadeOut);
            }
            Play(soundEvent, cachedMusicTransform);
        }

        public void PlayMusic(SoundEventBase soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true, params SoundParameterInternals[] soundParameters) {
            if (stopAllOtherMusic) {
                StopAllMusic(allowFadeOut);
            }
            Play(soundEvent, cachedMusicTransform, soundParameters);
        }

        public void StopMusic(SoundEventBase soundEvent, bool allowFadeOut = true) {
            Stop(soundEvent, cachedMusicTransform, allowFadeOut);
        }

        public void StopAllMusic(bool allowFadeOut = true) {
            StopAllAtOwner(cachedMusicTransform, allowFadeOut);
        }

        public void PauseMusic(SoundEventBase soundEvent, bool forcePause = false) {
            Pause(soundEvent, cachedMusicTransform, forcePause);
        }

        public void UnpauseMusic(SoundEventBase soundEvent) {
            Unpause(soundEvent, cachedMusicTransform);
        }

        public void PauseAllMusic(bool forcePause = false) {
            PauseAllAtOwner(cachedMusicTransform, forcePause);
        }

        public void UnpauseAllMusic() {
            UnpauseAllAtOwner(cachedMusicTransform);
        }

        public SoundEventState GetMusicSoundEventState(SoundEventBase soundEvent) {
            return InternalGetSoundEventState(soundEvent, cachedMusicTransform);
        }

        public float GetMusicLastPlayedClipLength(SoundEventBase soundEvent, bool pitchSpeed) {
            return InternalGetLastPlayedClipLength(soundEvent, cachedMusicTransform, pitchSpeed);
        }

        public float GetMusicLastPlayedClipTimeSeconds(SoundEventBase soundEvent, bool pitchSpeed) {
            return InternalGetLastPlayedClipTimeSeconds(soundEvent, cachedMusicTransform, pitchSpeed);
        }

        public float GetMusicLastPlayedClipTimeRatio(SoundEventBase soundEvent) {
            return InternalGetLastPlayedClipTimeRatio(soundEvent, cachedMusicTransform);
        }

        public float GetMusicTimePlayed(SoundEventBase soundEvent) {
            return InternalGetTimePlayed(soundEvent, cachedMusicTransform);
        }

        public Transform GetMusicTransform() {
            return cachedMusicTransform;
        }

        public SoundEventState GetSoundEventState(SoundEventBase soundEvent, Transform owner) {
            return InternalGetSoundEventState(soundEvent, owner);
        }

        public float GetLastPlayedClipLength(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            return InternalGetLastPlayedClipLength(soundEvent, owner, pitchSpeed);
        }

        public float GetLastPlayedClipTimeSeconds(SoundEventBase soundEvent, Transform owner, bool pitchSpeed) {
            return InternalGetLastPlayedClipTimeSeconds(soundEvent, owner, pitchSpeed);
        }

        public float GetLastPlayedClipTimeRatio(SoundEventBase soundEvent, Transform owner) {
            return InternalGetLastPlayedClipTimeRatio(soundEvent, owner);
        }

        public void GetSpectrumData(SoundEventBase soundEvent, Transform owner, ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            InternalGetSpectrumData(soundEvent, owner, ref samples, channel, window, spectrumDataFrom);
        }

        public AudioSource GetLastPlayedAudioSource(SoundEventBase soundEvent, Transform owner) {
            return InternalGetLastPlayedAudioSource(soundEvent, owner);
        }

        public float GetMaxLength(SoundEventBase soundEvent) {
            return InternalGetMaxLength(soundEvent);
        }

        public float GetTimePlayed(SoundEventBase soundEvent, Transform owner) {
            return InternalGetTimePlayed(soundEvent, owner);
        }

        public bool GetContainsLoop(SoundEventBase soundEvent) {
            return InternalGetContainsLoop(soundEvent);
        }

        public Transform GetSoundManagerTransform() {
            return cachedSoundManagerTransform;
        }

        public void LoadAudioData(SoundEventBase soundEvent) {
            InternalLoadUnloadAudioData(soundEvent, true);
        }

        public void UnloadAudioData(SoundEventBase soundEvent) {
            InternalLoadUnloadAudioData(soundEvent, false);
        }

        public void SetGlobalSoundTag(SoundTagBase soundTag) {
            settings.globalSoundTag = soundTag;
        }

        public SoundTagBase GetGlobalSoundTag() {
            return settings.globalSoundTag;
        }

        public void SetGlobalDistanceScale(float distanceScale) {
            settings.distanceScale = Mathf.Clamp(distanceScale, 0f, Mathf.Infinity);
        }

        public float GetGlobalDistanceScale() {
            return settings.distanceScale;
        }

        public void SetSpeedOfSoundEnabled(bool speedOfSoundEnabled) {
            settings.speedOfSoundEnabled = speedOfSoundEnabled;
        }

        public void SetSpeedOfSoundScale(float speedOfSoundScale) {
            settings.speedOfSoundScale = Mathf.Clamp(speedOfSoundScale, 0f, Mathf.Infinity);
        }

        public float GetSpeedOfSoundScale() {
            return settings.speedOfSoundScale;
        }

        public void SetVoiceLimit(int voiceLimit) {
            settings.voiceLimit = Mathf.Clamp(voiceLimit, 0, int.MaxValue);
        }

        public int GetVoiceLimit() {
            return settings.voiceLimit;
        }

        public void SetVoiceEffectLimit(int voiceEffectLimit) {
            settings.voiceEffectLimit = Mathf.Clamp(voiceEffectLimit, 0, int.MaxValue);
        }

        public int GetVoiceEffectLimit() {
            return settings.voiceEffectLimit;
        }

        public void SetDisablePlayingSounds(bool disablePlayingSounds) {
            settings.disablePlayingSounds = disablePlayingSounds;
        }

        public bool GetDisablePlayingSounds() {
            return settings.disablePlayingSounds;
        }

        public AudioMixer GetAdressableAudioMixer() {
#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
            return settings.GetAdressableAudioMixer();
#else
            Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: The Script Define Symbol \"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\" is false but you're trying to access the Adressable AudioMixer");
            return null;
#endif
        }
    }
}