namespace FSM.ViewModel;

[QueryProperty(nameof(Issue), "Issue")]
public partial class IssueDetailsViewModel : BaseViewModel
{
    IMap map;
    public IssueDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [ObservableProperty]
    IssueModel issue;

    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
            await map.OpenAsync(37.430078, -122.125186, new MapLaunchOptions
            {
                Name = Issue.Assignee,
                NavigationMode = NavigationMode.Driving
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
