
using GymTracker.Services;

namespace GymTracker.ViewModels
{
    [QueryProperty(nameof(CustomWorkout), "CustomWorkout")]
    public partial class CustomWorkoutDetailsViewModel : BaseViewModel
    {
        readonly IWrapperService<CustomWorkout> _customWorkoutWrapper;
        public CustomWorkoutDetailsViewModel(IWrapperService<CustomWorkout> wrapper)
        {
            _customWorkoutWrapper = wrapper;
        }

        [ObservableProperty]
        CustomWorkout customWorkoutt;

        [RelayCommand]
        async Task DeleteWorkoutAsync()
        {
            try
            {
               await _customWorkoutWrapper.DeleteAsync(CustomWorkoutt);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
