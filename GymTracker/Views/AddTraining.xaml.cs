namespace GymTracker.Views;

public partial class AddTraining : ContentPage
{
	public AddTraining(CustomWorkoutAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}