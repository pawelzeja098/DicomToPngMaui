
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Diagnostics;

using System.Net;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using SkiaSharp;
using Microsoft.Maui.Controls.PlatformConfiguration;


namespace DicomToPngMaui.Pages;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        //LoadImages();
    }
    private DateTime lastUnpackTime = DateTime.MinValue;
    [Obsolete]
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


        downloadProgressBar.IsVisible = true;
        var url = YourUrlEntry.Text;

        if (url != null)
        {
            try
            {
                downloadProgressBar.Progress = 0.1;

                using (var httpClient = new HttpClient())
                {
                    // Get data from URL
                    var data = await httpClient.GetByteArrayAsync(url);

                    downloadProgressBar.Progress = 0.5;

                    var fileName = "DownloadedFile";
                    var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
                    File.WriteAllBytes(filePath, data);

                    await DisplayAlert("Success", $"File downloaded and saved in: {fileName}", "OK");

                    UnpackDicom(filePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error ocurred: {ex.Message}", "OK");
            }
        }

        else
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a file",
                FileTypes = customFileType,
            });
            if (result == null)
                return; // user canceled file picking

            downloadProgressBar.Progress = 0.1;
            
            var stream = await result.OpenReadAsync();

            //myImage.Source = ImageSource.FromStream(() => stream);

            string tmpFolderPath = GetTmpFolderPath();

            // Create a target file path in the tmp folder
            string destinationFilePath = Path.Combine(tmpFolderPath, result.FileName);

            // Save the stream to the file
            using (FileStream fileStream = File.Create(destinationFilePath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
            }

            MemoryStream copyStream = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(copyStream);

            downloadProgressBar.Progress = 0.5;



            UnpackDicom(destinationFilePath);
        }
    }

    private async void UnpackDicom(string fileStream)
    {
        // Source to the dicom file
        string filePath = fileStream;

        // Load dicom file
        DicomFile dicomFile = DicomFile.Open(filePath);

        // Unpack image data if exist
        if (dicomFile.Dataset.Contains(DicomTag.PixelData))
        {
            DicomPixelData pixelData = DicomPixelData.Create(dicomFile.Dataset);

            
            int numberOfFrames = pixelData.NumberOfFrames;


            var path = FileSystem.Current.AppDataDirectory;

            for (int frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
            {
                // Get pixel data for the frame
                byte[] pixelBytes = pixelData.GetFrame(frameIndex).Data;

                

                using (var stream = new SKMemoryStream(pixelBytes))
                using (var skBitmap = SKBitmap.Decode(stream))
                {
                    // Save file into PNG format.
                    string outputImagePath = Path.Combine(path, $"frame_{frameIndex + 1}.png"); // frame_1.jpg, frame_2.jpg, ...
                    downloadProgressBar.Progress = 0.9;
                    using (var imageStream = File.OpenWrite(outputImagePath))
                    {
                        skBitmap.Encode(imageStream, SKEncodedImageFormat.Png, 100);
                    }
                }

            }

            

            await DisplayAlert("Alert", "Finished unpacking dicom", "OK");
            Console.WriteLine("Finished unpacking dicom");
            downloadProgressBar.Progress = 1;
            //LoadImages();
        }
        else
        {
            await DisplayAlert("Alert", "There is no image data in the DICOM file.", "OK");
            Console.WriteLine("There is no image data in the DICOM file.");
        }
    }

    //private void LoadImages()
    //{
    //    var path = FileSystem.Current.AppDataDirectory;

    //    // Usu� wcze�niej wy�wietlone obrazy
    //    //imageStackLayout.Children.Clear();

    //    // Get a list of PNG files created after the last call to UnpackDicom
    //    var pngFiles = Directory.GetFiles(path, "frame_*.png")
    //                            .Where(file => File.GetLastWriteTime(file) > lastUnpackTime)
    //                            .ToList();

    //    foreach (var pngFile in pngFiles)
    //    {
    //        var image = new Image
    //        {
    //            Source = ImageSource.FromFile(pngFile),
    //            WidthRequest = 300, // Dostosuj szeroko�� wed�ug potrzeb
    //            HeightRequest = 300 // Dostosuj wysoko�� wed�ug potrzeb
    //        };
    //        // Dodaj �cie�ki do plik�w do ObservableCollection, aby zaktualizowa� CarouselView
    //        //imageStackLayout.Children.Add(image);
    //    }
    //}
    private void OnOpenImagesClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ViewImages());

    }
        
    //    var path = FileSystem.Current.AppDataDirectory;

    //    // Usu� wcze�niej wy�wietlone obrazy
    //    imageStackLayout.Children.Clear();

    //    // Get a list of PNG files created after the last call to UnpackDicom
    //    var pngFiles = Directory.GetFiles(path, "frame_*.png")
    //                            .Where(file => File.GetLastWriteTime(file) > lastUnpackTime)
    //                            .ToList();
        
    //    imageCarousel.ItemsSource = pngFiles;

    //    foreach (var pngFile in pngFiles)
    //        {
    //        var image = new Image
    //        {
    //            Source = ImageSource.FromFile(pngFile),
    //            WidthRequest = 300, // Dostosuj szeroko�� wed�ug potrzeb
    //            HeightRequest = 300 // Dostosuj wysoko�� wed�ug potrzeb
    //        };
    //        // Dodaj �cie�ki do plik�w do ObservableCollection, aby zaktualizowa� CarouselView
    //        imageStackLayout.Children.Add(image);

            
    //    }
       
    //    // Wy�wietl CarouselView w pe�noekranowym oknie
    //    Navigation.PushModalAsync(new NavigationPage(new ContentPage
    //    {
    //        Content = imageCarousel
    //    }));
    //}
    //private int currentIndex = 0;
    //private void OnPreviousClicked(object sender, EventArgs e)
    //{
    //    if (currentIndex > 0)
    //    {
    //        currentIndex--;
    //        imageCarousel.Position = currentIndex;
    //    }
    //}

    //private void OnNextClicked(object sender, EventArgs e)
    //{
    //    if (currentIndex < 100)
    //    {
    //        currentIndex++;
    //        imageCarousel.Position = currentIndex;
    //    }
    //}
    [Obsolete]
    private string GetTmpFolderPath()
    {
        string tmpFolderPath = "";

        // Adjust the tmp folder path depending on your platform
        switch (Device.RuntimePlatform)
        {
            case Device.iOS:
                tmpFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "tmp");
                break;

            case Device.Android:
                tmpFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "tmp");
                break;

            
            case Device.WinUI:
                tmpFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tmp");
                break;

            default:
                throw new NotSupportedException("Platform not supported");
        }

        // Create tmp if not exists
        if (!Directory.Exists(tmpFolderPath))
        {
            Directory.CreateDirectory(tmpFolderPath);
        }

        return tmpFolderPath;
    }
    private async void OnClearFolderClicked(object sender, EventArgs e)
    {
        string appDataDirectory = FileSystem.AppDataDirectory;

        // Delete file from the app data directory
        foreach (var filePath in Directory.GetFiles(appDataDirectory))
        {
            File.Delete(filePath);
            Console.WriteLine($"Usuni�to plik: {filePath}");
        }
        await DisplayAlert("Alert", "Files have been deleted from LocalData", "OK");
    }   
   
}




