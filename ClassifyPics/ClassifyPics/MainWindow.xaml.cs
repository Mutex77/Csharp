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
			destinationFolderOpened = false,
			lmdCropTL = false,
			lmdCropBR = false,
			cropping = false;
		private List<string>
			imgFiles;
		private TouchDevice
			tdCropTL,
			tdCropBR;
		private Point
			ptTLLast,
			ptBRLast;
			

		public MainWindow()
		{
			InitializeComponent();
		}

		#region Window Events
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			double imgTop;

			CropTerminate();

			if (img.Source != null)
			{
				if (img.MaxHeight >= imgContainer.ActualHeight - 85)
				{
					img.Height = imgContainer.ActualHeight - 85;
					imgTop = 35;
				}
				else
				{
					img.Height = img.MaxHeight;
					imgTop = ((imgContainer.ActualHeight - img.ActualHeight - 85) / 2) + 35;
				}

				Canvas.SetTop(img, imgTop);
				Canvas.SetLeft(img, (imgContainer.ActualWidth - img.ActualWidth) / 2);
			}
		}
		#endregion

		#region Source and Destination Events
		private void btnSource_Click(object sender, System.EventArgs e)
		{
			SetNewSourcePath();
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
		#endregion

		#region CropBR Mouse Events
		private void cropBR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			lmdCropBR = true;
			cropBR.CaptureMouse();
		}

		private void cropBR_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			lmdCropBR = false;
			cropBR.ReleaseMouseCapture();
		}

		private void cropBR_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			double top, left;
			if(lmdCropBR)
			{
				top = Canvas.GetTop(img) + e.GetPosition(img).Y - (cropBR.ActualHeight / 2);
				left = Canvas.GetLeft(img) + e.GetPosition(img).X - (cropBR.ActualWidth / 2);

				if(top > Canvas.GetTop(cropTL) + cropTL.ActualHeight)
					Canvas.SetTop(cropBR, top);

				if(left > Canvas.GetLeft(cropTL) + cropTL.ActualWidth)
					Canvas.SetLeft(cropBR, left);

				Canvas.SetTop(cropArea, Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				Canvas.SetLeft(cropArea, Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);
			}
		}
		#endregion

		#region CropBR Touch Events
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
		}

		private void cropBR_TouchMove(object sender, TouchEventArgs e)
		{

		}

		private void cropBR_TouchUp(object sender, TouchEventArgs e)
		{

		}
		#endregion

		#region CropTL Mouse Events
		private void cropTL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			lmdCropTL = true;
			cropTL.CaptureMouse();
		}

		private void cropTL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			lmdCropTL = false;
			cropTL.ReleaseMouseCapture();
		}

		private void cropTL_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			double top, left;
			if(lmdCropTL)
			{
				top = Canvas.GetTop(img) + e.GetPosition(img).Y - (cropTL.ActualHeight / 2);
				left = Canvas.GetLeft(img) + e.GetPosition(img).X - (cropTL.ActualWidth / 2);

				if(top < Canvas.GetTop(cropBR) - cropTL.ActualHeight)
					Canvas.SetTop(cropTL, top);

				if(left < Canvas.GetLeft(cropBR) - cropTL.ActualWidth)
					Canvas.SetLeft(cropTL, left);

				Canvas.SetTop(cropArea, Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				Canvas.SetLeft(cropArea, Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);
			}
		}
		#endregion

		#region CropTL Touch Events
		private void cropTL_TouchDown(object sender, TouchEventArgs e)
		{

		}

		private void cropTL_TouchMove(object sender, TouchEventArgs e)
		{

		}

		private void cropTL_TouchUp(object sender, TouchEventArgs e)
		{

		}
		#endregion

		#region Button Events
		private void btnCrop_Click(object sender, RoutedEventArgs e)
		{
			CropBegin();
		}

		private void btnBlur_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnPuppy_Click(object sender, RoutedEventArgs e)
		{

		}
		#endregion

		#region Image Functions
		private void SetNewSourcePath()
		{
			CropTerminate();

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

			CropTerminate();

			var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif" };
			if (sourceFolderOpened && imgFiles == null)
			{
				imgFiles = Directory.EnumerateFiles(sourcePath, "*.*", SearchOption.AllDirectories).ToList<string>();
			}
			else if (sourceFolderOpened && imgFiles.Count == 0)
			{
				sourceFolderOpened = false;
				SetNewSourcePath();
				imgFiles = Directory.EnumerateFiles(sourcePath, "*.*", SearchOption.AllDirectories).ToList<string>();
			}

			if (sourceFolderOpened && extensions.Contains(System.IO.Path.GetExtension(imgFiles[imgFiles.Count - 1])))
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
						img.Height = img.MaxHeight;
						imgTop = ((imgContainer.ActualHeight - img.ActualHeight - 85) / 2) + 35;
					}

					Canvas.SetTop(img, imgTop);
					Canvas.SetLeft(img, (imgContainer.ActualWidth - img.ActualWidth) / 2);
				}), DispatcherPriority.ContextIdle);

				imgFiles.RemoveAt(imgFiles.Count - 1);
				
			}
		}
		#endregion

		#region Crop Functions
		private void CropBegin()
		{
			if (img.Source != null)
			{
				Canvas.SetTop(cropTL, Canvas.GetTop(img) - cropTL.ActualHeight);
				Canvas.SetLeft(cropTL, Canvas.GetLeft(img) - cropTL.ActualWidth);

				Canvas.SetTop(cropBR, Canvas.GetTop(img) + img.ActualHeight);
				Canvas.SetLeft(cropBR, Canvas.GetLeft(img) + img.ActualWidth);

				Canvas.SetTop(cropArea, Canvas.GetTop(img));
				Canvas.SetLeft(cropArea, Canvas.GetLeft(img));
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);

				cropping = true;
				cropBR.Visibility = Visibility.Visible;
				cropTL.Visibility = Visibility.Visible;
				cropArea.Visibility = Visibility.Visible;
			}
		}

		private void CropConfirm()
		{

		}

		private void CropTerminate()
		{
			cropping = false;
			cropBR.Visibility = Visibility.Hidden;
			cropTL.Visibility = Visibility.Hidden;
			cropArea.Visibility = Visibility.Hidden;
		}
		#endregion
	}
}
