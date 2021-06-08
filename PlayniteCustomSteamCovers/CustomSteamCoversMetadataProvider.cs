using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Playnite.SDK;
using Playnite.SDK.Metadata;
using Playnite.SDK.Plugins;

namespace PlayniteCustomSteamCovers
{
    public class CustomSteamCoversMetadataProvider : OnDemandMetadataProvider
    {
        // https://github.com/JosefNemec/Playnite/blob/928e47926528f2f1c5555aebcad35b6ce2579ca9/source/Plugins/SteamLibrary/SteamLibrary.cs#L677
        private static readonly Guid SteamPluginId = Guid.Parse("CB91DFC9-B977-43BF-8E70-55F46E410FAB");
        private static readonly ILogger Logger = LogManager.GetLogger();
        
        private readonly MetadataRequestOptions _options;
        private readonly IPlayniteAPI _playniteApi;
        private readonly CustomSteamCoversMetadataSettings _settings;
        private readonly CustomSteamCoversMetadataPlugin _plugin;
        private readonly string _pluginUserDataPath;
        private readonly XmlDocument _wiiDb;
        private readonly byte[] _gamelist;

        private List<MetadataField> _availableFields;

        private string _coverPath;

        public CustomSteamCoversMetadataProvider(MetadataRequestOptions options, CustomSteamCoversMetadataPlugin plugin)
        {
            _options = options;
            _playniteApi = plugin.PlayniteApi;
            _plugin = plugin;
            _pluginUserDataPath = plugin.GetPluginUserDataPath(); 
            _settings = plugin.LoadPluginSettings<CustomSteamCoversMetadataSettings>();
            //DolphinMetadataSettings.MigrateSettingsVersion(_settings, plugin);
        }

        public override List<MetadataField> AvailableFields
        {
            get
            {
                if (_availableFields == null) _availableFields = GetAvailableFields();
                return _availableFields;
            }
        }

        private List<MetadataField> GetAvailableFields()
        {
            if (_coverPath == null)
                if (!LoadGameData())
                    return new List<MetadataField>();

            if (_coverPath == null)
                return new List<MetadataField>();

            return new List<MetadataField>(new[] { MetadataField.CoverImage });
        }

        private bool LoadGameData()
        {
            if (_coverPath != null) return true;

            if (_options.GameData.PluginId != SteamPluginId)
            {
                return false;
            }

            var gridPath = Path.Combine(_settings.PathToSteamUserdata, "config", "grid");
            if (!Directory.Exists(gridPath))
            {
                return false;
            }

            var filePath = Path.Combine(gridPath, $"{_options.GameData.GameId}p");
            string[] availableExtensions =
            {
                ".png", ".jpg"
            };

            foreach (var extension in availableExtensions)
            {
                if (File.Exists(filePath + extension))
                {
                    _coverPath = filePath + extension;
                    return true;
                }
            }

            return false;
        }

        public override MetadataFile GetCoverImage()
        {
            if (AvailableFields.Contains(MetadataField.CoverImage) && _coverPath != null)
            {
                try
                {
                    var data = File.ReadAllBytes(_coverPath);
                    return new MetadataFile(Path.GetFileName(_coverPath), data);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not load cover image!");
                }
            }

            return base.GetCoverImage();
        }
    }
}