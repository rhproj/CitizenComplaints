﻿using System;
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
using System.Windows.Shapes;

namespace ComplaintsAdmin.Views
{
    /// <summary>
    /// Interaction logic for EditUsersView.xaml
    /// </summary>
    public partial class EditUsersView : Window
    {
        public EditUsersView()
        {
            InitializeComponent();

            btnClose.Click += (s, e) => Close();
        }
    }
}
