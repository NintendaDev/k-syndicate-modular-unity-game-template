using Sirenix.OdinInspector;

using UnityEngine;

namespace Modules.SaveManagement.Systems
{
    [CreateAssetMenu(fileName = "new SaveConfiguration", menuName = "Modules/SaveManagement/SaveConfiguration")]
    public sealed class SaveConfiguration : ScriptableObject
    {
        [SerializeField] private bool _isEnableEncryption;

        [ShowIf(nameof(_isEnableEncryption))]
        [SerializeField] private string _password;

        [SerializeField] private string _saveKey = "PlayerProgress";

        public string SaveKey => _saveKey;

        public bool IsEnableEncryption => _isEnableEncryption;    

        public string Password => _password;
    }
}
