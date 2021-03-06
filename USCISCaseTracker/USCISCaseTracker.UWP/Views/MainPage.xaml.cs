﻿using System;
using USCISCaseTracker.Models;
using USCISCaseTracker.Repositories;
using USCISCaseTracker.Services;
using USCISCaseTracker.UWP.Shared.Services;
using USCISCaseTracker.UWP.ViewModels;
using USCISCaseTracker.UWP.ViewModels.CaseViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace USCISCaseTracker.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new MainPageViewModel();
            DataContext = ViewModel;
            CasesListView.ItemsSource = ViewModel.Cases;

            SplitViewContentFrame.Navigate(typeof(DashboardPage));
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
            var dlg = new CaseAddDialog();
            var result = await dlg.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var caseViewModel = (CaseAddViewModel)dlg.DataContext;

                var valid = caseViewModel.Errors.ValidateProperties();

                if (!valid)
                    return; // no valid model

                CaseRepository repo = new CaseRepository(LocalDbConnectionService.Connect());

                var newCase = new Case()
                {
                    Name = caseViewModel.Name,
                    ReceiptNumber = caseViewModel.ReceiptNumber
                };
                newCase.Id = repo.Save(newCase);

                if (newCase.Id != 0)
                {
                    ViewModel.LoadLocalCases();
                    CasesListView.ItemsSource = ViewModel.Cases;
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
                await c.Update();

                var repo = new CaseRepository(LocalDbConnectionService.Connect());
                int success = repo.Save(c);

                if (success != 0)
                {
                    // update success
                    System.Diagnostics.Debug.WriteLine($"{c.Name} ::: {c.ReceiptNumber} ::: {c.Status} ::: {c.Description}");
                }
            }

            ViewModel.LoadLocalCases();
            CasesListView.ItemsSource = ViewModel.Cases;
        }

        private void CasesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = (Case)CasesListView.SelectedItem;

            if (c != null)
            {
                SplitViewContentFrame.Navigate(typeof(CaseDetailsPage), c);
                DeleteAppBarButton.Visibility = Visibility.Visible;

                var repo = new CaseRepository(LocalDbConnectionService.Connect());
                repo.SaveLastReadTime(c);


                var index = CasesListView.SelectedIndex;
                CasesListView.SelectionChanged -= CasesListView_SelectionChanged;

                ViewModel.LoadLocalCases();
                CasesListView.ItemsSource = ViewModel.Cases;

                CasesListView.SelectedIndex = index;
                CasesListView.SelectionChanged += CasesListView_SelectionChanged;

            }
            else
            {
                SplitViewContentFrame.Navigate(typeof(DashboardPage));
                DeleteAppBarButton.Visibility = Visibility.Collapsed;

            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            CasesListView.SelectedIndex = -1;
            SplitViewContentFrame.Navigate(typeof(DashboardPage));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            CasesListView.SelectedIndex = -1;
            SplitViewContentFrame.Navigate(typeof(SettingsPage));
        }
    }
}