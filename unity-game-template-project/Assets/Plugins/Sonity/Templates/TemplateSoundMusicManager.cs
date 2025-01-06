// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using Sonity;

namespace SonityTemplate {

    /// <summary>
    /// Template of a singleton music playback system.
    /// Add to a GameObject in the scene and use like this:
    /// SonityTemplate.TemplateSoundMusicManager.Instance.PlayMainMenu();
    /// </summary>
    [AddComponentMenu("Sonity/Template - Sound Music Manager")]
    public class TemplateSoundMusicManager : MonoBehaviour {

        // Music
        public SoundEvent musicMainMenu;
        public SoundEvent musicIngame;

#if UNITY_EDITOR
        // Debug
        private bool debugMusicPlay = false;
#endif
        public MusicPlaying GetMusicPlaying() {
            return musicPlaying;
        }

        private MusicPlaying musicPlaying = MusicPlaying.None;

        // Expand with more music to your hearts content
        public enum MusicPlaying {
            None,
            MainMenu,
            Ingame,
        }

        public void PlayMainMenu(bool allowFadeOut = true) {
            // Making sure it doesnt retrigger
            if (musicPlaying != MusicPlaying.MainMenu) {
                musicPlaying = MusicPlaying.MainMenu;
                // Playing the music, stopping all other music
                SoundManager.Instance.PlayMusic(musicMainMenu, true, allowFadeOut);
#if UNITY_EDITOR
                if (debugMusicPlay) {
                    Debug.Log(gameObject.name + " Play: " + musicMainMenu.name);
                }
#endif
            }
        }

        public void PlayIngame(bool allowFadeOut = true) {

            // Making sure it doesnt retrigger
            if (musicPlaying != MusicPlaying.Ingame) {
                musicPlaying = MusicPlaying.Ingame;
                // Playing the music, stopping all other music and allowing fade out
                SoundManager.Instance.PlayMusic(musicIngame, true, allowFadeOut);
#if UNITY_EDITOR
                if (debugMusicPlay) {
                    Debug.Log(gameObject.name + " Play: " + musicIngame.name);
                }
#endif
            }
        }

        public void StopAllMusic(bool allowFadeOut = true) {
            // Making sure it doesnt retrigger
            if (musicPlaying != MusicPlaying.None) {
                musicPlaying = MusicPlaying.None;
                // Stopping music and allowing fade out
                SoundManager.Instance.StopAllMusic(allowFadeOut);
#if UNITY_EDITOR
                if (debugMusicPlay) {
                    Debug.Log(gameObject.name + " Stop All Music");
                }
#endif
            }
        }

        // Singleton Instance
        public bool useDontDestroyOnLoad = true;
        private bool isGoingToDelete;
#if UNITY_EDITOR
        private bool debugInstanceDestroyed = true;
#endif

        public static TemplateSoundMusicManager Instance {
            get;
            private set;
        }

        private void Awake() {
            InstanceCheck();
            if (useDontDestroyOnLoad && !isGoingToDelete) {
                // DontDestroyOnLoad only works for root GameObjects
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Checks if there are multiple instances this script, if so it destroys one of them
        private void InstanceCheck() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
#if UNITY_EDITOR
                if (debugInstanceDestroyed) {
                    Debug.LogWarning($"There can only be one {nameof(TemplateSoundMusicManager)} instance per scene.", this);
                }
#endif
                // So that it does not run the rest of the Awake and Update code
                isGoingToDelete = true;
                if (Application.isPlaying) {
                    Destroy(gameObject);
                }
            }
        }
    }
}