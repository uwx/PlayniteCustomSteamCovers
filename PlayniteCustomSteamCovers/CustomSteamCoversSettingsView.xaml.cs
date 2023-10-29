using System.Windows;

namespace PlayniteCustomSteamCovers;

public partial class CustomSteamCoversSettingsView
{
	public CustomSteamCoversSettingsView()
	{
		InitializeComponent();
	}

	private void btnOpenFile_Click(object sender, RoutedEventArgs e)
	{
		var dataContext = (CustomSteamCoversMetadataSettings) DataContext;

		var browsedFolder = dataContext.BrowseForFolder();

		if (!string.IsNullOrWhiteSpace(browsedFolder))
		{
			dataContext.PathToSteamUserdata = browsedFolder;
		}
	}
}