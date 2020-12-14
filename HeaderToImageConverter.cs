
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TreeViewAndValuesConverters
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder, file
    /// </summary>
 
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the full path
            var path = (string)value;

            //If the path is null
            if (path == null)
                return null;
            var name = MainWindow.GetFileFolderName(path);

            //By default, we presume an image
            var image = "Images/file.png";

            //If the name is blank, we presume it's drive as we cannot have a blank file or folder
            if (string.IsNullOrEmpty(name))
                image = "Images/drive.png";
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Images/folder-closed.png";


            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
