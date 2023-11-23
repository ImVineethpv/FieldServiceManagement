namespace FSM.View.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(IssuesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

