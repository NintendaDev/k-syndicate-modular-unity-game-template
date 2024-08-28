using GameTemplate.UI.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate.UI.GameHub.LevelsMenu.Views
{
    public class LevelsMenuView : ViewWithBackButton
    {
        [SerializeField, Required] private LevelsPanelView _levelsPanelView;

        public void Link(IEnumerable<LevelView> levelViews) =>
            _levelsPanelView.Link(levelViews);   
    }
}
