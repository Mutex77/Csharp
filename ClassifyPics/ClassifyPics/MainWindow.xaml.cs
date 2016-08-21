using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClassifyPics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            int horCenter, verCenter;

            horCenter = (int)imgContainer.Width / 2;
            verCenter = (int)imgContainer.Height / 2;

            cropArea.Margin = new Thickness(cropTL.Margin.Left + cropTL.Width, cropTL.Margin.Top + cropTL.Height, 0, 0);
            cropArea.Height = cropBR.Margin.Top - (cropTL.Margin.Top + cropTL.Height);
            cropArea.Width = cropBR.Margin.Left - (cropTL.Margin.Left + cropTL.Width);


        }
    }
}
