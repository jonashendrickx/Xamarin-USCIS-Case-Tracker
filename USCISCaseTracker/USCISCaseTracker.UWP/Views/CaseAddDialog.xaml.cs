using System;
using System.ComponentModel;
using USCISCaseTracker.UWP.ViewModels.CaseViewModels;
using Windows.UI.Xaml.Controls;

namespace USCISCaseTracker.UWP.Views
{
    public sealed partial class CaseAddDialog : ContentDialog
    {
        public CaseAddDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
