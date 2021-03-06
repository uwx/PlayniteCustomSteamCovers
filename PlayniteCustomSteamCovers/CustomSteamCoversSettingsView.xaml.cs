using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Globalization;
using System.Windows;

namespace PlayniteCustomSteamCovers
{
    public partial class CustomSteamCoversSettingsView
    {
        public CustomSteamCoversSettingsView()
        {
            InitializeComponent();
        }

		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			var dataContext = (CustomSteamCoversMetadataSettings) DataContext;

			using (var dialog = new CommonOpenFileDialog())
			{
				dialog.IsFolderPicker = true;
				CommonFileDialogResult result = dialog.ShowDialog();
				if (result == CommonFileDialogResult.Ok)
                {
					dataContext.PathToSteamUserdata = dialog.FileName;
                }
			}
		}
	}
}