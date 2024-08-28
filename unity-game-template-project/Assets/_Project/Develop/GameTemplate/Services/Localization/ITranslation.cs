namespace GameTemplate.Services.Localization
{
    public interface ITranslation
    {
        public string MakeTranslatedTextByTerm(string term);

        public string MakeTranslatedText(LocalizationTerm term);
    }
}