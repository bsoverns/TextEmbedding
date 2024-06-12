using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using TextEmbedding;

namespace ImageEmbedding
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                string directory = Path.GetDirectoryName(openFileDialog.FileName);
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                string extension = Path.GetExtension(openFileDialog.FileName);
                SaveLocationTextBox.Text = Path.Combine(directory, $"{filenameWithoutExtension}-embedded{extension}");
            }
        }

        private void EmbedTextButton_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = ImagePathTextBox.Text;
            string textToEmbed = EmbedTextBox.Text;
            string saveLocation = SaveLocationTextBox.Text;

            try
            {
                EmbedTextInImage(imagePath, textToEmbed, saveLocation);
                MessageBox.Show("Text embedded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportTextButton_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = ImagePathTextBox.Text;

            try
            {
                string extractedText = ExtractTextFromImage(imagePath);
                ExportedTextBox.Text = extractedText;
                MessageBox.Show("Text extracted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EmbedTextInImage(string imagePath, string text, string saveLocation)
        {
            string encryptedText = EncryptionClass.Encrypt(text);
            Bitmap bitmap = BitmapImageToBitmap(new BitmapImage(new Uri(imagePath)));
            Bitmap embeddedBitmap = SteganographyHelper.embedText(encryptedText, bitmap);
            embeddedBitmap.Save(saveLocation, ImageFormat.Png);
        }

        private string ExtractTextFromImage(string imagePath)
        {
            string encryptedText = SteganographyHelper.extractText(new Bitmap(imagePath));
            string Text = EncryptionClass.Decrypt(encryptedText);
            return Text;
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
