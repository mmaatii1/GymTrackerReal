using CommunityToolkit.Mvvm.ComponentModel;
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
        SpecificExercise specificExerciseInEdit;

        [ObservableProperty]
        List<string> searchResults;


        [ObservableProperty]
        List<SpecificExercise> doneExercises;


        [ObservableProperty]
        List<Exercise> selectedExercises;

        [ObservableProperty]
        bool isSearchResultVisible = false;
        string EnteredExerciseInput { get; set; } = "";

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
            SpecificExerciseInEdit ??= new SpecificExercise();
            SpecificExerciseInEdit.Exercise = selectedItemAsObject;
            SelectedExercises ??= new List<Exercise>();
            SelectedExercises.Add(selectedItemAsObject);
            e = null;
            IsSearchResultVisible = false;
        }

        [RelayCommand]
        void ExerciseConfirm()
        {
            DoneExercises ??= new List<SpecificExercise>();
            if(SpecificExerciseInEdit is null)
            {
                return;
            }
            DoneExercises.Add(SpecificExerciseInEdit);
            SpecificExerciseInEdit = null;
            SelectedExercises = null;
        }

        public void OnExerciseInfoEntry(string e)
        {
            if (e.Equals(EnteredExerciseInput))
            {
                return;
            }
            var textFromInput = e;
            var splitted = textFromInput.Split("-");
            var weight = splitted[0];
            var splittedReps = splitted.TakeLast(splitted.Count() - 1);
            var numberOfSets = splittedReps.Count();
            SpecificExerciseInEdit.Weight = double.Parse(weight);
            SpecificExerciseInEdit.Sets = (byte)numberOfSets;
            var splittedRepsAsDoubles = splittedReps.Select(c=> {
                double parsed;
                if(double.TryParse(c, out parsed))
                {
                    return parsed;
                }
                return 0;
                
            }).ToArray();
            SpecificExerciseInEdit.Repetitions = splittedRepsAsDoubles;
            EnteredExerciseInput = e;
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
