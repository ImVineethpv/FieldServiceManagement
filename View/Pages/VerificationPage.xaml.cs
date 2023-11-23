namespace FSM.View.Pages;

[QueryProperty("LastThreeDigits", "data")]
public partial class VerificationPage : ContentPage
{
    private string _lastThreeDigits;
    private TimeSpan timerDuration = TimeSpan.FromMinutes(2);
    private DateTime timerStartTime;
    private bool isTimerRunning = false;
    public VerificationPage()
	{
		InitializeComponent();
        StartTimer();
    }
    public string LastThreeDigits
    {
        get => _lastThreeDigits;
        set
        {
            _lastThreeDigits = Uri.UnescapeDataString(value);
            ProcessData();
        }
    }

    private void ProcessData()
    {
        lblMessage.Text = $"your (xxx)-xxxx-{_lastThreeDigits}";
    }
    private void StartTimer()
    {
        if (!isTimerRunning)
        {
            timerStartTime = DateTime.Now;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateTimer();
                return isTimerRunning;
            });

            isTimerRunning = true;
        }
    }

    private void UpdateTimer()
    {
        TimeSpan elapsed = DateTime.Now - timerStartTime;
        TimeSpan remaining = timerDuration - elapsed;

        if (remaining.TotalSeconds <= 0)
        {
            StopTimer();
            lblResentCode.IsEnabled = true; 
        }
        else
        {
            Timer.Text = remaining.ToString(@"mm\:ss");
        }
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        
        lblResentCode.IsEnabled = false; 
        StartTimer(); 
    }
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue) && !IsNumeric(e.NewTextValue))
        {
            ((Entry)sender).Text = e.OldTextValue;
        }
        if (!string.IsNullOrEmpty(value1.Text) && value1.Text.Length == 4)
        {
            value2.Text = value1.Text.Substring(1, 1);
            value3.Text = value1.Text.Substring(2, 1);
            value4.Text = value1.Text.Substring(3, 1);
            value4.Focus();
        }
        if (!string.IsNullOrEmpty(value1.Text) && value1.Text.Length == 1)
        {
            value2.Focus();
        }
        if (!string.IsNullOrEmpty(value2.Text) && value1.Text.Length == 1)
        {
            value3.Focus();
        }
        if (!string.IsNullOrEmpty(value3.Text) && value1.Text.Length == 1)
        {
            value4.Focus();
        }
        if (!string.IsNullOrEmpty(value1.Text) && !string.IsNullOrEmpty(value2.Text) && !string.IsNullOrEmpty(value3.Text) && !string.IsNullOrEmpty(value4.Text))
            btnVerify.IsVisible= true;
    }

    private bool IsNumeric(string text)
    {
        return int.TryParse(text, out _);
    }
    private async void OnVerify_Pressed(object sender, EventArgs e)
    {
        try
        {
            string enteredValue = value1.Text + value2.Text + value3.Text + value4.Text;

            if (enteredValue.Length >= 4)
            {
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}"); 
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error, Navigation error!", ex.Message, "OK");
        }
    }

}