using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ProjectSpace.Services.Validation;

internal sealed class ValidationResult
{
    public static readonly ValidationResult Valid = new(null);
    private ValidationResult(Dictionary<string, string[]>? errors)
    {
        if (errors is not null)
        {
            Errors = new(errors);
        }
    }

    [MemberNotNullWhen(false, nameof(Errors))]
    public bool Success => Errors is null;
    public ReadOnlyDictionary<string, string[]>? Errors { get; }

    public static ValidationResult FromErrors<T>(IEnumerable<KeyValuePair<string, T>> errors) 
        where T : IEnumerable<string> 
    {
        Dictionary<string, string[]> errorDic = errors
                .GroupBy(e => e.Key)
                .ToDictionary(k => k.Key, v => v.SelectMany(x => x.Value).ToArray());

        return new(errorDic);
    }
}
