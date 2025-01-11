using Modules.Localization.Core.Types;

namespace Modules.Localization.Core.Systems
{
    public interface ITranslation
    {
        public string MakeTranslatedTextByTerm(string term);

        public string MakeTranslatedText(LocalizationTerm term);
    }
}