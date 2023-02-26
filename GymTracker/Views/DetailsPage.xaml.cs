namespace GymTracker.Views;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(CustomWorkoutDetailsViewModel customWorkoutDetailsViewModel)
	{
		InitializeComponent();
		BindingContext= customWorkoutDetailsViewModel;
	}
}