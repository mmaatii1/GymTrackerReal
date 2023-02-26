using GymTracker.Services;
using GymTracker.Views;


namespace GymTracker.ViewModels
{
    public partial class CustomWorkoutViewModel : BaseViewModel
    {
        public ObservableCollection<CustomWorkout> Workouts { get; } = new ObservableCollection<CustomWorkout>();
        IWorkoutService _workoutService;
        IConnectivity _connectivity;
        public CustomWorkoutViewModel(IWorkoutService workoutService, IConnectivity connectivity)
        {
            Title = "WorkoutList";
            _workoutService = workoutService;
            _connectivity = connectivity;
        }
        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetWorkoutsAsync()
        {
            if(IsBusy) return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }

                IsBusy = true;
                var workouts = await _workoutService.GetWorkoutAsync();

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
        [RelayCommand]
        async Task GoToDetails(CustomWorkout workout)
        {
            if (workout == null)
                return;
            var navigationParameter = new Dictionary<string, object>
            {
                { "CustomWorkout", workout }
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), navigationParameter);
        }
    }
}
