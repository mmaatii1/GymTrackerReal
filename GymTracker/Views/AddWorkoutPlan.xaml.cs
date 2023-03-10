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
    private void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string newText = e.NewTextValue;
        _viewModel.OnWorkoutPlanNameInfoEntry(newText);
    }
    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        _viewModel.OnWorkoutPlanNameInfoEntry(text);
    }
}