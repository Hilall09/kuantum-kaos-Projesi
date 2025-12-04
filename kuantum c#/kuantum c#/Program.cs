// See https://aka.ms/new-console-template for more information

// ABSTRACT CLASS: Tüm kuantum nesnelerinin atası
public abstract class KuantumNesnesi
{
    // ID sadece okunabilir (getter), dışarıdan değiştirilemez
    public string ID { get; private set; }

    // Encapsulation: Stabilite değeri 0-100 arasında sınırlandırılır
    private double stabilite;
    public double Stabilite
    {
        get => stabilite;
        set => stabilite = Math.Max(0, Math.Min(100, value));
    }

    public int TehlikeSeviyesi { get; protected set; }

    public KuantumNesnesi(string id, double stab, int tehlike)
    {
        ID = id;
        Stabilite = stab;
        TehlikeSeviyesi = tehlike;
    }

    // Abstract metot: Her alt sınıf kendi davranışını tanımlar
    public abstract void AnalizEt();

    public string DurumBilgisi()
    {
        return $"[{ID.Substring(0, 8)}...] Stabilite: {Stabilite:F1}% | Tehlike: {TehlikeSeviyesi}";
    }
}

// INTERFACE: Sadece kritik nesneler için soğutma yeteneği
public interface IKritik
{
    void AcilDurumSogutmasi();
}

// CONCRETE CLASS 1: Güvenli veri paketi
public class VeriPaketi : KuantumNesnesi
{
    public VeriPaketi(string id) : base(id, 100, 1) { }

    public override void AnalizEt()
    {
        Stabilite -= 5;
        Console.WriteLine("Veri içeriği okundu.");
    }
}

// CONCRETE CLASS 2: Tehlikeli karanlık madde (IKritik uygular)
public class KaranlikMadde : KuantumNesnesi, IKritik
{
    public KaranlikMadde(string id) : base(id, 100, 7) { }

    public override void AnalizEt()
    {
        Stabilite -= 15;
        Console.WriteLine("Karanlık madde analiz edildi.");
    }

    public void AcilDurumSogutmasi()
    {
        Stabilite = Math.Min(100, Stabilite + 50);
        Console.WriteLine("❄ Soğutma sistemi aktif! +50 stabilite");
    }
}

// CONCRETE CLASS 3: Çok tehlikeli anti madde (IKritik uygular)
public class AntiMadde : KuantumNesnesi, IKritik
{
    public AntiMadde(string id) : base(id, 100, 10) { }

    public override void AnalizEt()
    {
        Console.WriteLine(" Evrenin dokusu titriyor...");
        Stabilite -= 25;
    }

    public void AcilDurumSogutmasi()
    {
        Stabilite = Math.Min(100, Stabilite + 50);
        Console.WriteLine(" ACİL SOĞUTMA! +50 stabilite");
    }
}

// CUSTOM EXCEPTION: Stabilite 0'a düştüğünde fırlatılır
public class KuantumCokusuException : Exception
{
    public KuantumCokusuException(string id)
        : base($"KUANTUM ÇÖKÜŞÜ! Nesne patladı: {id}")
    { }
}

class Program
{
    static void Main()
    {
        // Generic List ile polimorfik nesne saklama
        List<KuantumNesnesi> envanter = new List<KuantumNesnesi>();
        Random rnd = new Random();

        try
        {
            while (true)
            {
 
                Console.WriteLine("KUANTUM AMBARI KONTROL PANELİ");

                Console.WriteLine("1. Yeni Nesne Ekle (Rastgele)");
                Console.WriteLine("2. Tüm Envanteri Listele");
                Console.WriteLine("3. Nesneyi Analiz Et");
                Console.WriteLine("4. Acil Durum Soğutması");
                Console.WriteLine("5. Çıkış");
                Console.Write("\nSeçiminiz: ");

                string secim = Console.ReadLine();

                if (secim == "1")
                {
                    // Polimorfizm: Rastgele somut sınıf üretimi
                    int t = rnd.Next(3);
                    string id = Guid.NewGuid().ToString();

                    KuantumNesnesi yeniNesne = t switch
                    {
                        0 => new VeriPaketi(id),
                        1 => new KaranlikMadde(id),
                        _ => new AntiMadde(id)
                    };

                    envanter.Add(yeniNesne);
                    Console.WriteLine($"✓ {yeniNesne.GetType().Name} üretildi.");
                }
                else if (secim == "2")
                {
                    Console.WriteLine($"\n--- ENVANTER ({envanter.Count} nesne) ---");
                    foreach (var nesne in envanter)
                    {
                        // Polimorfizm: Her nesne kendi DurumBilgisi metodunu çağırır
                        Console.WriteLine(nesne.DurumBilgisi());
                    }
                }
                else if (secim == "3")
                {
                    Console.Write("Nesne ID (ilk 8 karakter): ");
                    string aramaID = Console.ReadLine();

                    // LINQ ile arama
                    var nesne = envanter.FirstOrDefault(x => x.ID.StartsWith(aramaID));

                    if (nesne != null)
                    {
                        nesne.AnalizEt();

                        // Stabilite kontrolü
                        if (nesne.Stabilite <= 0)
                            throw new KuantumCokusuException(nesne.ID);
                    }
                    else
                    {
                        Console.WriteLine(" Nesne bulunamadı!");
                    }
                }
                else if (secim == "4")
                {
                    Console.Write("Nesne ID (ilk 8 karakter): ");
                    string aramaID = Console.ReadLine();

                    var nesne = envanter.FirstOrDefault(x => x.ID.StartsWith(aramaID));

                    if (nesne != null)
                    {
                        // Type Checking: IKritik interface'ini uygulayıp uygulamadığını kontrol et
                        if (nesne is IKritik kritikNesne)
                        {
                            kritikNesne.AcilDurumSogutmasi();
                        }
                        else
                        {
                            Console.WriteLine(" Bu nesne soğutulamaz! (IKritik değil)");
                        }
                    }
                    else
                    {
                        Console.WriteLine(" Nesne bulunamadı!");
                    }
                }
                else if (secim == "5")
                {
                    Console.WriteLine("Sistem kapatılıyor...");
                    break;
                }
            }
        }
        catch (KuantumCokusuException ex)
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine(" SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR");
            Console.WriteLine(ex.Message);
            Console.WriteLine(new string('=', 50));
        }
    }
}