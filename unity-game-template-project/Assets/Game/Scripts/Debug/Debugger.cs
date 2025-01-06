using Modules.AudioManagement.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Debug
{
    public class Debugger : MonoBehaviour
    {
        [Inject]
        [ShowInInspector, HideInEditorMode]
        private IAudioAssetPlayer _audioAssetPlayer;
    }
}