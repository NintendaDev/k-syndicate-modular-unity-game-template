using System;
using System.Collections.Generic;
using Zenject;

namespace Modules.MusicManagement.Clip
{
    public sealed class AddressableAudioClipFactory : IDisposable
    {
        private readonly DiContainer _diContainer;
        private List<IDisposable> _disposableObjects = new();

        public AddressableAudioClipFactory(DiContainer diContainer) =>
            _diContainer = diContainer;

        public void Dispose() =>
            _disposableObjects.ForEach(x => x.Dispose());

        public AddressableAudioClip Create()
        {
            AddressableAudioClip clip = _diContainer.Resolve<AddressableAudioClip>();
            _disposableObjects.Add(clip);

            return clip;
        }  
    }
}