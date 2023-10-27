using System;
using DicomToPngMaui.Data;


namespace DicomToPngMaui



{
    public partial class MainPage : ContentPage
    {
        private User UserLogin;

        public MainPage()
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
            }
            else
            {
                await DisplayAlert("Login Info", "Incorrect login or password", "OK");
            }

            //void OnEntryTextChanged(object sender, TextChangedEventArgs e)
            //{
            //    string oldText = e.OldTextValue;
            //    string newText = e.NewTextValue;
            //    string myText = login.Text;
            //}

            //void OnEntryCompleted(object sender, EventArgs e)
            //{
            //    string text = ((Entry)sender).Text;
            //}

            //count++;

            //if (count == 1)
            //    CounterBtn.Text = $"Clicked {count} time";
            //else
            //    CounterBtn.Text = $"Clicked {count} times";

            //SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}