using CommunityToolkit.Mvvm.Input;
using GymTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.ViewModels
{
    public partial class CustomWorkoutAddViewModel : BaseViewModel
    {
        readonly IWrapperService<Exercise> _exerciesWrapper;
        readonly IWrapperService<SpecificExercise> _specificExerciseWrapper;
        IConnectivity _connectivity;

        public ObservableCollection<Exercise> ExerciseCollection = new();
        public ObservableCollection<SpecificExercise> SpecificExerciseCollection = new();
        public CustomWorkoutAddViewModel(IWrapperService<Exercise> wrapper, IWrapperService<SpecificExercise> specificExerciseWrapper, IConnectivity connectivity)
        {
            _exerciesWrapper = wrapper;
            _specificExerciseWrapper = specificExerciseWrapper;
            _connectivity = connectivity;
        }
        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        Exercise exercise;

        [ObservableProperty]
        SpecificExercise specificExercise;

        [ObservableProperty]
        List<string> searchResults;

        [ObservableProperty]
        List<Exercise> selectedExercises;

        [RelayCommand]
        void PerformSearch(string query)
        {
            SearchResults = ExerciseCollection.Where(c=>c.Name.Contains(query)).Select(c=>c.Name).ToList();
        }

        [RelayCommand]
        void OnPicked(SelectedItemChangedEventArgs e)
        {
            SelectedExercises ??=new List<Exercise> ();
            var selectedItem = e.SelectedItem as string;
            var selectedItemAsObject = ExerciseCollection.Where(c => c.Name.Equals(selectedItem)).FirstOrDefault();
            SelectedExercises.Add(selectedItemAsObject);
            e = null;
            SearchResults= null;
        }

        [RelayCommand]
        void OnWeightEntry(EventArgs e)
        {
            string text = e.ToString();
            SpecificExercise ??= new SpecificExercise();
            SpecificExercise.Exercise = SelectedExercises[0];
            SpecificExercise.Weight = int.Parse(text);
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
                var exercises = await _exerciesWrapper.GetAllAsync();

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
                IsRefreshing = false;
            }
        }

       

        
    }
}
