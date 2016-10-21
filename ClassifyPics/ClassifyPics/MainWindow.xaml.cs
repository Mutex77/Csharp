using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MySql.Data.MySqlClient;

namespace ClassifyPics
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const bool TESTING = true;
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
		private System.Windows.Point
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

			appWindow.ContentRendered += new EventHandler(ResizeEvent);
			imgContainer.SizeChanged += new SizeChangedEventHandler(appWindow_SizeChanged);
		}


		#region Window Events
		//################################################################################################
		//##																							##
		//##									Window Events											##
		//##																							##
		//################################################################################################
		
		private void ResizeEvent(object sender, EventArgs e)
		{
			CenterImage();
		}

		private void appWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (TESTING)
			{
				sourcePath = "C:\\Users\\Mutex\\Pictures";
				destinationPath = "C:\\Puppy";
				sourceFolderOpened = true;
				destinationFolderOpened = true;
				GetFirstPicture();
			}
		}

		private void appWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			CenterImage();
		}

		private void CenterImage()
		{
			double imgTop;

			CropTerminate();
			BlurTerminate();

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
				GetFirstPicture();
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

		private void btnCategorize_Click(object sender, RoutedEventArgs e)
		{
			CategorizeImage();
		}

		private void btnTrash_Click(object sender, RoutedEventArgs e)
		{
			//CategorizeImage("Trash");
			test();
		}

		private void CategorizeImage()
		{
			bool isUnique = false;
			string unique = "";
			List<String> tags = new List<string>();
			string str = "";

			if (cropping)
				CropConfirm();

			if (blurring)
				BlurConfirm();

			if (Directory.Exists(destinationPath) && img.Source != null)
			{
				if (!Directory.Exists(destinationPath))
				{
					Directory.CreateDirectory(destinationPath);
				}

				while(!isUnique)
				{
					unique = string.Format(@"{0}.txt", Guid.NewGuid());
					if (!File.Exists(destinationPath + "\\" + unique + ".jpg"))
						isUnique = true;
				}
				
				FileStream fs = new FileStream(destinationPath + "\\" + unique + ".jpg", FileMode.Create);
				JpegBitmapEncoder encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(img.Source as BitmapSource));
				encoder.QualityLevel = 50;
				encoder.Save(fs);
				
				fs.Flush();
				fs.Dispose();
				encoder = null;
				img.Source = null;
				//File.Delete((img.Source as BitmapImage).UriSource.LocalPath);

				if (rbNormal.IsChecked == true)
					tags.Add("Normal");
				else if (rbMeme.IsChecked == true)
					tags.Add("Meme");

				if (cbPuppy.IsChecked == true)
					tags.Add("Puppy");

				if (cbDog.IsChecked == true)
					tags.Add("Dog");

				if (cbKitten.IsChecked == true)
					tags.Add("Kitten");

				if (cbCat.IsChecked == true)
					tags.Add("Cat");

				var pic = ShellFile.FromFilePath(destinationPath + "\\" + unique + ".jpg");
				
				foreach (string s in tags)
                {
					str = str + s + ";";
				}
				System.Windows.Forms.MessageBox.Show(str);
                pic.Properties.System.Subject.Value = str;
				
				str = new DirectoryInfo((new Uri(img.Source.ToString())).AbsolutePath).Parent.Name;
				pic.Properties.System.Comment.Value = str;

				pic.Dispose();
				
				GetPicture();
			}
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
			if(cropping)
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
				GetFirstPicture();
		}

		private void GetFirstPicture()
		{
			double imgTop;
			BitmapImage bitmap;

			if (sourceFolderOpened && destinationFolderOpened && Directory.Exists(sourcePath) && Directory.Exists(destinationPath))
			{
				var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif" };
				imgFiles = Directory.EnumerateFiles(sourcePath, "*.*", SearchOption.AllDirectories).ToList<string>();

				for(int i = imgFiles.Count - 1; i >= 0; i--)
				{
					if (!extensions.Contains(System.IO.Path.GetExtension(imgFiles[i])))
					{
						imgFiles.RemoveAt(i);
					}
				}
				
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

		private void GetPicture()
		{
			double imgTop;
			BitmapImage bitmap;

			if(cropping)
				CropTerminate();
			
			if(!sourceFolderOpened)
			{
				System.Windows.MessageBox.Show("No source folder selected. Please select a source folder.");
				SetNewSourcePath();
			}
			else if (imgFiles == null || imgFiles.Count == 0)
			{
				sourceFolderOpened = false;
				imgFiles = null;
				System.Windows.MessageBox.Show("No more images available. Please select a new source folder.");
				SetNewSourcePath();
			}
			else
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
			if (img.Source != null)
			{
				Rect rect1 = new Rect(	Canvas.GetLeft(cropArea) - Canvas.GetLeft(img), 
										Canvas.GetTop(cropArea) - Canvas.GetTop(img), 
										cropArea.ActualWidth, 
										cropArea.ActualHeight);
				Int32Rect crpRect = new Int32Rect();
				crpRect.X = (int)(rect1.X * ((img.Source as BitmapSource).PixelWidth / img.ActualWidth));
				crpRect.Y = (int)(rect1.Y * ((img.Source as BitmapSource).PixelHeight / img.ActualHeight));
				crpRect.Width = (int)(rect1.Width * ((img.Source as BitmapSource).PixelWidth / img.ActualWidth));
				crpRect.Height = (int)(rect1.Height * ((img.Source as BitmapSource).PixelHeight / img.ActualHeight));
				BitmapSource crpImg = new CroppedBitmap(img.Source as BitmapSource, crpRect);
				img.Source = crpImg;

				img.MaxHeight = crpImg.PixelHeight;
				img.MaxWidth = crpImg.PixelWidth;
			}

			cropping = false;
			btnCrop.IsEnabled = true;

			btnConfirm.Visibility = Visibility.Hidden;
			btnCancel.Visibility = Visibility.Hidden;

			cropBR.Visibility = Visibility.Hidden;
			cropTL.Visibility = Visibility.Hidden;
			cropArea.Visibility = Visibility.Hidden;

			img.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
			CenterImage();
		}

		public static Action EmptyDelegate = delegate () { };

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

				blurring = true;

				btnBlur.IsEnabled = false;

				btnConfirm.Visibility = Visibility.Visible;
				btnCancel.Visibility = Visibility.Visible;

				cropBR.Visibility = Visibility.Visible;
				cropTL.Visibility = Visibility.Visible;
				cropArea.Visibility = Visibility.Visible;
			}
		}

		private void BlurConfirm()
		{
			if(img.Source != null)
			{
				Rect rect1 = new Rect(Canvas.GetLeft(cropArea) - Canvas.GetLeft(img),
										Canvas.GetTop(cropArea) - Canvas.GetTop(img),
										cropArea.ActualWidth,
										cropArea.ActualHeight);
				System.Drawing.Rectangle blurRect = new System.Drawing.Rectangle();
				blurRect.X = (int)(rect1.X * ((img.Source as BitmapSource).PixelWidth / img.ActualWidth));
				blurRect.Y = (int)(rect1.Y * ((img.Source as BitmapSource).PixelHeight / img.ActualHeight));
				blurRect.Width = (int)(rect1.Width * ((img.Source as BitmapSource).PixelWidth / img.ActualWidth));
				blurRect.Height = (int)(rect1.Height * ((img.Source as BitmapSource).PixelHeight / img.ActualHeight));
				AForge.Imaging.Filters.Pixellate px = new AForge.Imaging.Filters.Pixellate(50);
				Bitmap bmp = BitmapFromSource(img.Source as BitmapSource);
				px.ApplyInPlace(bmp, blurRect);
				img.Source = BitmapToImageSource(bmp);
			}
			
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

		//public Bitmap BitmapFromSource(System.Windows.Media.Imaging.BitmapSource bitmapsource)
		//{
		//	//convert image format
		//	var src = new System.Windows.Media.Imaging.FormatConvertedBitmap();
		//	src.BeginInit();
		//	src.Source = bitmapsource;
		//	src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
		//	src.EndInit();

		//	//copy to bitmap
		//	Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		//	var data = bitmap.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		//	src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
		//	bitmap.UnlockBits(data);

		//	return bitmap;
		//}

		public Bitmap BitmapFromSource(System.Windows.Media.Imaging.BitmapSource bitmapsource)
		{
			//convert image format
			var src = new System.Windows.Media.Imaging.FormatConvertedBitmap();
			src.BeginInit();
			src.Source = bitmapsource;
			src.DestinationFormat = System.Windows.Media.PixelFormats.Bgr24;
			src.EndInit();

			//copy to bitmap
			Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			var data = bitmap.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
			bitmap.UnlockBits(data);

			return bitmap;
		}

		BitmapImage BitmapToImageSource(Bitmap bitmap)
		{
			using (MemoryStream memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				BitmapImage bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();

				return bitmapimage;
			}
		}

		public static Bitmap ConvertToBitmap(BitmapSource bitmapSource)
		{
			var width = bitmapSource.PixelWidth;
			var height = bitmapSource.PixelHeight;
			var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
			var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
			bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
			var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, memoryBlockPointer);
			return bitmap;
		}
		#endregion

		public void test()
		{
			MySqlConnection cn = new MySqlConnection();
			MySqlCommand cmd = new MySqlCommand();

			cn.ConnectionString = "server=; userid=; password=; database=";
			cn.Open();

			cmd.Connection = cn;
			cmd.CommandText = "INSERT INTO `mutex_PuppiesAgainstKittens`.`images` (`image_id`, `image_path`) VALUES (NULL, 'puppiesagainstkittens.com/poop/yay.jpg')";

			try
			{
				cmd.ExecuteNonQuery();
			} catch (MySqlException e)
			{
				System.Windows.MessageBox.Show("Error inserting row: \n" + e);
			}

			System.Windows.MessageBox.Show("Woot");

			cmd.Dispose();
			cn.Close();
		}
	}
}
