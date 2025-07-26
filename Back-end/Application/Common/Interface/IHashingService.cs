using System;

namespace Application.Common.Interface;

public interface IHashingService
{
    string HashString(string str, string salt);
}
