using Application.Common.Interface;
using Microsoft.Extensions.Localization;
using Application.Resources;
using System.Reflection;
using Application.Common.ValueObject;

namespace Application.Common.Service;

public class LocalizationService : StringLocalizer<string>, ILocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory) : base(factory)
    {
        var type = typeof(SharedResources);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create(nameof(Localization), assemblyName.Name);
    }

    public string GetString(string key)
    {
        return _localizer[key];
    }

    public override LocalizedString this[string name]
    {
        get => _localizer[name];
    }
}
