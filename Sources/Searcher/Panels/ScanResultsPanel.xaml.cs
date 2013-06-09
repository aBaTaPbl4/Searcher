using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common;
using Searcher.VM;

namespace Searcher.Panels
{
    public partial class ScanResultsPanel : UserControl
    {
        private ActiveScanPanelVM _scanData;

        public ScanResultsPanel(ActiveScanPanelVM scanData)
        {
            InitializeComponent();
            _scanData = scanData;
        }

        private void MenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            MenuItem m = (MenuItem)sender;
            if (m != null)
            {
                ScanDataVM s = (ScanDataVM)lstResults.Items.CurrentItem;
                if (s != null)
                {
                    //todo:add delete files
                    switch (m.Header.ToString())
                    {
                        case "Select Item":
                            {
                                s.Checked = true;
                                break;
                            }
                        case "Deselect Item":
                            {
                                s.Checked = false;
                                break;
                            }
                        case "Select All":
                            {
                                _scanData.CheckResults(true);
                                break;
                            }
                        case "Deselect All":
                            {
                                _scanData.CheckResults(false);
                                break;
                            }
                        case "Go to Folder":
                            {
                                OpenFolder(s);
                                break;
                            }
                        case "Item Details":
                            {
                                WndDetails wndData = new WndDetails();
                                wndData.Data = (ScanDataVM)lstResults.Items.CurrentItem;
                                wndData.ShowDialog();
                                break;
                            }
                    }
                }
            }
        }


        private void OpenFolder(ScanDataVM dataVM)
        {
            string argument;
            if (AppContext.FileSystem.FileExtists(dataVM.FullName))
            {
                argument = string.Format("/select, \"{0}\"", dataVM.FullName);
            }
            else if (AppContext.FileSystem.DirectoryExists(dataVM.FolderName))
            {
                argument = string.Format("\"{0}\"", dataVM.FolderName);
            }
            else
            {
                return;
            }
            Process.Start("explorer.exe", argument);
        }

    }
}
