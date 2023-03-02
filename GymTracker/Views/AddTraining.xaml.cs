namespace GymTracker.Views;

public partial class AddTraining : ContentPage
{
    CustomWorkoutAddViewModel _viewModel;
    public AddTraining(CustomWorkoutAddViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

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
        _viewModel.OnExerciseInfoEntry(newText);
    }
    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        _viewModel.OnExerciseInfoEntry(text);
    }
}
