# VeriketApp

## Proje Hakkında
Veriket, loglama ve veri yönetimi süreçlerini optimize etmek için geliştirilmiş bir Windows uygulamasıdır. Proje, hem bir Windows servisi hem de kullanıcı dostu bir arayüz sunan WinForm uygulamasını içermektedir. Bu uygulama, loglama işlemlerini yönetmekle kalmaz, aynı zamanda kullanıcıya dinamik ve otomatik veri güncellemeleri sağlar.

---

## Özellikler
1. **Windows Servisi ile Loglama:**
   - Her dakika oluşturulan logları, `VeriketAppTest.txt` isimli bir dosyada saklar.
   - Dosyada şu bilgiler bulunur:
     - Tarih
     - Bilgisayar Adı
     - Kullanıcı Adı

2. **Dinamik Veri Güncelleme:**
   - **Ekstra Zamanlanmış Görev:** 
     - Her 10 saniyede bir çalışan zamanlanmış görev, log dosyasındaki güncel veriyi kontrol eder.
     - En son eklenen veriyi otomatik olarak gridin en üstüne taşır.
     - Kullanıcının herhangi bir butona basmasına gerek kalmaz.

3. **Kullanıcı Dostu Arayüz:**
   - Logları görüntülemek ve analiz etmek için kolay anlaşılır bir WinForm uygulaması.

4. **Özel Kurulum Süreci:**
   - MSI paketiyle kolay kurulum.
   - Program dosyaları `Program Files` dizininde, log dosyaları ise `ProgramData` altında saklanır.
   - Dinamik dosya yolları ile herhangi bir sürücüde çalıştırılabilir.

---

## Kurulum
1. MSI paketini indirip çalıştırarak uygulamayı kurun.
2. Kurulum tamamlandıktan sonra Windows servisini başlatın.
3. WinForm uygulamasını açarak logları görüntüleyin.

---

## Kullanım
- **Loglama:** Servis, arka planda her dakika log üretir ve ilgili dosyaya kaydeder.
- **Grid Güncelleme:** Her 10 saniyede bir çalışan görev sayesinde, log dosyasındaki en güncel bilgi otomatik olarak ekrana yansıtılır.

---

## Teknik Detaylar
- **Proje Yapısı:**
  - **Windows Service:** Logların üretilmesi ve dosyaya kaydedilmesi.
  - **WinForm Uygulaması:** Log dosyasını analiz etmek ve görselleştirmek için kullanıcı arayüzü.
  
- **Dosya Konumları:**
  - Uygulama: `C:\Program Files\VeriketApp`
  - Log Dosyası: `C:\ProgramData\VeriketApp\VeriketAppTest.txt`

- **Kodlama Prensipleri:**
  - Statik dosya yolları kullanılmaz. Dosya yolları dinamik olarak alınır.
  - Servis, kurulum yapan kullanıcının bilgilerini otomatik algılar.

---

## Örnek Senaryo
1. Servis başlatıldığında, her dakika bir log kaydı oluşturulur.
2. Log kaydı CSV formatta bir satır ekler:
3. 10 saniyelik zamanlanmış görev, dosyadaki en yeni veriyi gridin üstüne taşır.
4. Kullanıcı, gridde en güncel veriyi otomatik olarak görür.

---


## İletişim
Proje hakkında sorularınız veya önerileriniz için benimle iletişime geçebilirsiniz:
- **İsim:** İrem İçyer
- **E-posta:** iremicyer@gmail.com
- **GitHub:** [github.com/iremciyer](https://github.com/iremciyer)
