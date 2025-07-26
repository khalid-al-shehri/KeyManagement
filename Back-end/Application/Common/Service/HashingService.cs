using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Application.Common.Interface;

namespace Application.Common.Service;

public class HashingService : IHashingService
{
    public string HashString(string str, string salt)
    {
        byte[] saltByte = Encoding.ASCII.GetBytes(salt);
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password :str,
        salt: saltByte,
        prf: KeyDerivationPrf.HMACSHA1,
        iterationCount: 10000,
        numBytesRequested: 256 / 8));
    }
}