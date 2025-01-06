using System;
using Game.Infrastructure.Levels;
using Modules.Core.Systems;
using Modules.Core.UI;

namespace Game.UI.GameHub.LevelsMenu.Views
{
    public sealed class LevelView : UITextButton, IDestroyEvent
    {
        public event Action Destroyed;
        
        public LevelCode LevelCode { get; private set; }

        public void Set(LevelCode levelCode) =>
            LevelCode = levelCode;
    }
}
