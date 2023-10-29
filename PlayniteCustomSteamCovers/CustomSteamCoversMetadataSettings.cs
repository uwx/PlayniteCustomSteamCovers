using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Playnite.SDK;

namespace PlayniteCustomSteamCovers;

public class CustomSteamCoversMetadataSettings : ISettings, INotifyPropertyChanged
{
    public const int CurrentVersion = 1;

    private static readonly ILogger Logger = LogManager.GetLogger();

    #region Shared Properties
    private string pathToSteamUserdata = "";
    public string PathToSteamUserdata
    {
        get => pathToSteamUserdata;
        set => OnPropertySet(ref pathToSteamUserdata, value, nameof(PathToSteamUserdata), nameof(PathStatus));
    }

    [JsonIgnore]
    public string PathStatus
    {
        get
        {
            if (string.IsNullOrWhiteSpace(PathToSteamUserdata))
            {
                return "❌ Path is empty";
            }
            if (Directory.Exists(Path.Combine(PathToSteamUserdata, "config", "grid")))
            {
                return "✔️ Path is valid";
            }

            return "❌ Path does not point to Steam user folder, or it doesn't have any covers";
        }
    }
    #endregion

    private readonly CustomSteamCoversMetadataPlugin _plugin;

    public string BrowseForFolder()
    {
        return _plugin.PlayniteApi.Dialogs.SelectFolder();
    }

    // Constructor for deserialization
    public CustomSteamCoversMetadataSettings()
    {
    }

    public CustomSteamCoversMetadataSettings(CustomSteamCoversMetadataPlugin plugin) : this()
    {
        _plugin = plugin;
        var savedSettings = plugin.LoadPluginSettings<CustomSteamCoversMetadataSettings>();
        if (savedSettings == null) return;

        PathToSteamUserdata = savedSettings.PathToSteamUserdata;
        // X = savedSettings.X;
    }

    public void BeginEdit()
    {
    }

    public void EndEdit()
    {
        _plugin.SavePluginSettings(this);
    }

    public void CancelEdit()
    {
    }

    public bool VerifySettings(out List<string> errors)
    {
        errors = new List<string>();
        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // Create the OnPropertyChanged method to raise the event
    // The calling member's name will be used as the parameter.
    protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    protected void OnPropertySet<T>(ref T oldValue, T newValue, [CallerMemberName] string property = null)
    {
        if (oldValue == null || newValue == null || !newValue.Equals(oldValue))
        {
            oldValue = newValue;
            OnPropertyChanged(property);
        }
    }

    protected void OnPropertySet<T>(ref T oldValue, T newValue, params string[] additionalProperties)
    {
        if (oldValue == null || newValue == null || !newValue.Equals(oldValue))
        {
            oldValue = newValue;
            foreach (var property in additionalProperties)
            {
                OnPropertyChanged(property);
            }
        }
    }
}