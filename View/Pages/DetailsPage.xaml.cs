namespace FSM.View.Pages;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(IssueDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}