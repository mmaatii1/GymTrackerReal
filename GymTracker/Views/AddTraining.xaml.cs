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
        var entry = (Entry)sender;
        var exercise = (Exercise)entry.BindingContext; 
        string newText = e.NewTextValue;
        _viewModel.OnExerciseInfoEntry(newText,exercise);
    }
    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        var entry = (Entry)sender;
        var exercise = (Exercise)entry.BindingContext;
        _viewModel.OnExerciseInfoEntry(text,exercise);
    }
}
