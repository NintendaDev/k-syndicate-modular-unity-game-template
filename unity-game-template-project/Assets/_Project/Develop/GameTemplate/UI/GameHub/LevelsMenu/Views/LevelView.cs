using GameTemplate.Infrastructure.Levels;
using GameTemplate.UI.Core.Buttons;

namespace GameTemplate.UI.GameHub.LevelsMenu.Views
{
    public class LevelView : UITextButton
    {
        public LevelCode LevelCode { get; private set; }

        public void Set(LevelCode levelCode) =>
            LevelCode = levelCode;
    }
}
