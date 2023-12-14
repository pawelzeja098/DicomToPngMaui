namespace DicomToPngMaui.Pages;

public partial class ViewImages : ContentPage
{
	public ViewImages()
	{
		InitializeComponent();

       DateTime lastUnpackTime = DateTime.MinValue;

       var path = FileSystem.Current.AppDataDirectory;

        // Usu� wcze�niej wy�wietlone obrazy
        imageStackLayout.Children.Clear();

        // Get a list of PNG files created after the last call to UnpackDicom
        var pngFiles = Directory.GetFiles(path, "frame_*.png")
                                .Where(file => File.GetLastWriteTime(file) > lastUnpackTime)
                                .ToList();

        imageCarousel.ItemsSource = pngFiles;

        foreach (var pngFile in pngFiles)
        {
            var image = new Image
            {
                Source = ImageSource.FromFile(pngFile),
                WidthRequest = 300, // Dostosuj szeroko�� wed�ug potrzeb
                HeightRequest = 300 // Dostosuj wysoko�� wed�ug potrzeb
            };
            // Dodaj �cie�ki do plik�w do ObservableCollection, aby zaktualizowa� CarouselView
            imageStackLayout.Children.Add(image);


        }
    }
}