﻿using FSM.Services;
using FSM.View.Pages;

namespace FSM.ViewModel;

public partial class IssuesViewModel : BaseViewModel
{
    public ObservableCollection<IssueModel> Issues { get; } = new();
    IssueService issueService;
    IConnectivity connectivity;
    IGeolocation geolocation;
    public IssuesViewModel(IssueService issueService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Issues";
        this.issueService = issueService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetIssuesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            IsBusy = true;
            var issues = await issueService.GetIssues();

            if(Issues.Count != 0)
                Issues.Clear();
                
            foreach(var issue in issues)
                Issues.Add(issue);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get Issues: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }
    
    [RelayCommand]
    async Task GoToDetails(IssueModel issue)
    {
        if (issue == null)
        return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Issue", issue }
        });
    }

    [RelayCommand]
    async Task GetClosestIssue()
    {
        if (IsBusy || Issues.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            // Find closest issue to us
            //var first = Issues.OrderBy(m => location.CalculateDistance(
            //    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
            //    .FirstOrDefault();

            //await Shell.Current.DisplayAlert("", first.Name + " " +
            //    first.Location, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}
