using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Model
{
    public class SimpleTask(string title, string description, bool status = false)
    {
        private const string filePath = "data.csv";

        public String Title { get; set; } = title;
        public String Description { get; set; } = description;
        public bool Status { get; set; } = status;

        public static async Task LoadDataAsync(ObservableCollection<SimpleTask> _out)
        {
            // This is terrible, but I don't have time.
            // Time to ship.
            await Task.Run(() => LoadData(_out));
        }

        public static void LoadData(ObservableCollection<SimpleTask> _out)
        {
            InitializeDataStorage(filePath, _out);
            ReadFromCsv(filePath, _out);
        }

        public static void SaveData(ObservableCollection<SimpleTask> _in)
        {
            WriteToCsv(filePath, _in);
        }

        private static void InitializeDataStorage(string filePath, ObservableCollection<SimpleTask> _out)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                GenerateData(_out);
            }
        }

        private static void GenerateData(ObservableCollection<SimpleTask> _out)
        {
            _out.Add(new SimpleTask("Machining 012", "Drilling 0xAFF3 21pce at 1000rpm"));
            _out.Add(new SimpleTask("Maintenance", "Machine 0x0010 needs repairs"));
            _out.Add(new SimpleTask("Machining 103", "WaterCutting 0xFF1F 1pc at 100bar"));
            _out.Add(new SimpleTask("Check Up", "Verify all machines and shut down manual assembly"));
        }

        private static void WriteToCsv(string filePath, ObservableCollection<SimpleTask> _in)
        {
            try
            {               
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine("Title,Description,Status");
                    foreach (var task in _in)
                    {
                        writer.WriteLine($"{task.Title},{task.Description},{task.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to CSV file: {ex.Message}");
            }
        }

        private static void ReadFromCsv(string filePath, ObservableCollection<SimpleTask> _out)
        {
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    // Skip header
                    parser.ReadLine();
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields != null && fields.Length >= 3)
                        {
                            string title = fields[0];
                            string description = fields[1];
                            bool status = bool.TryParse(fields[2], out bool parsedStatus) ? parsedStatus : false;

                            SimpleTask task = new SimpleTask(title, description, status);
                            _out.Add(task);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
            }
        }
    }
}
