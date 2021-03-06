﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Launcher_1._0
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Dictionary<Button, string> filePaths = new Dictionary<Button, string>();

        List<FileInfo> getFileInfos(string path, string fileName)
        {
            return new DirectoryInfo(path.ToString()).GetFiles(fileName, SearchOption.AllDirectories).ToList();
        }

        void renderIcos(List<FileInfo> files)
        {
            foreach(FileInfo file in files)
            {
                var fileType = file.Directory.Extension;
                createIco(file.Name, file.Directory.ToString());
            }
        }

        void createIco(string name, string path)
        {
            /*<Button.ContextMenu>
                <ContextMenu>
                    <MenuItem Header="Menu item 1" />
                    <MenuItem Header="Menu item 2" />
                    <Separator />
                    <MenuItem Header="Menu item 3" />
                </ContextMenu>
            </Button.ContextMenu>*/
            Button newButton = new Button();
            newButton.Width = 80;
            newButton.Height = 80;
            newButton.Content = name;
            newButton.Margin = new Thickness(5);
            newButton.HorizontalAlignment = HorizontalAlignment.Left;
            newButton.VerticalAlignment = VerticalAlignment.Top;
            newButton.Click += icon_click;

            ContextMenu contextMenu = new ContextMenu();

            MenuItem delete = new MenuItem();

            newButton.ContextMenu = contextMenu;
            
            filePaths.Add(newButton, path);

            iconPanel.Children.Insert(0, newButton);
        }

        private void deleteFile_click(object sender, EventArgs e)
        {
        }


        void openFile(string fileName, string directory)
        {
            Process proc = new Process();

            try
            {
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.WorkingDirectory = directory;
                proc.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Aplikace nezpuštěna (" + e.Message + ")");
            }
        }

        private void icon_click(object sender, RoutedEventArgs e)
        {
            Button ico = sender as Button;

            string directory = filePaths[ico];

            openFile(ico.Content.ToString(), directory);
        }

        private void selectPath_click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog x = new OpenFileDialog();
                x.Multiselect = false;
                x.Filter = "All Files (*.exe*)|*.csproj*";
                x.ShowDialog();
                string filePath = x.FileName;
                string result = System.IO.Path.GetDirectoryName(filePath);
                string fileName = x.SafeFileName;
                string exeFile = fileName.Replace(".csproj", ".exe");

                MessageBox.Show("Selected directory: " + result);

                iconPanel.Children.Clear();
                renderIcos(getFileInfos(result, exeFile));
            }
            catch (Exception pathE)
            {
                MessageBox.Show("Error: " + pathE.Message);
            }
        }
    }
}
