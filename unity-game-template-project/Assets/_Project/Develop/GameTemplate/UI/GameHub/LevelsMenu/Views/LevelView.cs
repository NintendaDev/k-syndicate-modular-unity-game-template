using System;
using GameTemplate.Core;
using GameTemplate.Infrastructure.Levels;
using GameTemplate.UI.Core.Buttons;

namespace GameTemplate.UI.GameHub.LevelsMenu.Views
{
    public class LevelView : UITextButton, IDestroyEvent
    {
        public event Action Destroyed;
        
        public LevelCode LevelCode { get; private set; }

        public void Set(LevelCode levelCode) =>
            LevelCode = levelCode;
    }
}
