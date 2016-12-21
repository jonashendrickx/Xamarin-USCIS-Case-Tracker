using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using USCISCaseTracker.Models;
using USCISCaseTracker.Repositories;
using USCISCaseTracker.Services;
using USCISCaseTracker.UWP.Services;
using USCISCaseTracker.UWP.ViewModels;
using USCISCaseTracker.UWP.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace USCISCaseTracker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainPageViewModel();
            DataContext = ViewModel;
            CasesListView.ItemsSource = ViewModel.Cases;

            SplitViewContentFrame.Navigate(typeof(HomePage));
        }

        public MainPageViewModel ViewModel
        {
            get; set;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MasterDetailView.IsPaneOpen = !MasterDetailView.IsPaneOpen;
        }

        private async void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddCaseDialog();
            var result = await dlg.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {

                var newCase = (Case)dlg.DataContext;
                CaseRepository repo = new CaseRepository(LocalDbConnectionService.Connect());
                newCase.Id = repo.Save(newCase);

                if (newCase.Id != 0)
                {
                    ViewModel.Cases.Add(newCase);
                }
            }
        }

        private void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCase = (Case)CasesListView.SelectedItem;

            if (selectedCase != null)
            {
                var repo = new CaseRepository(LocalDbConnectionService.Connect());
                var rows = repo.Delete(selectedCase.Id);

                if (rows > 0)
                {
                    ViewModel.Cases.Remove(selectedCase);
                }
            }
        }

        private async void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var c in ViewModel.Cases)
            {
                var uscisSvc = new USCISService();
                var onlineCase = await uscisSvc.GetCaseStatusAsync(c.ReceiptNumber);
                c.Status = onlineCase.Status;
                c.Description = onlineCase.Description;
                c.LastSyncedDate = onlineCase.LastSyncedDate;

                var repo = new CaseRepository(LocalDbConnectionService.Connect());
                var rows = repo.Save(c);

                if (rows > 0)
                {
                    // update success
                    System.Diagnostics.Debug.WriteLine(string.Format("{0} ::: {1} ::: {2} ::: {3}", c.Name, c.ReceiptNumber, c.Status, c.Description));
                }
            }
        }

        private void CasesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = (Case) CasesListView.SelectedItem;

            if (c != null)
            {
                SplitViewContentFrame.Navigate(typeof(CaseDetailPage), c);
                DeleteAppBarButton.Visibility = Visibility.Visible;

            }
            else
            {
                SplitViewContentFrame.Navigate(typeof(HomePage));
                DeleteAppBarButton.Visibility = Visibility.Collapsed;

            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            CasesListView.SelectedIndex = -1;
            SplitViewContentFrame.Navigate(typeof(HomePage));
        }
    }
}
