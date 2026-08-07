using CeibaTestEventos.Domain.Common;
using System.Text.RegularExpressions;

namespace CeibaTestEventos.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                "El correo electrónico no puede estar vacío.");
        }

        value = value.Trim();

        if (!IsValid(value))
        {
            throw new DomainException(
                "El formato del correo electrónico no es válido.");
        }

        return new Email(value.ToLowerInvariant());
    }

    private static bool IsValid(string value)
    {
        const string pattern =
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        return Regex.IsMatch(
            value,
            pattern,
            RegexOptions.CultureInvariant);
    }

    public bool Equals(Email? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email email &&
               Equals(email);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(
        Email? left,
        Email? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(
        Email? left,
        Email? right)
    {
        return !(left == right);
    }
}