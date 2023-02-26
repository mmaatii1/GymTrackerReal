using GymTracker.Views;

namespace GymTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        Routing.RegisterRoute(nameof(ListOfTrainings), typeof(ListOfTrainings));
    }
}
