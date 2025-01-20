using Google.Protobuf;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace StaticDataConverter
{
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void BrowseDat(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                InputDat.Text = openFileDialog.FileName;
            }
        }

        private void BrowseJSON(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                InputJSON.Text = openFileDialog.FileName;
            }
        }

        private void ConvertJsonToDat(object sender, RoutedEventArgs e) {
            try {
                string json = File.ReadAllText(InputJSON.Text);

                // Parse JSON into a StaticData protobuf object
                var parser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
                var staticData = parser.Parse<StaticData.StaticData>(json);

                // Serialize to .dat file
                string outputFilePath = Path.ChangeExtension(InputJSON.Text, ".dat");
                using (var output = File.Create(outputFilePath)) {
                    staticData.WriteTo(output);
                }

                MessageBox.Show("Conversion from JSON to .dat completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Error during conversion: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConvertDatToJson(object sender, RoutedEventArgs e) {
            try {
                // Read .dat file into a StaticData protobuf object
                var staticData = new StaticData.StaticData();
                using (var input = File.OpenRead(InputDat.Text)) {
                    staticData.MergeFrom(input);
                }

                // Convert protobuf object to JSON using Google.Protobuf.JsonFormatter
                var json = JsonFormatter.Default.Format(staticData);

                // Format JSON using Newtonsoft.Json for pretty printing
                var formattedJson = Newtonsoft.Json.JsonConvert.SerializeObject(
                    Newtonsoft.Json.JsonConvert.DeserializeObject(json),
                    Newtonsoft.Json.Formatting.Indented
                );

                // Write the formatted JSON to a file
                string outputFilePath = Path.ChangeExtension(InputDat.Text, ".json");
                File.WriteAllText(outputFilePath, formattedJson);
                MessageBox.Show("Conversion from .dat to JSON completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Error during conversion: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}