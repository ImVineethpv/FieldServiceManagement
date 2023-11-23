namespace FSM.View.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnVerify_Pressed(object sender, EventArgs e)
    {
        string mobileNumber = txtMobilenumber.Text;
        if (mobileNumber.Length >= 3)
        {
            string lastThreeDigits = mobileNumber.Substring(mobileNumber.Length - 3);
            await Shell.Current.GoToAsync($"{nameof(VerificationPage)}?data={lastThreeDigits}");
        }
    }
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue) && !IsNumeric(e.NewTextValue))
        {
            ((Entry)sender).Text = e.OldTextValue;
        }       
    }

    private bool IsNumeric(string text)
    {
        return int.TryParse(text, out _);
    }
}