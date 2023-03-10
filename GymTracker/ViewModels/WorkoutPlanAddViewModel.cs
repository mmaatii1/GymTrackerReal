using CommunityToolkit.Mvvm.Input;
using GymTracker.Dtos;
using GymTracker.Services;

namespace GymTracker.ViewModels
{
    public partial class WorkoutPlanAddViewModel : BaseViewModel
    {
        IConnectivity _connectivity;
        IWrapperService<Exercise> _exerciseWrapper;
        IWrapperService<WorkoutPlanCreateDto> _workoutPlanWrapper;
        public WorkoutPlanAddViewModel(IConnectivity connectivity, IWrapperService<Exercise> exerciseWrapper, IWrapperService<WorkoutPlanCreateDto> workoutPlanWrapper)
        {
            _exerciseWrapper = exerciseWrapper;
            _connectivity = connectivity;
            _workoutPlanWrapper = workoutPlanWrapper;
        }

        public ObservableCollection<Exercise> ExerciseCollection = new();
        public ObservableCollection<Exercise> SelectedExercises { get; set; } = new();

        [ObservableProperty]
        bool isSearchResultVisible = false;

        [ObservableProperty]
        List<string> searchResults;


        [ObservableProperty]
        WorkoutPlanCreateDto newWorkoutPlan = new WorkoutPlanCreateDto();

        public void PerformSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                SearchResults = null;
                IsSearchResultVisible = false;
                return;
            }
            SearchResults = ExerciseCollection.Where(c => c.Name.ToLower()
            .Contains(query.ToLower()))
                .Select(c => c.Name)
                .ToList();
            IsSearchResultVisible = true;
        }


        [RelayCommand]
        void OnPicked(SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as string;
            var selectedItemAsObject = ExerciseCollection.Where(c => c.Name.Equals(selectedItem)).FirstOrDefault();
            SelectedExercises.Add(selectedItemAsObject);
            e = null;
            IsSearchResultVisible = false;
        }

        public void OnWorkoutPlanNameInfoEntry(string text)
        {
            NewWorkoutPlan.Name = text;
        }

        [RelayCommand]
        async Task ConfirmPlan()
        {
            NewWorkoutPlan.ExercisesIds = SelectedExercises.Select(c => c.Id);

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

                await _workoutPlanWrapper.SaveAsync(NewWorkoutPlan, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get exercises: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        async Task GetExercises()
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
                var exercises = await _exerciseWrapper.GetAllAsync();

                if (ExerciseCollection.Count != 0)
                {
                    ExerciseCollection.Clear();
                }

                foreach (var exercise in exercises)
                {
                    ExerciseCollection.Add(exercise);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get exercises: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
