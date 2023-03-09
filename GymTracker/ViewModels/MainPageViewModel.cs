using GymTracker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {

        [RelayCommand]
        async Task GoToTrainings()
        {
            await Shell.Current.GoToAsync(nameof(ListOfTrainings));
        }
        [RelayCommand]
        async Task AddNewTraning()
        {
            await Shell.Current.GoToAsync(nameof(AddTraining));
        }
        [RelayCommand]
        async Task AddNewWorkoutPlan()
        {
            await Shell.Current.GoToAsync(nameof(AddWorkoutPlan));
        }
    }
}
