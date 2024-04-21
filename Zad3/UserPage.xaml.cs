
namespace Zad3;

public partial class UserPage : ContentPage
{
	public UserPage(string userName)
	{
		InitializeComponent();
        DisplayUserInformation(userName);
    }

    private void DisplayUserInformation(string userName)
    {
        if (userName == null) { UserDescriptionLabel.Text = "No information available"; }
        else { UserDescriptionLabel.Text = $"Hi my name is {userName}! This is my profile"; }
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}