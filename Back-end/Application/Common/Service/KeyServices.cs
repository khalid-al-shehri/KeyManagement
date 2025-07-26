using System;
using System.Security.Cryptography;
using Application.Common.Interface;

namespace Application.Common.Service.KeyServices;

public class KeyServices : IKeyServices
{
    public string GenerateKey()
    {
        const string prefix = "key_khalid_";
        const int byteLength = 32;

        var randomBytes = new byte[byteLength];
        RandomNumberGenerator.Fill(randomBytes);

        // Convert bytes to a URL safe string
        string randomString = Convert.ToBase64String(randomBytes)
                                     .Replace("+", "-")
                                     .Replace("/", "_")
                                     .TrimEnd('=');

        return $"{prefix}{randomString}";
    }

}