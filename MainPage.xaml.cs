using material_manager.ViewModels;

namespace material_manager;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
