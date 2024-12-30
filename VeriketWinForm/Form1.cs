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

            // Timer baþlat
            StartScheduler();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // ProgramData klasörünün yolunu al
            string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // VeriketApp klasörünü oluþtur
            string logDirectory = Path.Combine(programDataPath, "VeriketApp");

            // CSV dosyasýnýn tam yolunu belirle
            string csvFilePath = Path.Combine(logDirectory, "VeriketAppTest.txt"); // Burada dosya adýnýzý kullanabilirsiniz.

            // CSV dosyasýný oku ve DataGridView'e yükle
            if (File.Exists(csvFilePath))
            {
                DataTable table = LoadCsvIntoDataTable(csvFilePath);
                dataGridView1.DataSource = table;

                // Dosyanýn deðiþtirilme zamanýný güncelle
                lastModifiedTime = File.GetLastWriteTime(csvFilePath);
            }
            else
            {
                MessageBox.Show("CSV dosyasý bulunamadý. Lütfen dosya yolunu kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoadCsvIntoDataTable(string filePath)
        {
            DataTable dataTable = new DataTable();

            try
            {
                // CSV dosyasýný oku
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    // Baþlýk satýrýný (kolon isimleri) ekle
                    string[] headers = lines[0].Split(',');
                    foreach (string header in headers)
                    {
                        if (header == "Tarih")
                            dataTable.Columns.Add(header, typeof(DateTime)); // Tarih sütunu için DateTime türü
                        else if (header == "Saat")
                            dataTable.Columns.Add(header, typeof(TimeSpan)); // Saat sütunu için TimeSpan türü
                        else
                            dataTable.Columns.Add(header, typeof(string)); // Diðer sütunlar için string türü
                    }

                    // Satýrlarý ekle
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] data = lines[i].Split(',');
                        DataRow row = dataTable.NewRow();

                        for (int j = 0; j < headers.Length; j++)
                        {
                            if (headers[j] == "Tarih")
                                row[j] = DateTime.Parse(data[j]); // Tarih sütununu DateTime olarak ekle
                            else if (headers[j] == "Saat")
                                row[j] = TimeSpan.Parse(data[j]); // Saat sütununu TimeSpan olarak ekle
                            else
                                row[j] = data[j]; // Diðer sütunlarý string olarak ekle
                        }

                        dataTable.Rows.Add(row);
                    }

                    // Tarih ve saate göre sýralama (en güncel en üstte)
                    dataTable.DefaultView.Sort = "Tarih DESC, Saat DESC";
                    dataTable = dataTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV dosyasý okunurken bir hata oluþtu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        // DataGridView'i güncelle
                        Invoke(new Action(() =>
                        {
                            button1.PerformClick(); // Grid'i yeniden yükle
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Scheduler hatasý: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
