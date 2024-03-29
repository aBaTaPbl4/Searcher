﻿using System.Windows;
using Searcher.VM;

namespace Searcher
{
    /// <summary>
    /// Interaction logic for wndDetails.xaml
    /// </summary>
    public partial class WndDetails : Window
    {
        private ScanDataVM _data;

        public WndDetails()
        {
            InitializeComponent();
        }


        public ScanDataVM Data
        {
            get { return _data; }
            set
            {
                _data = value;
                DataContext = value;
            }
        }
    }
}