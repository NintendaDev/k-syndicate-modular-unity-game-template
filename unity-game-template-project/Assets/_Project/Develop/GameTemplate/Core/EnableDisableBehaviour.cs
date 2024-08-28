using UnityEngine;

namespace GameTemplate.Core
{
    public class EnableDisableBehaviour : MonoBehaviour
    {
        private GameObject _currentGameObject;

        private GameObject CurrentGameObject
        {
            get
            {
                if (_currentGameObject == null)
                    _currentGameObject = gameObject;

                return _currentGameObject;
            }
        }

        public virtual void Enable() =>
            CurrentGameObject.SetActive(true);

        public virtual void Disable() =>
            CurrentGameObject.SetActive(false);
    }
}
