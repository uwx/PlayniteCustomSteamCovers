using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Playnite.SDK;
using Playnite.SDK.Plugins;

namespace PlayniteCustomSteamCovers;

public class CustomSteamCoversMetadataPlugin : MetadataPlugin
{
    private static readonly ILogger Logger = LogManager.GetLogger();
        
    private CustomSteamCoversMetadataSettings _settings;

    public CustomSteamCoversMetadataPlugin(IPlayniteAPI playniteApi) : base(playniteApi)
    {
        _settings = CreateSettingsIfNotExists();
    }

    public override Guid Id { get; } = Guid.Parse("382f576f-dd0d-4dd3-9cca-d3ac11a6f3ce");
    public override string Name => "Custom Steam Covers";

    public override List<MetadataField> SupportedFields { get; } = new List<MetadataField>
    {
        MetadataField.CoverImage
    };

    private CustomSteamCoversMetadataSettings CreateSettingsIfNotExists()
    {
        var settings = LoadPluginSettings<CustomSteamCoversMetadataSettings>();
        if (settings == null)
        {
            settings = new CustomSteamCoversMetadataSettings();
            SavePluginSettings(settings);
        }

        return settings;
    }

    public override OnDemandMetadataProvider GetMetadataProvider(MetadataRequestOptions options)
    {
        return new CustomSteamCoversMetadataProvider(options, this);
    }

    public override ISettings GetSettings(bool firstRunSettings)
    {
        return new CustomSteamCoversMetadataSettings(this);
    }

    public override UserControl GetSettingsView(bool firstRunView)
    {
        return new CustomSteamCoversSettingsView();
    }
}