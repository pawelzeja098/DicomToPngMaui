namespace DicomToPngMaui.Pages;

public partial class ViewSelectedImage : ContentPage
{
	public ViewSelectedImage(string selectedImagePath)
	{
		InitializeComponent();

		// Set the image source to the selected image
		imageView.Source = selectedImagePath;


	}
}