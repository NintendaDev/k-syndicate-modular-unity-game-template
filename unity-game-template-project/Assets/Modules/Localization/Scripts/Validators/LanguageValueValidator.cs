using System;

#if UNITY_EDITOR
using Modules.Localization.Types;
using Sirenix.OdinInspector.Editor.Validation;
#endif

#if UNITY_EDITOR
[assembly: RegisterValidator(typeof(languageValueValidator))]

public class languageValueValidator : AttributeValidator<IsNotNoneLanguageAttribute, Language>
{
    protected override void Validate(ValidationResult result)
    {
        if (this.Attribute.NoneLanguage == this.Value)
            result.AddError($"Language cannot be None");
    }
}
#endif

public class IsNotNoneLanguageAttribute : Attribute
{
    public Language NoneLanguage { get; } = Language.None;
}