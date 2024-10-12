using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Core.UI
{
    public class LinkedPanelView<TViewType> : MonoBehaviour
        where TViewType : MonoBehaviour
    {
        [SerializeField, Required] private Transform _viewsRoot;

        private IEnumerable<TViewType> _levelViews;

        public event Action Enabled;

        private void OnEnable()
        {
            Enabled?.Invoke();
        }

        public void Link(IEnumerable<TViewType> levelViews)
        {
            Clear();
            _levelViews = levelViews;
            _levelViews.ForEach(x => x.transform.SetParent(_viewsRoot));
        }

        private void Clear() =>
            _levelViews?.ForEach(x => Destroy(x.gameObject));
    }
}
