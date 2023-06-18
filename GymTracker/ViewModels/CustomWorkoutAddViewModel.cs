using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using GymTracker.Dtos;
using GymTracker.Services;
using Microsoft.Maui.Storage;
using System.IO;

namespace GymTracker.ViewModels
{
    public partial class CustomWorkoutAddViewModel : BaseViewModel
    {
        readonly IWrapperService<Exercise> _exerciesWrapper;
        readonly IWrapperService<SpecificExerciseUpdateCreateDto> _specificExerciseWrapper;
        readonly IWrapperService<CustomWorkoutCreateUpdateDto> _customWorkoutWrapper;
        readonly IWrapperService<WorkoutPlan> _workoutPlanWrapper;
        readonly IWrapperService<WorkoutPlanUpdateDto> _workoutPlanUpdateDtoWrapper;
        readonly IWrapperService<WorkoutPhoto> _photoWrapper;
        readonly IMapper _mapper;
        IConnectivity _connectivity;

        public ObservableCollection<Exercise> ExerciseCollection { get; set; } = new();
        public ObservableCollection<SpecificExercise> SpecificExerciseCollection = new();
        public ObservableCollection<WorkoutPlan> WorkoutPlansCollection { get; set; } = new();
        public ObservableCollection<Exercise> SelectedExercises { get; set; }
        public ObservableCollection<SpecificExercise> DoneExercises { get; set; }
        public CustomWorkoutAddViewModel(IWrapperService<Exercise> wrapper, IMapper mapper,
            IWrapperService<SpecificExerciseUpdateCreateDto> specificExerciseWrapper,
            IConnectivity connectivity, IWrapperService<CustomWorkoutCreateUpdateDto> customWorkoutWrapper, 
            IWrapperService<WorkoutPlan> workoutPlanWrapper, IWrapperService<WorkoutPlanUpdateDto> workoutPlanUpdateDtoWrapper,
            IWrapperService<WorkoutPhoto> photoWrapper)
        {
            _exerciesWrapper = wrapper;
            _specificExerciseWrapper = specificExerciseWrapper;
            _connectivity = connectivity;
            _customWorkoutWrapper = customWorkoutWrapper;
            _mapper = mapper;
            _workoutPlanWrapper = workoutPlanWrapper;
            _workoutPlanUpdateDtoWrapper = workoutPlanUpdateDtoWrapper;
            _photoWrapper = photoWrapper;
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
        bool isSearchResultVisible = false;

        [ObservableProperty]
        WorkoutPlan chosedWorkoutPlan;

        WorkoutPlan prevWorkoutPlan { get; set; }
        string EnteredExerciseInput { get; set; } = "";

        Guid Guid { get; set; }

        partial void OnChosedWorkoutPlanChanged(WorkoutPlan value)
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
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedExercises)));
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
            SelectedExercises = new();
            SelectedExercises.Add(selectedItemAsObject);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedExercises)));
            e = null;
            IsSearchResultVisible = false;
        }

        [RelayCommand]
        void ExerciseConfirm()
        {
            DoneExercises ??= new();
            if(SpecificExerciseInEdit is null)
            {
                return;
            }
            DoneExercises.Add(SpecificExerciseInEdit);
            SelectedExercises.Remove(SpecificExerciseInEdit.Exercise);
            SpecificExerciseInEdit = null;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(DoneExercises)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedExercises)));
     
            Vibration.Vibrate(TimeSpan.FromSeconds(1));
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
        [RelayCommand]
        public async void TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                    byte[] fileBytes = null;
                    using (var stream = await photo.OpenReadAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            fileBytes = memoryStream.ToArray();
                        }
                    }
                    await SendToApi(fileBytes);
                }
            }
        }
        [RelayCommand]
        public async void ChoosePhoto()
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
                byte[] fileBytes = null;
                using (var stream = await photo.OpenReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                }
                await SendToApi(fileBytes);
            }
        }
        private async Task SendToApi(byte[] picture)
        {
           Guid = Guid.NewGuid();
           await _photoWrapper.SaveAsync(new WorkoutPhoto() { PhotoAsBytes = picture, Guid = Guid}, true, true);
        }
       
        public void OnExerciseInfoEntry(string e, Exercise exercise)
        {
            if (e.Equals(EnteredExerciseInput))
            {
                return;
            }
            SpecificExerciseInEdit ??= new();
            SpecificExerciseInEdit.Exercise = exercise;
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

    string locationString = null;
    try
    {
        // Try to get current location
        Location location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
        var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
        var placemark = placemarks?.FirstOrDefault();
        if (placemark != null)
        {
            locationString = $"Country: {placemark.CountryName}, City: {placemark.Locality}, Street: {placemark.Thoroughfare}";
        }
    }
    catch (FeatureNotSupportedException fnsEx)
    {
        // Handle not supported on device exception
        // Log the error or notify the user
    }
    catch (PermissionException pEx)
    {
        // Handle permission exception
        // Log the error or notify the user
    }
    catch (Exception ex)
    {
        // Unable to get location
        // Log the error or notify the user
    }

    var workout = new CustomWorkoutCreateUpdateDto() { DateOfWorkout= DateTime.Now, SpecificExercisesIds = results, Name="Dodane z apki", Guid = Guid, Location = locationString };

    var res2 = await _customWorkoutWrapper.SaveAsync(workout, true);
    if(ChosedWorkoutPlan is not null)
    {
        var workoutPlanToUpdateWith = new WorkoutPlanUpdateDto();
        var mapped = _mapper.Map<WorkoutPlanUpdateDto>(ChosedWorkoutPlan);
        mapped.ExercisesIds = ChosedWorkoutPlan.Exercises.Select(x => x.Id);
        mapped.DoneWorkoutsIds.Add(res2.Id);
        await _workoutPlanUpdateDtoWrapper.SaveAsync(mapped, false);
    }
}
        //async Task PostWorkout()
        //{
        //    var results = new List<int>();
        //    foreach (var specEx in DoneExercises)
        //    {
        //        var specExCreate = new SpecificExerciseUpdateCreateDto() { ExerciseId = specEx.Exercise.Id, Repetitions = specEx.Repetitions, Sets = specEx.Sets, Weight = specEx.Weight };
        //        var res = await _specificExerciseWrapper.SaveAsync(specExCreate, true);
        //        results.Add(res.Id);
        //    }
        //    var workout = new CustomWorkoutCreateUpdateDto() { DateOfWorkout= DateTime.Now, SpecificExercisesIds = results, Name="Dodane z apki", Guid = Guid };
        //    var res2 = await _customWorkoutWrapper.SaveAsync(workout, true);
        //    if(ChosedWorkoutPlan is not null)
        //    {
        //        var workoutPlanToUpdateWith = new WorkoutPlanUpdateDto();
        //        var mapped = _mapper.Map<WorkoutPlanUpdateDto>(ChosedWorkoutPlan);
        //        mapped.ExercisesIds = ChosedWorkoutPlan.Exercises.Select(x => x.Id);
        //        mapped.DoneWorkoutsIds.Add(res2.Id);
        //        await _workoutPlanUpdateDtoWrapper.SaveAsync(mapped, false);
        //    }
        //}
    }
}
