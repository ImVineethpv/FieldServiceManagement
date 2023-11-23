namespace FSM.View.Pages;

public partial class OnboardingPage : ContentPage
{

    private readonly OnboardingViewModel _onboardingVm;
    public OnboardingPage()
    {
        InitializeComponent();
        _onboardingVm = new OnboardingViewModel();
        BindingContext = _onboardingVm;

    }
    private async void OnGetStartedClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");

    }

    private void carouselView_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        _onboardingVm.IsLastStep = e.CurrentPosition == (_onboardingVm.OnboardingSteps.Count - 1);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        await Navigation.PushAsync(new LoginPage());

    }

    private async void OnGetStarted_Pressed(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation error: {ex.Message}");
        }

    }

    }