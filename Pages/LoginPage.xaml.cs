using System;
using DicomToPngMaui.Data;
using DicomToPngMaui.Pages;


namespace DicomToPngMaui



{
    public partial class LoginPage : ContentPage
    {
        private User UserLogin;

        public LoginPage()
        {
            InitializeComponent();
            UserLogin = new User(); 
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {


            string loginText = login.Text;
            string passwordText = password.Text;




            if (UserLogin.VerifyLogin(loginText, passwordText))
            {
                await DisplayAlert("Login Info", "Logged in successfully", "OK");
                await Navigation.PushAsync(new MainPage());
            }
            else
            {

                await DisplayAlert("Login Info", "Incorrect login or password", "OK");
            }

            
        }
    }
}