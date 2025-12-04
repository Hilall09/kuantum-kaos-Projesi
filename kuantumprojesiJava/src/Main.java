import java.util.*;

// ABSTRACT CLASS: Tüm nesnelerin temel yapısı
abstract class KuantumNesnesi {
    private final String id;  // final = sadece constructor'da atanır
    private double stabilite;
    protected int tehlike;

    public KuantumNesnesi(String id, double stab, int tehlike) {
        this.id = id;
        setStabilite(stab);
        this.tehlike = tehlike;
    }

    // Encapsulation: Getter/Setter ile kontrollü erişim
    public final void setStabilite(double s) {
        stabilite = Math.max(0, Math.min(100, s));
    }

    public double getStabilite() {
        return stabilite;
    }

    public String getId() {
        return id;
    }

    // Abstract metot: Alt sınıflar override edecek
    public abstract void analizEt();

    public String durumBilgisi() {
        return String.format("[%s...] Stabilite: %.1f%% | Tehlike: %d",
                id.substring(0, 8), stabilite, tehlike);
    }
}

// INTERFACE: Kritik nesneler için soğutma yeteneği
interface IKritik {
    void acilDurumSogutmasi();
}

// CONCRETE CLASS 1: Güvenli veri
class VeriPaketi extends KuantumNesnesi {
    public VeriPaketi(String id) {
        super(id, 100, 1);
    }

    @Override
    public void analizEt() {
        setStabilite(getStabilite() - 5);
        System.out.println("Veri içeriği okundu.");
    }
}

// CONCRETE CLASS 2: Karanlık madde (implements IKritik)
class KaranlikMadde extends KuantumNesnesi implements IKritik {
    public KaranlikMadde(String id) {
        super(id, 100, 7);
    }

    @Override
    public void analizEt() {
        setStabilite(getStabilite() - 15);
        System.out.println(" Karanlık madde analiz edildi.");
    }

    @Override
    public void acilDurumSogutmasi() {
        setStabilite(Math.min(100, getStabilite() + 50));
        System.out.println("Soğutma aktif! +50 stabilite");
    }
}

// CONCRETE CLASS 3: Anti madde (implements IKritik)
class AntiMadde extends KuantumNesnesi implements IKritik {
    public AntiMadde(String id) {
        super(id, 100, 10);
    }

    @Override
    public void analizEt() {
        System.out.println(" Evrenin dokusu titriyor...");
        setStabilite(getStabilite() - 25);
    }

    @Override
    public void acilDurumSogutmasi() {
        setStabilite(Math.min(100, getStabilite() + 50));
        System.out.println(" ACİL SOĞUTMA +50 stabilite");
    }
}

// CUSTOM EXCEPTION
class KuantumCokusuException extends Exception {
    public KuantumCokusuException(String id) {
        super("KUANTUM ÇÖKÜŞÜ Nesne patladı: " + id);
    }
}

public class Main {
    public static void main(String[] args) {
        ArrayList<KuantumNesnesi> envanter = new ArrayList<>();
        Scanner scan = new Scanner(System.in);
        Random rnd = new Random();

        try {
            OUTER:
            while (true) {
                System.out.println("KUANTUM AMBARI KONTROL PANELİ");
                System.out.println("1. Yeni Nesne Ekle");
                System.out.println("2. Envanteri Listele");
                System.out.println("3. Nesneyi Analiz Et");
                System.out.println("4. Acil Durum Soğutması");
                System.out.println("5. Çıkış");
                System.out.print("\nSeçiminiz: ");
                String s = scan.nextLine();
                switch (s) {
                    case "1" -> {
                        // Polimorfizm: Rastgele somut sınıf üretimi
                        int t = rnd.nextInt(3);
                        String id = UUID.randomUUID().toString();
                        KuantumNesnesi yeni = switch (t) {
                            case 0 -> new VeriPaketi(id);
                            case 1 -> new KaranlikMadde(id);
                            default -> new AntiMadde(id);
                        };  envanter.add(yeni);
                        System.out.println("✓ " + yeni.getClass().getSimpleName() + " eklendi.");
                    }
                    case "2" -> {
                        System.out.println("\n ENVANTER (" + envanter.size() + " nesne) ");
                        for (var nesne : envanter) {
                            System.out.println(nesne.durumBilgisi());
                        }
                    }
                    case "3" ->                         {
                            System.out.print("Nesne ID (ilk 8 karakter): ");
                            String aramaID = scan.nextLine();
                            KuantumNesnesi nesne = null;
                            for (var n : envanter) {
                                if (n.getId().startsWith(aramaID)) {
                                    nesne = n;
                                    break;
                                }
                            }       if (nesne != null) {
                                nesne.analizEt();
                                
                                if (nesne.getStabilite() <= 0) {
                                    throw new KuantumCokusuException(nesne.getId());
                                }
                            } else {
                                System.out.println(" Nesne bulunamadı!");
                            }                              }
                    case "4" ->                         {
                            System.out.print("Nesne ID (ilk 8 karakter): ");
                            String aramaID = scan.nextLine();
                            KuantumNesnesi nesne = null;
                            for (var n : envanter) {
                                if (n.getId().startsWith(aramaID)) {
                                    nesne = n;
                                    break;
                                }
                            }       if (nesne != null) {
                                // Type Checking: instanceof ile IKritik kontrolü
                                if (nesne instanceof IKritik iKritik) {
                                    iKritik.acilDurumSogutmasi();
                                } else {
                                    System.out.println(" Bu nesne soğutulamaz (IKritik değil)");
                                }
                            } else {
                                System.out.println(" Nesne bulunamadı!");
                            }                              }
                    case "5" -> {
                        System.out.println("Sistem kapatılıyor...");
                        break OUTER;
                    }
                    default -> {
                    }
                }
            }
        }
        catch (KuantumCokusuException e) {
            System.out.println("\n" + "=".repeat(50));
            System.out.println(" SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR... ");
            System.out.println(e.getMessage());
            System.out.println("=".repeat(50));
        }
        finally {
            scan.close();
        }
    }
}