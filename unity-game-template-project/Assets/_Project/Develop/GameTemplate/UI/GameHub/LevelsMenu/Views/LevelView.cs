using System;
using GameTemplate.Infrastructure.Levels;
using Modules.Core.Systems;
using Modules.Core.UI;

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
