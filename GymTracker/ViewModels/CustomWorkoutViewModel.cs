using Android.Graphics;
using GymTracker.Services;
using GymTracker.Views;


namespace GymTracker.ViewModels
{
    public partial class CustomWorkoutViewModel : BaseViewModel
    {
        public ObservableCollection<CustomWorkout> Workouts { get; } = new ObservableCollection<CustomWorkout>();
         readonly IWrapperService<CustomWorkout> _workoutService;
        IConnectivity _connectivity;
        public CustomWorkoutViewModel(IWrapperService<CustomWorkout> workoutService, IConnectivity connectivity)
        {
            Title = "WorkoutList";
            _workoutService = workoutService;
            _connectivity = connectivity;
        }
        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task ToCustomWorkout(CustomWorkout workout)
        {
            if (workout == null)
                return;
            var asString =  workout.CustomWorkoutSpecificExercises.Select(c => c.Repetitions.ToString());
            foreach (var ex in workout.CustomWorkoutSpecificExercises)
            {
                ex.RepetitionsAsStrings = ex.Repetitions.Select(c => c.ToString() + "-").ToArray();
                string v = ex.RepetitionsAsStrings.Last().Replace("-", "");
                ex.RepetitionsAsStrings[^1] = v;
            }
            var navigationParameter = new Dictionary<string, object>
            {
                { "CustomWorkout", workout }
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), navigationParameter);
        }
        public async void Set()
        {
            byte[] imageData = GetImageData(); // Your array of bytes containing the image data

            if (imageData != null && imageData.Length > 0)
            {
                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

                // Create a stream from the bitmap
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);

                    // Set the source of the Image control to the stream
                    MyImage.Source = ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
                }
            }
        }

        [RelayCommand]
        async Task GetWorkoutsAsync()
        {
            if (IsBusy) return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                    
                }

                IsBusy = true;
                var workouts = await _workoutService.GetAllAsync();

                if (Workouts.Count != 0)
                {
                    Workouts.Clear();
                }

                foreach (var workout in workouts)
                {
                    Workouts.Add(workout);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get workouts: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

    }
}
