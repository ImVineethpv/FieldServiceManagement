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
            //await map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            //{
            //    Name = Monkey.Name,
            //    NavigationMode = NavigationMode.None
            //});
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
