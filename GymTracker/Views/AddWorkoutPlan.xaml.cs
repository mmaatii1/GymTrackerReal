namespace GymTracker.Views;

public partial class AddWorkoutPlan : ContentPage
{
    WorkoutPlanAddViewModel _viewModel;
    public AddWorkoutPlan(WorkoutPlanAddViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        var text = searchBar.Text;
        _viewModel.PerformSearch(text);
    }
}