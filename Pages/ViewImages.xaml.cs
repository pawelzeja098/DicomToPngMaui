namespace DicomToPngMaui.Pages;

public partial class ViewImages : ContentPage
{
	public ViewImages()
	{
		InitializeComponent();

       DateTime lastUnpackTime = DateTime.MinValue;

       var path = FileSystem.Current.AppDataDirectory;

        // Usuñ wczeœniej wyœwietlone obrazy
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
                WidthRequest = 300, // Dostosuj szerokoœæ wed³ug potrzeb
                HeightRequest = 300 // Dostosuj wysokoœæ wed³ug potrzeb
            };
            // Dodaj œcie¿ki do plików do ObservableCollection, aby zaktualizowaæ CarouselView
            imageStackLayout.Children.Add(image);


        }
    }
}