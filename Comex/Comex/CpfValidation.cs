namespace Comex;

internal static class CpfValidation
{
    public static bool TryNormalize(string? input, out string cpf)
    {
        cpf = string.Empty;
        if (input is null)
        {
            return false;
        }

        string candidate = input.Trim();
        if (candidate.Length != 11)
        {
            return false;
        }

        foreach (char character in candidate)
        {
            if (character is < '0' or > '9')
            {
                return false;
            }
        }

        cpf = candidate;
        return true;
    }
}
