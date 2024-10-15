using System;

namespace Modules.Authorization.Interfaces
{
    public interface IAuthorizationService : ILoginInfo
    {
        public event Action LoginCompleted;

        public event Action LoginError;

        public void StartAuthorizationBehaviour();
    }
}