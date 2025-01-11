using System;
using Modules.Localization.Core.Types;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.Validation;
#endif

#if UNITY_EDITOR
[assembly: RegisterValidator(typeof(LanguageValueValidator))]

public class LanguageValueValidator : AttributeValidator<IsNotNoneLanguageAttribute, Language>
{
    protected override void Validate(ValidationResult result)
    {
        if (Attribute.NoneLanguage == this.Value)
            result.AddError($"Language cannot be None");
    }
}
#endif

public class IsNotNoneLanguageAttribute : Attribute
{
    public Language NoneLanguage { get; } = Language.None;
}