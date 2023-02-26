namespace GymTracker.Views;

public partial class MainPage : ContentPage
{
	public MainPage(CustomWorkoutViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
	}
}