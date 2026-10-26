using material_manager.ViewModels;

namespace material_manager;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		if (BindingContext is MainViewModel vm)
		{
			Task.Run(vm.Init);
		}
    }
}
