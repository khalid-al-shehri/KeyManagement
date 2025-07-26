using System;
using Application.Common.Interface;

namespace Infrastructure.Service;

public class StringServices : IStringServices
{
    public string GetUsernameFromEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        string[] parts = email.Split('@');
        return parts.Length > 0 ? parts[0] : string.Empty;
    }
}
