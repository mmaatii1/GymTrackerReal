namespace GymTracker.Views;

public partial class AddTraining : ContentPage
{
    CustomWorkoutAddViewModel _viewModel;
    public AddTraining(CustomWorkoutAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel= viewModel;
        var e = new Exercise() { Name = "adsa" };
        var ex = new List<Exercise>() { e };
        ExPicker.ItemsSource = (System.Collections.IList)(ex as IList<Exercise>);

    }

    void OnEntryCompleted(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        _viewModel.OnWeightEntry(text);
    }
    void OnEntryCompleted2(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        _viewModel.OnSetsEntry(text);
    }
    void OnEntryCompleted3(object sender, EventArgs e)
    {
        string text = ((Entry)sender).Text;
        _viewModel.OnRepEntry(text);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            _viewModel.SelectedExercise(selectedIndex);
        }
    }
}