namespace GymTracker.Views;

public partial class AddTraining : ContentPage
{
	public AddTraining(CustomWorkoutAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

   

    private void Entry_Completed(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
    }
}