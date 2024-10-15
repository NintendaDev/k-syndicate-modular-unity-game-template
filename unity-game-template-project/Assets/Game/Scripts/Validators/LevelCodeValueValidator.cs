using System;
using GameTemplate.Infrastructure.Levels;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.Validation;
#endif

#if UNITY_EDITOR
[assembly: RegisterValidator(typeof(LevelCodeValueValidator))]

public class LevelCodeValueValidator : AttributeValidator<IsNotNoneLevelCodeAttribute, LevelCode>
{
    protected override void Validate(ValidationResult result)
    {
        if (this.Attribute.NoneLevelCode == this.Value)
            result.AddError($"LevelCode type cannot be None");
    }
}
#endif

public class IsNotNoneLevelCodeAttribute : Attribute
{
    public LevelCode NoneLevelCode { get; } = LevelCode.None;
}