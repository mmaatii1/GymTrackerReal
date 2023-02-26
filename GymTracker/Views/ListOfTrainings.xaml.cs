namespace GymTracker.Views;

public partial class ListOfTrainings : ContentPage
{
	public ListOfTrainings(CustomWorkoutViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}