using AssetManagement.Domain.Common;
using System.Text.RegularExpressions;

namespace AssetManagement.Domain.ValueObjects;

public partial class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        if (!MyRegex().IsMatch(email))
            throw new ArgumentException("Invalid email format.");

        return new Email(email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex MyRegex();
}
