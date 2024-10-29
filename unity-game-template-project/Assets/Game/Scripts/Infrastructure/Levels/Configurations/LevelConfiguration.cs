using Modules.AudioManagement.Clip;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.Infrastructure.Levels.Configurations
{
    [CreateAssetMenu(fileName = "new LevelConfiguration", menuName = "GameTemplate/Levels/LevelConfiguration")]
    public sealed class LevelConfiguration : ScriptableObject, IReferenceAudio
    {
        [SerializeField, IsNotNoneLevelCode] private LevelCode _levelCode;
        [SerializeField, Required] private string _title;
        [SerializeField, Required, ValidateInput(nameof(IsSceneAsset))] private AssetReference _scene;
        [SerializeField] private AssetReferenceT<AudioClip> _audioReference;

        public LevelCode LevelCode => _levelCode;

        public string Title => _title;

        public string SceneAddress => _scene.AssetGUID;

        public AssetReferenceT<AudioClip> AudioReference => _audioReference;

        private bool IsSceneAsset(AssetReference sceneReference)
        {
#if UNITY_EDITOR
            return sceneReference != null && sceneReference.editorAsset is SceneAsset;
#else
            return true;
#endif
        }
    }
}
