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
			tchCropTL = false,
			tchCropBR = false,
			styCropTL = false,
			styCropBR = false,
			cropping = false,
			blurring = false;
		private List<string>
			imgFiles;
		private Point
			ptTLLast,
			ptBRLast;
			

		public MainWindow()
		{
			InitializeComponent();

			cropTL.MouseLeftButtonDown += new MouseButtonEventHandler(cropTL_MouseLeftButtonDown);
			cropTL.MouseMove += new System.Windows.Input.MouseEventHandler(cropTL_MouseMove);
			cropTL.MouseLeftButtonUp += new MouseButtonEventHandler(cropTL_MouseLeftButtonUp);
			cropTL.TouchDown += new EventHandler<TouchEventArgs>(cropTL_TouchDown);
			cropTL.TouchMove += new EventHandler<TouchEventArgs>(cropTL_TouchMove);
			cropTL.TouchUp += new EventHandler<TouchEventArgs>(cropTL_TouchUp);

			cropBR.MouseLeftButtonDown += new MouseButtonEventHandler(cropBR_MouseLeftButtonDown);
			cropBR.MouseMove += new System.Windows.Input.MouseEventHandler(cropBR_MouseMove);
			cropBR.MouseLeftButtonUp += new MouseButtonEventHandler(cropBR_MouseLeftButtonUp);
			cropBR.TouchDown += new EventHandler<TouchEventArgs>(cropBR_TouchDown);
			cropBR.TouchMove += new EventHandler<TouchEventArgs>(cropBR_TouchMove);
			cropBR.TouchUp += new EventHandler<TouchEventArgs>(cropBR_TouchUp);
		}

		#region Window Events
		//################################################################################################
		//##																							##
		//##									Window Events											##
		//##																							##
		//################################################################################################

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

		#region CropBR Mouse Events
		//################################################################################################
		//##																							##
		//##									CropBR Mouse Events										##
		//##																							##
		//################################################################################################

		private void cropBR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			cropBR.ReleaseAllTouchCaptures();
			cropBR.ReleaseMouseCapture();
			cropBR.ReleaseStylusCapture();

			lmdCropBR = true;
			tchCropBR = false;
			styCropBR = false;

			cropBR.CaptureMouse();
		}

		private void cropBR_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			double top, left;
			if (lmdCropBR)
			{
				top = Canvas.GetTop(img) + e.GetPosition(img).Y - (cropBR.ActualHeight / 2);
				left = Canvas.GetLeft(img) + e.GetPosition(img).X - (cropBR.ActualWidth / 2);

				if (top > Canvas.GetTop(cropTL) + cropTL.ActualHeight && top < Canvas.GetTop(img) + img.ActualHeight)
					Canvas.SetTop(cropBR, top);

				if (left > Canvas.GetLeft(cropTL) + cropTL.ActualWidth && left < Canvas.GetLeft(img) + img.ActualWidth)
					Canvas.SetLeft(cropBR, left);

				Canvas.SetTop(cropArea, Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				Canvas.SetLeft(cropArea, Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);
			}
		}

		private void cropBR_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (lmdCropBR)
			{
				lmdCropBR = false;
				cropBR.ReleaseMouseCapture();
			}
		}
		#endregion

		#region CropBR Touch Events
		//################################################################################################
		//##																							##
		//##									CropBR Touch Events										##
		//##																							##
		//################################################################################################

		private void cropBR_TouchDown(object sender, TouchEventArgs e)
		{
			cropBR.ReleaseAllTouchCaptures();
			cropBR.ReleaseMouseCapture();
			cropBR.ReleaseStylusCapture();

			lmdCropBR = false;
			tchCropBR = true;
			styCropBR = false;

			cropBR.CaptureTouch(e.TouchDevice);

			e.Handled = true;
		}

		private void cropBR_TouchMove(object sender, TouchEventArgs e)
		{
			double top, left;
			if (tchCropBR)
			{
				top = Canvas.GetTop(img) + e.TouchDevice.GetTouchPoint(img).Position.Y - (cropBR.ActualHeight / 2);
				left = Canvas.GetLeft(img) + e.TouchDevice.GetTouchPoint(img).Position.X - (cropBR.ActualWidth / 2);

				if (top > Canvas.GetTop(cropTL) + cropTL.ActualHeight && top < Canvas.GetTop(img) + img.ActualHeight)
					Canvas.SetTop(cropBR, top);

				if (left > Canvas.GetLeft(cropTL) + cropTL.ActualWidth && left < Canvas.GetLeft(img) + img.ActualWidth)
					Canvas.SetLeft(cropBR, left);

				Canvas.SetTop(cropArea, Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				Canvas.SetLeft(cropArea, Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);
			}
		}

		private void cropBR_TouchUp(object sender, TouchEventArgs e)
		{
			if (tchCropBR)
			{
				tchCropBR = false;
				cropBR.ReleaseAllTouchCaptures();
			}
		}
		#endregion

		#region CropTL Mouse Events
		//################################################################################################
		//##																							##
		//##									CropTL Mouse Events										##
		//##																							##
		//################################################################################################

		private void cropTL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			cropTL.ReleaseAllTouchCaptures();
			cropTL.ReleaseMouseCapture();
			cropTL.ReleaseStylusCapture();

			lmdCropTL = true;
			tchCropTL = false;
			styCropTL = false;
			
			cropTL.CaptureMouse();
		}

		private void cropTL_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			double top, left;
			if(lmdCropTL)
			{
				top = Canvas.GetTop(img) + e.GetPosition(img).Y - (cropTL.ActualHeight / 2);
				left = Canvas.GetLeft(img) + e.GetPosition(img).X - (cropTL.ActualWidth / 2);

				if(top < Canvas.GetTop(cropBR) - cropTL.ActualHeight && top > Canvas.GetTop(img) - cropTL.ActualHeight)
					Canvas.SetTop(cropTL, top);

				if(left < Canvas.GetLeft(cropBR) - cropTL.ActualWidth && left > Canvas.GetLeft(img) - cropTL.ActualWidth)
					Canvas.SetLeft(cropTL, left);

				Canvas.SetTop(cropArea, Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				Canvas.SetLeft(cropArea, Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
			}
		}

		private void cropTL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (lmdCropTL)
			{
				lmdCropTL = false;
				cropTL.ReleaseMouseCapture();
			}
		}
		#endregion

		#region CropTL Touch Events
		//################################################################################################
		//##																							##
		//##									CropTL Touch Events										##
		//##																							##
		//################################################################################################

		private void cropTL_TouchDown(object sender, TouchEventArgs e)
		{
			cropTL.ReleaseAllTouchCaptures();
			cropTL.ReleaseMouseCapture();
			cropTL.ReleaseStylusCapture();

			lmdCropTL = false;
			tchCropTL = true;
			styCropTL = false;

			cropTL.CaptureTouch(e.TouchDevice);
			
			e.Handled = true;
		}

		private void cropTL_TouchMove(object sender, TouchEventArgs e)
		{
			double top, left;
			if (tchCropTL)
			{
				top = Canvas.GetTop(img) + e.TouchDevice.GetTouchPoint(img).Position.Y - (cropTL.ActualHeight / 2);
				left = Canvas.GetLeft(img) + e.TouchDevice.GetTouchPoint(img).Position.X - (cropTL.ActualWidth / 2);

				if (top < Canvas.GetTop(cropBR) - cropTL.ActualHeight && top > Canvas.GetTop(img) - cropTL.ActualHeight)
					Canvas.SetTop(cropTL, top);

				if (left < Canvas.GetLeft(cropBR) - cropTL.ActualWidth && left > Canvas.GetLeft(img) - cropTL.ActualWidth)
					Canvas.SetLeft(cropTL, left);

				Canvas.SetTop(cropArea, Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				Canvas.SetLeft(cropArea, Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
				cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.ActualHeight);
				cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.ActualWidth);
			}
		}

		private void cropTL_TouchUp(object sender, TouchEventArgs e)
		{
			if (tchCropTL)
			{
				tchCropTL = false;
				cropTL.ReleaseAllTouchCaptures();
			}
		}
		#endregion

		#region Button Events
		//################################################################################################
		//##																							##
		//##										Button Events										##
		//##																							##
		//################################################################################################

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

		private void btnCrop_Click(object sender, RoutedEventArgs e)
		{
			CropBegin();
		}

		private void btnBlur_Click(object sender, RoutedEventArgs e)
		{
			BlurBegin();
		}

		private void btnConfirm_Click(object sender, RoutedEventArgs e)
		{
			if(cropping)
			{
				CropConfirm();
			}
			else if (blurring)
			{
				BlurConfirm();
			}
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			if (cropping)
			{
				CropTerminate();
			}
			else if (blurring)
			{
				BlurTerminate();
			}
		}

		private void btnPuppy_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void btnKitten_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnDog_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnCat_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnTrash_Click(object sender, RoutedEventArgs e)
		{

		}
		#endregion

		#region Image Functions
		//################################################################################################
		//##																							##
		//##									Image Functions											##
		//##																							##
		//################################################################################################

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
		//################################################################################################
		//##																							##
		//##									Crop Functions											##
		//##																							##
		//################################################################################################

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

				btnCrop.IsEnabled = false;

				btnConfirm.Visibility = Visibility.Visible;
				btnCancel.Visibility = Visibility.Visible;

				cropBR.Visibility = Visibility.Visible;
				cropTL.Visibility = Visibility.Visible;
				cropArea.Visibility = Visibility.Visible;
			}
		}

		private void CropConfirm()
		{
			//perform crop
			//Need to convert incoming images to a standard type, then process crops and blurs. JPG and PNG have big problems.

			if (img.Source != null)
			{
				Rect rect1 = new Rect(	Canvas.GetLeft(cropArea) - Canvas.GetLeft(img), 
										Canvas.GetTop(cropArea) - Canvas.GetTop(img), 
										cropArea.ActualWidth, 
										cropArea.ActualHeight);
				Int32Rect crpRect = new Int32Rect();
				crpRect.X = (int)(rect1.X * (img.Source.Width / img.ActualWidth));
				crpRect.Y = (int)(rect1.Y * (img.Source.Height / img.ActualHeight));
				crpRect.Width = (int)(rect1.Width * (img.Source.Width / img.ActualWidth));
				crpRect.Height = (int)(rect1.Height * (img.Source.Height / img.ActualHeight));
				BitmapSource crpImg = new CroppedBitmap(img.Source as BitmapSource, crpRect);
				img.Source = crpImg;
			}

			cropping = false;
			btnCrop.IsEnabled = true;

			btnConfirm.Visibility = Visibility.Hidden;
			btnCancel.Visibility = Visibility.Hidden;

			cropBR.Visibility = Visibility.Hidden;
			cropTL.Visibility = Visibility.Hidden;
			cropArea.Visibility = Visibility.Hidden;
			
		}

		private void CropTerminate()
		{
			cropping = false;
			btnCrop.IsEnabled = true;

			btnConfirm.Visibility = Visibility.Hidden;
			btnCancel.Visibility = Visibility.Hidden;

			cropBR.Visibility = Visibility.Hidden;
			cropTL.Visibility = Visibility.Hidden;
			cropArea.Visibility = Visibility.Hidden;
		}
		#endregion

		#region Blur Functions
		//################################################################################################
		//##																							##
		//##									Blur Functions											##
		//##																							##
		//################################################################################################

		private void BlurBegin()
		{
			Canvas.SetTop(cropTL, Canvas.GetTop(img) - cropTL.ActualHeight);
			Canvas.SetLeft(cropTL, Canvas.GetLeft(img) - cropTL.ActualWidth);

			Canvas.SetTop(cropBR, Canvas.GetTop(img) + img.ActualHeight);
			Canvas.SetLeft(cropBR, Canvas.GetLeft(img) + img.ActualWidth);

			Canvas.SetTop(cropArea, Canvas.GetTop(img));
			Canvas.SetLeft(cropArea, Canvas.GetLeft(img));
			cropArea.Height = Canvas.GetTop(cropBR) - (Canvas.GetTop(cropTL) + cropTL.Height);
			cropArea.Width = Canvas.GetLeft(cropBR) - (Canvas.GetLeft(cropTL) + cropTL.Width);

			blurring = true;

			btnBlur.IsEnabled = false;

			btnConfirm.Visibility = Visibility.Visible;
			btnCancel.Visibility = Visibility.Visible;

			cropBR.Visibility = Visibility.Visible;
			cropTL.Visibility = Visibility.Visible;
			cropArea.Visibility = Visibility.Visible;
		}

		private void BlurConfirm()
		{
			//perform blur


			blurring = false;
			btnBlur.IsEnabled = true;

			btnConfirm.Visibility = Visibility.Hidden;
			btnCancel.Visibility = Visibility.Hidden;

			cropBR.Visibility = Visibility.Hidden;
			cropTL.Visibility = Visibility.Hidden;
			cropArea.Visibility = Visibility.Hidden;
		}

		private void BlurTerminate()
		{
			blurring = false;
			btnBlur.IsEnabled = true;

			btnConfirm.Visibility = Visibility.Hidden;
			btnCancel.Visibility = Visibility.Hidden;

			cropBR.Visibility = Visibility.Hidden;
			cropTL.Visibility = Visibility.Hidden;
			cropArea.Visibility = Visibility.Hidden;
		}
		#endregion
	}
}
