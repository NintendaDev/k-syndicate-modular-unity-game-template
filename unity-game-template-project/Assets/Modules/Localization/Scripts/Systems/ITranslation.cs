using Modules.Localization.Types;

namespace Modules.Localization.Systems
{
    public interface ITranslation
    {
        public string MakeTranslatedTextByTerm(string term);

        public string MakeTranslatedText(LocalizationTerm term);
    }
}