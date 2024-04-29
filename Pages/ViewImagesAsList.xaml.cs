namespace DicomToPngMaui.Pages;

public partial class ViewImagesAsList : ContentPage
{
	public ViewImagesAsList()
	{
		InitializeComponent();
        DateTime lastUnpackTime = DateTime.MinValue;
        var path = FileSystem.AppDataDirectory;

        // Get a list of PNG files created after the last call to UnpackDicom
        var pngFiles = Directory.GetFiles(path, "frame_*.png")
                                .Where(file => File.GetLastWriteTime(file) > lastUnpackTime)
                                .ToList();

        // Create a list of image items
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

        // Create a ListView to display images
        var listView = new ListView();
        listView.ItemsSource = imageItems;
        listView.ItemTemplate = new DataTemplate(() =>
        {
            var imageCell = new ImageCell();
            imageCell.SetBinding(ImageCell.TextProperty, "FileName");
            imageCell.SetBinding(ImageCell.ImageSourceProperty, "ImagePath");
            return imageCell;
        });

        // Handle item selection
        listView.ItemSelected += async (sender, e) =>
        {
            if (e.SelectedItem != null)
            {
                // Handle selected item, e.g., display in a separate page
                var selectedItem = (dynamic)e.SelectedItem;
                var selectedImagePath = selectedItem.ImagePath;

                var nextPage = new ViewSelectedImage(selectedImagePath);
                await Navigation.PushAsync(nextPage);
            }
        };

        Content = listView;
    }
}