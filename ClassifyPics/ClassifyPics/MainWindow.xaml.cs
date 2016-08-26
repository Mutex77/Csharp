using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClassifyPics
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string
			sourcePath, 
			destinationPath;
		private bool
			sourceFolderOpened = false,
			destinationFolderOpened = false;
		private List<string>
			imgFiles;
		private TouchDevice
			//tdCropTL,
			tdCropBR;
		private Point
			//ptTLLast,
			ptBRLast;

		public MainWindow()
		{
			InitializeComponent();
			InitControls();
		}

		private void InitControls()
		{
			//int horCenter, verCenter;

			//horCenter = (int)imgContainer.Width / 2;
			//verCenter = (int)imgContainer.Height / 2;

			


		}

		private void btnSource_Click(object sender, System.EventArgs e)
		{            
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if(result.ToString() == "OK")
			{
				sourcePath = dialog.SelectedPath;
				sourceFolderOpened = true;
				if (imgFiles != null && imgFiles.Count > 0)
					imgFiles.Clear();
			}

			if (sourceFolderOpened && destinationFolderOpened)
				GetPicture();
		}

		private void btnDestination_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if (result.ToString() == "OK")
			{
				destinationPath = dialog.SelectedPath;
				destinationFolderOpened = true;
			}

			if (sourceFolderOpened && destinationFolderOpened)
				GetPicture();
		}

		private void cropBR_TouchDown(object sender, TouchEventArgs e)
		{
			e.TouchDevice.Capture(cropBR);

			if(tdCropBR == null)
			{
				tdCropBR = e.TouchDevice;
				ptBRLast = tdCropBR.GetTouchPoint(imgContainer).Position;
				Canvas.SetTop(cropBR, Canvas.GetTop(imgContainer) + (ptBRLast.Y - (cropBR.ActualHeight / 2)));
			}

			e.Handled = true;


			//if (tdRightSlider == null)
			//{
			//	tdRightSlider = e.TouchDevice;
			//	ptRightLast = tdRightSlider.GetTouchPoint(imgRightTrack).Position;
			//	Canvas.SetTop(rightSlider, Canvas.GetTop(imgRightTrack) + (ptRightLast.Y - (rightSlider.ActualHeight / 2)));
			//}

			//e.Handled = true;
		}

		private void cropBR_TouchMove(object sender, TouchEventArgs e)
		{

		}

		private void cropBR_TouchUp(object sender, TouchEventArgs e)
		{

		}
		

		private void cropTL_TouchDown(object sender, TouchEventArgs e)
		{

		}

		private void cropTL_TouchMove(object sender, TouchEventArgs e)
		{

		}

		private void cropTL_TouchUp(object sender, TouchEventArgs e)
		{

		}

        private void btnPuppy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetNewSourcePath()
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			if (result.ToString() == "OK")
			{
				sourcePath = dialog.SelectedPath;
				sourceFolderOpened = true;
				if (imgFiles != null && imgFiles.Count > 0)
					imgFiles.Clear();
			}

			if (sourceFolderOpened && destinationFolderOpened)
				GetPicture();
		}

		private void GetPicture()
		{
            double imgTop;
            BitmapImage bitmap;

			var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif" };
			if(imgFiles == null || imgFiles.Count == 0)
				imgFiles = Directory.EnumerateFiles(sourcePath, "*.*", SearchOption.AllDirectories).ToList<string>();
			if(extensions.Contains(System.IO.Path.GetExtension(imgFiles[imgFiles.Count - 1])))
			{
                bitmap = new BitmapImage(new Uri(imgFiles[imgFiles.Count - 1]));
                img.Source = bitmap;
                img.MaxHeight = bitmap.PixelHeight;
                img.MaxWidth = bitmap.PixelWidth;

                Dispatcher.Invoke(new Action(() =>
                {
                    if (img.MaxHeight >= imgContainer.ActualHeight - 85)
                    {
                        img.Height = imgContainer.ActualHeight - 85;
                        imgTop = 35;
                    }
                    else
                    {
                        imgTop = ((imgContainer.ActualHeight - img.ActualHeight - 85) / 2) + 35;
                    }

                    Canvas.SetTop(img, imgTop);

                }), DispatcherPriority.ContextIdle);

                Dispatcher.Invoke(new Action(() =>
                {
                    
                    Canvas.SetLeft(img, (imgContainer.ActualWidth - img.ActualWidth) / 2);

                    Canvas.SetTop(cropTL, Canvas.GetTop(img) - cropTL.ActualHeight);
                    Canvas.SetLeft(cropTL, Canvas.GetLeft(img) - cropTL.ActualWidth);

                    Canvas.SetTop(cropBR, Canvas.GetTop(img) + img.ActualHeight);
                    Canvas.SetLeft(cropBR, Canvas.GetLeft(img) + img.ActualWidth);

                    Canvas.SetTop(cropArea, Canvas.GetTop(img));
                    Canvas.SetLeft(cropArea, Canvas.GetLeft(img));
                    cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
                    cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);
                }), DispatcherPriority.ContextIdle);

                imgFiles.RemoveAt(imgFiles.Count - 1);

                cropBR.Visibility = Visibility.Visible;
                cropTL.Visibility = Visibility.Visible; 
                cropArea.Visibility = Visibility.Visible;
                
                
            }

            if (imgFiles.Count == 0)
			{
                cropBR.Visibility = Visibility.Hidden;
                cropTL.Visibility = Visibility.Hidden;
                cropArea.Visibility = Visibility.Hidden;
                sourceFolderOpened = false;
				SetNewSourcePath();
			}
		}
	}
}
