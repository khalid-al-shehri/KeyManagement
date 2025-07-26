using System;

namespace Application.Common.Shared;

public record AppSettings
{
    public AppSettings()
    {
        AllowedCrossOrigin = [];
    }

    public string[] AllowedCrossOrigin { get; set; }
    public APISecuritySettings APISecuritySettings { get; set; }
    public AccountLockingSettings AccountLockingSettings { get; set; }
    public FileSettings FileSettings { get; set; }
    public GeneralSettings GeneralSettings { get; set; }
    public SMSSettings SMSSettings { get;set; }
    public EmailSettings EmailSettings { get; set; }
    public InternalSSOSettings InternalSSOSettings { get; set; }
    public OTPSettings OTPSettings { get; set; }

}

public record APISecuritySettings
{
    public string Key { get; set; } = null!;
    public int Expiration { get; set; }
    public int ExpirationInvitation { get; set; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Authority { get; set; } = null!;
}

public record FileSettings
{
    public FileSettings()
    {
        AllowedFiles = [];
    }

    public int MaximumSize { get; set; }
    public AllowedExtensions[] AllowedFiles { get; set; }
}

public record AllowedExtensions
{
    public string ContentType { get; set; }
    public string Extension { get; set; }
}

public record AccountLockingSettings
{
    public int AccountLockoutAttempts { get; set; }
    public int AccountLockoutDuration { get; set; }
}

public record GeneralSettings
{
    public int AgeLimit { get; set; }
}

public record EmailSettings
{
    public bool Enabled { get; set; }
    public string API { get; set; }
}

public record SMSSettings
{
    public bool Enabled { get; set; }
    public string API { get; set; }
}

public record OTPSettings
{
    public bool Enabled { get; set; }
    public string MockOtpValue { get; set; } = null!;
    public string OtpSecretKey { get; set; } = null!;
    public int OtpValidationDuration { get; set; }
    public int Size { get; set; }
}

public record InternalSSOSettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrantType { get; set; }
    public string Scope { get; set; }
    public string API { get; set;}
}