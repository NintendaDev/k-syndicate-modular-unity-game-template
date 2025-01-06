using System.Linq;
using Modules.AudioManagement.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AudioManagement.Configurations
{
    [CreateAssetMenu(fileName = "new SoundConfiguration", menuName = "Modules/AudioManagement/SoundConfiguration")]
    public sealed class SoundConfiguration : ScriptableObject
    {
        [ValidateInput(nameof(IsUniqueSoundEvent))] 
        [SerializeField] private AudioAsset[] _audioAssets;

        public bool IsExistAudioAsset(AudioCode audioCode, out AudioAsset audioAsset)
        {
            audioAsset = _audioAssets.FirstOrDefault(x => x.Code == audioCode);

            return audioAsset.Code != AudioCode.None;
        }

        private bool IsUniqueSoundEvent(AudioAsset[] data, ref string errorMessage)
        {
            if (data.Length != data.GroupBy(x => x.Code).Count())
            {
                errorMessage = "";

                return false;
            }

            return true;
        }
    }
}