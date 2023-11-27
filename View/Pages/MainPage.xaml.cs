namespace FSM.View.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(IssuesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IssuesViewModel viewModel)
        {
            // Call the GetIssuesAsync method when the page appears
            await viewModel.GetIssuesAsync();
        }
    }
}

