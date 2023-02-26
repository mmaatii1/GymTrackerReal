
namespace GymTracker.ViewModels
{
    [QueryProperty(nameof(CustomWorkout), "CustomWorkout")]
    public partial class CustomWorkoutDetailsViewModel : BaseViewModel
    {
        public CustomWorkoutDetailsViewModel()
        {
         
        }

        [ObservableProperty]
        CustomWorkout customWorkout;
    }
}
