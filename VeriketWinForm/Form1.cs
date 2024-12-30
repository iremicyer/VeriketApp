using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace VeriketWinForm
{
    public partial class Form1 : Form
    {
        private System.Threading.Timer scheduler; // System.Threading.Timer
        private DateTime lastModifiedTime;

        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;

            // Timer ba�lat
            StartScheduler();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // ProgramData klas�r�n�n yolunu al
            string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // VeriketApp klas�r�n� olu�tur
            string logDirectory = Path.Combine(programDataPath, "VeriketApp");

            // CSV dosyas�n�n tam yolunu belirle
            string csvFilePath = Path.Combine(logDirectory, "VeriketAppTest.txt"); // Burada dosya ad�n�z� kullanabilirsiniz.

            // CSV dosyas�n� oku ve DataGridView'e y�kle
            if (File.Exists(csvFilePath))
            {
                DataTable table = LoadCsvIntoDataTable(csvFilePath);
                dataGridView1.DataSource = table;

                // Dosyan�n de�i�tirilme zaman�n� g�ncelle
                lastModifiedTime = File.GetLastWriteTime(csvFilePath);
            }
            else
            {
                MessageBox.Show("CSV dosyas� bulunamad�. L�tfen dosya yolunu kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoadCsvIntoDataTable(string filePath)
        {
            DataTable dataTable = new DataTable();

            try
            {
                // CSV dosyas�n� oku
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    // Ba�l�k sat�r�n� (kolon isimleri) ekle
                    string[] headers = lines[0].Split(',');
                    foreach (string header in headers)
                    {
                        if (header == "Tarih")
                            dataTable.Columns.Add(header, typeof(DateTime)); // Tarih s�tunu i�in DateTime t�r�
                        else if (header == "Saat")
                            dataTable.Columns.Add(header, typeof(TimeSpan)); // Saat s�tunu i�in TimeSpan t�r�
                        else
                            dataTable.Columns.Add(header, typeof(string)); // Di�er s�tunlar i�in string t�r�
                    }

                    // Sat�rlar� ekle
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] data = lines[i].Split(',');
                        DataRow row = dataTable.NewRow();

                        for (int j = 0; j < headers.Length; j++)
                        {
                            if (headers[j] == "Tarih")
                                row[j] = DateTime.Parse(data[j]); // Tarih s�tununu DateTime olarak ekle
                            else if (headers[j] == "Saat")
                                row[j] = TimeSpan.Parse(data[j]); // Saat s�tununu TimeSpan olarak ekle
                            else
                                row[j] = data[j]; // Di�er s�tunlar� string olarak ekle
                        }

                        dataTable.Rows.Add(row);
                    }

                    // Tarih ve saate g�re s�ralama (en g�ncel en �stte)
                    dataTable.DefaultView.Sort = "Tarih DESC, Saat DESC";
                    dataTable = dataTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV dosyas� okunurken bir hata olu�tu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataTable;
        }

        private void StartScheduler()
        {
            scheduler = new System.Threading.Timer(CheckFileUpdate, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void CheckFileUpdate(object state)
        {
            try
            {
                string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string logDirectory = Path.Combine(programDataPath, "VeriketApp");
                string csvFilePath = Path.Combine(logDirectory, "VeriketAppTest.txt");

                if (File.Exists(csvFilePath))
                {
                    DateTime currentModifiedTime = File.GetLastWriteTime(csvFilePath);

                    if (currentModifiedTime > lastModifiedTime)
                    {
                        lastModifiedTime = currentModifiedTime;

                        // DataGridView'i g�ncelle
                        Invoke(new Action(() =>
                        {
                            button1.PerformClick(); // Grid'i yeniden y�kle
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Scheduler hatas�: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (scheduler != null)
            {
                scheduler.Dispose();
            }
        }
    }
}
