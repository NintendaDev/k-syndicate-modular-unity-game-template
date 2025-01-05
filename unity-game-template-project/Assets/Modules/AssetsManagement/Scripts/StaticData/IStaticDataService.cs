using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.AssetsManagement.StaticData
{
    public interface IStaticDataService 
    {
        public UniTask InitializeAsync();
        
        public TConfigType GetConfiguration<TConfigType>() where TConfigType : ScriptableObject;
    }
}
