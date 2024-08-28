using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameTemplate.Services.StaticData
{
    public interface IStaticDataService 
    {
        public UniTask InitializeAsync();

        public TConfigType GetConfiguration<TConfigType>() where TConfigType : ScriptableObject;
    }
}
