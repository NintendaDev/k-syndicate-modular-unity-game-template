using System;

namespace Modules.Authorization.Interfaces
{
    public sealed class DummyAuthorizationService : IAuthorizationService
    {
        public event Action LoginCompleted;

        public event Action LoginError;

        public bool IsLogined { get; private set; }

        public void StartAuthorizationBehaviour()
        {
            IsLogined = true;
            LoginCompleted?.Invoke();
        }
    }
}
