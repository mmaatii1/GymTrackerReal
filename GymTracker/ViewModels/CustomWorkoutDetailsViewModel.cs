
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
        CustomWorkout customWorkout;

        [RelayCommand]
        async Task DeleteWorkoutAsync()
        {
            try
            {
               await _customWorkoutWrapper.DeleteAsync(CustomWorkout);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
