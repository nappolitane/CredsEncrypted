using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace CredsEncrypted
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string mCreds;
        private string mCredsPath;
        private string inputPassword;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var dlg = new PasswordDialogBox { Owner = this };
                dlg.ShowDialog();
                if (dlg.DialogResult == true) { inputPassword = dlg.passwordBox.Password; }
                else return;

                mCreds = File.ReadAllText(openFileDialog.FileName);
                byte[] iv = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] key = Encoding.ASCII.GetBytes(inputPassword);
                byte[] cipherText;
                try
                {
                    cipherText = Convert.FromBase64String(mCreds);
                }
                catch
                {
                    DataTextBox.Text = mCreds;

                    cipherText = EncryptorDecryptorManager.Encrypt(mCreds, key, iv);
                    string cipherToFile = Convert.ToBase64String(cipherText);
                    string partialPath = openFileDialog.FileName.Remove(openFileDialog.FileName.LastIndexOf('\\'));
                    mCredsPath = System.IO.Path.Combine(partialPath, "enc.txt");
                    using (StreamWriter outputFile = new StreamWriter(mCredsPath))
                    {
                        outputFile.Write(cipherToFile);
                    }
                    MessageBox.Show("Encrypted credentials saved to: " + mCredsPath);
                    OpenFileTextBox.Text = mCredsPath;
                    return;
                }

                try
                {
                    mCreds = EncryptorDecryptorManager.Decrypt(cipherText, key, iv);
                }
                catch
                {
                    MessageBox.Show("Wrong Password!");
                    return;
                }

                OpenFileTextBox.Text = openFileDialog.FileName;
                DataTextBox.Text = mCreds;
                mCredsPath = openFileDialog.FileName;
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(mCredsPath);

            byte[] iv = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] key = Encoding.ASCII.GetBytes(inputPassword);
            byte[] cipherText;

            mCreds = DataTextBox.Text;

            cipherText = EncryptorDecryptorManager.Encrypt(mCreds, key, iv);
            string cipherToFile = Convert.ToBase64String(cipherText);
            using (StreamWriter outputFile = new StreamWriter(mCredsPath))
            {
                outputFile.Write(cipherToFile);
            }
            MessageBox.Show("Encrypted credentials saved to: " + mCredsPath);
            OpenFileTextBox.Text = mCredsPath;

            Close();
        }

        private void ExitWOSaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
