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
using System.Windows.Shapes;

namespace CredsEncrypted
{
    /// <summary>
    /// Interaction logic for PasswordDialogBox.xaml
    /// </summary>
    public partial class PasswordDialogBox : Window
    {
        public PasswordDialogBox()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            while (passwordBox.Password.Length < 16)
            {
                passwordBox.Password += "A";
            }
            while (passwordBox.Password.Length > 16)
            {
                passwordBox.Password = passwordBox.Password.Substring(0, 16);
            }
            DialogResult = true;
        }
    }
}
