
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
        [ObservableProperty]
        ImageSource workoutPhoto;
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
        [RelayCommand]
        void SetImage()
        {
            WorkoutPhoto = ByteArrayToImageSource();
        }
        private ImageSource ByteArrayToImageSource()
        {
            var byteArray = CustomWorkout.PhotoAsBytes;
            Stream stream = new MemoryStream(byteArray);
            return ImageSource.FromStream(() => stream);
        }
    }
}
