using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Linq;
using System.Collections.Generic;

namespace VeriketService
{
    public class BatchService : ServiceBase
    {
        public const string ServiceNameConst = "VeriketAppService";

        public BatchService()
        {
            this.ServiceName = ServiceNameConst; // Servis adı sabitten alınır
        }

        private Timer timer;
        private string logDirectory;
        private string logFilePath;

        protected override void OnStart(string[] args)
        {
            try
            {
                // Log yazma işlemi
                File.AppendAllText("C:\\VeriketAppServiceDebug.txt", "OnStart çalıştı.\n");

                // Dinamik olarak ProgramData altındaki klasör yolunu al
                string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                logDirectory = Path.Combine(programDataPath, "VeriketApp");
                logFilePath = Path.Combine(logDirectory, "VeriketAppTest.txt");

                // Klasörü oluştur
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Timer ayarı
                timer = new Timer(60000); // 1 dakika
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = true;  // Timer'ın sürekli çalışmasını sağlar
                timer.Enabled = true;    // Timer'ı etkinleştirir
                timer.Start();           // Timer'ı başlat

                // Başarılı başlatma logu
                File.AppendAllText("C:\\VeriketAppServiceDebug.txt", "Timer başlatıldı.\n");
            }
            catch (Exception ex)
            {
                // Hata loglama
                File.AppendAllText("C:\\VeriketAppServiceDebug.txt", $"OnStart hatası: {ex.Message}{Environment.NewLine}");
            }
        }

        protected override void OnStop()
        {
            // Timer'ı durdur
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Log giriş bilgileri
                string logEntry = $"{DateTime.Now:yyyy-MM-dd},{DateTime.Now:HH:mm:ss},{Environment.MachineName},{Environment.UserName}";
                string logFilePath = Path.Combine(logDirectory, "VeriketAppTest.txt");

                // Eğer dosya yoksa başlık satırı ekle
                if (!File.Exists(logFilePath))
                {
                    File.WriteAllText(logFilePath, "Tarih,Saat,Bilgisayar Adı,Kullanıcı Adı\n");
                }

                // Tüm mevcut satırları oku ve yeni log girişini ekle
                List<string> allLines = File.ReadAllLines(logFilePath).ToList();

                // Başlığı çıkar
                string header = allLines.FirstOrDefault();
                allLines.RemoveAt(0);

                // Yeni log girişini listeye ekle
                allLines.Add(logEntry);

                // Tarihe ve saate göre sıralama (en güncel tarih ve saat en üstte)
                allLines = allLines
                    .OrderByDescending(line => DateTime.Parse(line.Split(',')[0]))
                    .ThenByDescending(line => TimeSpan.Parse(line.Split(',')[1]))
                    .ToList();

                // Başlığı ve sıralı satırları tekrar yaz
                allLines.Insert(0, header);
                File.WriteAllLines(logFilePath, allLines);

                // Debug logu
                File.AppendAllText("C:\\VeriketAppServiceDebug.txt", $"Log eklendi ve sıralandı: {logEntry}\n");
            }
            catch (Exception ex)
            {
                // Hata loglama
                EventLog.WriteEntry("Veriket Application Test", $"Log yazma hatası: {ex.Message}", EventLogEntryType.Error);
            }
        }
    }
}
