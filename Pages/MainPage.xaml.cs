
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Diagnostics;
namespace DicomToPngMaui.Pages;

using System.Net;

    

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnFilePickerClicked(object sender, EventArgs e)
    {
        var customFileType =
    new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
            { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // or general UTType values
            { DevicePlatform.Android, new[] { "application/png","application/jpg","application/dcm","application/DCM" } },
            { DevicePlatform.WinUI, new[] { ".png", ".jpg",".dcm" ,".DCM"} },
            { DevicePlatform.Tizen, new[] { "*/*" } },
            { DevicePlatform.macOS, new[] { "png", "jpg" ,"dcm","DCM"} }, // or general UTType values
    });


        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Please select a file",
            FileTypes = customFileType,
        });
        if (result == null)
            return; // user canceled file picking
  
           
        var stream = await result.OpenReadAsync();

        myImage.Source = ImageSource.FromStream(() => stream);
        
        
        //async Task<FileResult> PickAndShow(PickOptions options)
        //{
        //    try
        //    {
             
                
        //        var result = await FilePicker.PickAsync(options);
        //        if (result != null)
        //        {
        //            //Text = $"File Name: {result.FileName}";
        //            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
        //                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
        //            {
        //                var stream = await result.OpenReadAsync();
        //                var Image = ImageSource.FromStream(() => stream);


        //                byte[] fileBytes = new byte[stream.Length];
        //                await stream.ReadAsync(fileBytes, 0, (int)stream.Length);
        //                string fileName = result.FileName;
        //                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
        //                File.WriteAllBytes(filePath, fileBytes);
        //            }
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        // The user canceled or something went wrong
        //    }

        //    return null;
        }

   
    //    var options = new PickOptions
    //    {
    //        PickerTitle = "Please select a comic file",
    //        FileTypes = customFileType,
    //    };
    //    await PickAndShow(options);
    //}
    

    


}