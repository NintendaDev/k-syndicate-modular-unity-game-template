using System;

namespace GameTemplate.Services.Authorization
{
    public interface IAuthorizationService : ILoginInfo
    {
        public event Action LoginCompleted;

        public event Action LoginError;

        public void StartAuthorizationBehaviour();
    }
}