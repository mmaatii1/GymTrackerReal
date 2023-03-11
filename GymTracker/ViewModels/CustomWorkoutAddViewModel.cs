﻿using AutoMapper;
using GymTracker.Dtos;
using GymTracker.Services;

namespace GymTracker.ViewModels
{
    public partial class CustomWorkoutAddViewModel : BaseViewModel
    {
        readonly IWrapperService<Exercise> _exerciesWrapper;
        readonly IWrapperService<SpecificExerciseUpdateCreateDto> _specificExerciseWrapper;
        readonly IWrapperService<CustomWorkoutCreateUpdateDto> _customWorkoutWrapper;
        readonly IWrapperService<WorkoutPlan> _workoutPlanWrapper;
        readonly IMapper _mapper;
        IConnectivity _connectivity;

        public ObservableCollection<Exercise> ExerciseCollection { get; set; } = new();
        public ObservableCollection<SpecificExercise> SpecificExerciseCollection = new();
        public ObservableCollection<WorkoutPlan> WorkoutPlansCollection { get; set; } = new();

        public CustomWorkoutAddViewModel(IWrapperService<Exercise> wrapper, IMapper mapper,
            IWrapperService<SpecificExerciseUpdateCreateDto> specificExerciseWrapper,
            IConnectivity connectivity, IWrapperService<CustomWorkoutCreateUpdateDto> customWorkoutWrapper, IWrapperService<WorkoutPlan> workoutPlanWrapper)
        {
            _exerciesWrapper = wrapper;
            _specificExerciseWrapper = specificExerciseWrapper;
            _connectivity = connectivity;
            _customWorkoutWrapper = customWorkoutWrapper;
            _mapper = mapper;
            _workoutPlanWrapper = workoutPlanWrapper;
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

        [ObservableProperty]
        WorkoutPlan workoutPlan;

        WorkoutPlan prevWorkoutPlan { get; set; }
        string EnteredExerciseInput { get; set; } = "";

        partial void OnWorkoutPlanChanged(WorkoutPlan value)
        {
            var exercises = value.Exercises.ToList();

            if(prevWorkoutPlan?.Id is not null)
            {
                SelectedExercises.Clear();
                foreach (var ex in exercises)
                {
                    SelectedExercises.Add(ex);
                }
            }
            else
            {
                SelectedExercises ??= new();
                foreach (var ex in exercises)
                {
                    SelectedExercises.Add(ex);
                }
            }
            prevWorkoutPlan = value;
        }

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
                var workoutPlans = await _workoutPlanWrapper.GetAllAsync();

                if (WorkoutPlansCollection.Count != 0)
                {
                    WorkoutPlansCollection.Clear();
                }

                if (ExerciseCollection.Count != 0)
                {
                    ExerciseCollection.Clear();
                }

                foreach (var exercise in exercises)
                {
                    ExerciseCollection.Add(exercise);
                }
                foreach (var plan in workoutPlans)
                {
                    WorkoutPlansCollection.Add(plan);
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
            var splittedRepsAsDoubles = splittedReps.Select(c => {
                double parsed;
                if (double.TryParse(c, out parsed))
                {
                    return parsed;
                }
                return 0;

            }).ToArray();
            var splittedRepsAsStrings = splittedReps.Select(c => c + " - ").ToList();
            if (splittedRepsAsStrings.Any())
            {
                splittedRepsAsStrings[splittedRepsAsStrings.Count - 1] = splittedRepsAsStrings.Last().Replace("-", "");
                SpecificExerciseInEdit.RepetitionsAsStrings = splittedRepsAsStrings.ToArray();
            }
            SpecificExerciseInEdit.RepetitionsAsStrings = splittedRepsAsStrings.ToArray();
            SpecificExerciseInEdit.Repetitions = splittedRepsAsDoubles;
            EnteredExerciseInput = e;
        }

        [RelayCommand]
        async Task PostWorkout()
        {
            var results = new List<int>();
            foreach (var specEx in DoneExercises)
            {
                var specExCreate = new SpecificExerciseUpdateCreateDto() { ExerciseId = specEx.Exercise.Id, Repetitions = specEx.Repetitions, Sets = specEx.Sets, Weight = specEx.Weight };
                var res = await _specificExerciseWrapper.SaveAsync(specExCreate, true);
                results.Add(res.Id);
            }
            var workout = new CustomWorkoutCreateUpdateDto() { DateOfWorkout= DateTime.Now, SpecificExercisesIds = results, Name="Dodane z apki"  };
            await _customWorkoutWrapper.SaveAsync(workout, true);
        }
    }
}
