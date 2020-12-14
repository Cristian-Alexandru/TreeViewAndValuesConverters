using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TreeViewAndValuesConverters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region DefaultConstructor
        /// <summary>
        /// DefaultConstructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

        }
        #endregion DefaultConstructor

        #region On Load
        /// <summary>
        /// What the application do when is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get every logicla driver on machine
            foreach (var drive in Directory.GetLogicalDrives())
            {
                //Create a new item for it
                var items = new TreeViewItem()
                {
                    //Seat the header
                    Header = drive,
                    //Add the full path
                    Tag = drive
                };

                //Add a dumy items
                items.Items.Add(null);

                //Listen out for items being expanded
                items.Expanded += Folder_Expanded;

                //Add it to the main tree view
                FolderView.Items.Add(items);
            }
        }
        #endregion

        /// <summary>
        /// When a folder is expanded, find the sub folder/files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            //If the item only contains the dumy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            //Clear the items off
            item.Items.Clear();

            //Get full path
            var fullPath = (string)item.Tag;
            #endregion

            #region Folder Expande
            //Create a blank list for directories
            var directories = new List<string>();

            //Trying to get directories from the folder
            //Ignoring any issues
            try
            {
                var dir = Directory.GetDirectories(fullPath);
                if (dir.Length > 0)
                    directories.AddRange(dir);

            }
            catch { }

            directories.ForEach(directoryPath =>
            {
                //Create directory item
                var subItem = new TreeViewItem()
                {
                    //Set header as folder name
                    //In this situation header represent the name of the folder
                    //Ex: C:
                    Header = GetFileFolderName(directoryPath),
                    //Set tag as folder path
                    //In this situation tag represent the full path of the folder
                    //Ex: C:/ProgramFile/Google/Chrome/chrome.exe
                    Tag = directoryPath
                };

                //Adding a dumy item for expanding the folder
                subItem.Items.Add(null);

                //Handle the expanding out
                subItem.Expanded += Folder_Expanded;
                //Add this items to the parrent
                item.Items.Add(subItem);

            });
            #endregion

            #region Get File
            //Create a blank list for directories
            var files = new List<string>();

            //Trying to get file from the folder
            //Ignoring any issues
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);

            }
            catch { }

            files.ForEach(filePath =>
            {
                //Create file item
                var subItem = new TreeViewItem()
                {
                    //Set header as file name
                    //In this situation header represent the name of the folder
                    //Ex: C:
                    Header = GetFileFolderName(filePath),
                    //Set tag as folder path
                    //In this situation tag represent the full path of the folder
                    //Ex: C:/ProgramFile/Google/Chrome/chrome.exe
                    Tag = filePath
                };
                //Add this items to the parrent
                item.Items.Add(subItem);

            });
            #endregion


        }



        #region Helper
        /// <summary>
        /// Return the file or folder name from a full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            //C:\xxx\xxx\xxx\file.png

            //If we have a null  or empty string return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //Make all slashes, back slashes
            var normalizePath = path.Replace('/', '\\');
            //Find the last backslash
            var lastIndex = normalizePath.LastIndexOf('\\');

            //If we don't find a \ return the path itself
            if (lastIndex <= 0)
                return path;

            //Return name after the last \
            return path.Substring(lastIndex + 1);
            #endregion

        }
    }
}
