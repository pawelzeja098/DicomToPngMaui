namespace DicomToPngMaui.Pages;

//Carousell view
public partial class ViewImages : ContentPage
{
    public ViewImages()
    {
        InitializeComponent();

        DateTime lastUnpackTime = DateTime.MinValue;

        var path = FileSystem.Current.AppDataDirectory;



        // Get a list of PNG files created after the last call to UnpackDicom
        var pngFiles = Directory.GetFiles(path, "frame_*.png")
                                .Where(file => File.GetLastWriteTime(file) > lastUnpackTime)
                                .ToList();

        imageCarousel.ItemsSource = pngFiles;
        List<object> imageItems = new List<object>();

        // Sort the files by frame number
        var sortedPngFiles = pngFiles.OrderBy(file =>
        {

            var fileName = Path.GetFileNameWithoutExtension(file);
            var frameNumberString = fileName.Replace("frame_", "");
            if (int.TryParse(frameNumberString, out int frameNumber))
            {
                return frameNumber;
            }
            return 0;
        }).ToList();

        foreach (var pngFile in sortedPngFiles)
        {

            var imageName = Path.GetFileNameWithoutExtension(pngFile);
            var imageItem = new { FileName = imageName, ImagePath = pngFile };
            imageItems.Add(imageItem);




        }
        imageCarousel.ItemsSource = imageItems;
    }
}







