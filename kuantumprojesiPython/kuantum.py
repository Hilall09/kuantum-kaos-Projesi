import uuid
import random
from abc import ABC, abstractmethod

# CUSTOM EXCEPTION
class KuantumCokusuException(Exception):
    def __init__(self, id):
        super().__init__(f"KUANTUM ÇÖKÜŞÜ! Nesne patladı: {id}")

# ABSTRACT BASE CLASS (ABC kullanımı)
class KuantumNesnesi(ABC):
    def __init__(self, id, stab, tehlike):
        self.__id = id  # Private attribute (name mangling)
        self._stabilite = stab
        self.tehlike = tehlike

    # Getter (ID sadece okunabilir)
    @property
    def id(self):
        return self.__id

    # Property ile Encapsulation (0-100 sınırlaması)
    @property
    def stabilite(self):
        return self._stabilite

    @stabilite.setter
    def stabilite(self, v):
        self._stabilite = max(0, min(100, v))

    # Abstract metot (zorunlu override)
    @abstractmethod
    def analiz_et(self):
        pass

    def durum_bilgisi(self):
        return f"[{self.id[:8]}...] Stabilite: {self.stabilite:.1f}% | Tehlike: {self.tehlike}"

# INTERFACE benzeri sınıf (Duck Typing)
class IKritik:
    @abstractmethod
    def acil_sogutma(self):
        pass

# CONCRETE CLASS 1: Güvenli veri
class VeriPaketi(KuantumNesnesi):
    def __init__(self, id):
        super().__init__(id, 100, 1)

    def analiz_et(self):
        self.stabilite -= 5
        print("Veri içeriği okundu.")

# CONCRETE CLASS 2: Karanlık madde (Multiple Inheritance)
class KaranlikMadde(KuantumNesnesi, IKritik):
    def __init__(self, id):
        super().__init__(id, 100, 7)

    def analiz_et(self):
        self.stabilite -= 15
        print("Karanlık madde analiz edildi.")

    def acil_sogutma(self):
        self.stabilite = min(100, self.stabilite + 50)
        print("Soğutma aktif! +50 stabilite")

# CONCRETE CLASS 3: Anti madde (Multiple Inheritance)
class AntiMadde(KuantumNesnesi, IKritik):
    def __init__(self, id):
        super().__init__(id, 100, 10)

    def analiz_et(self):
        print("Evrenin dokusu titriyor")
        self.stabilite -= 25

    def acil_sogutma(self):
        self.stabilite = min(100, self.stabilite + 50)
        print(" ACİL SOĞUTMA! +50 stabilite")

# MAIN LOOP
envanter = []

try:
    while True:
        print(" KUANTUM AMBARI KONTROL PANELİ")
        print("1. Yeni Nesne Ekle")
        print("2. Envanteri Listele")
        print("3. Nesneyi Analiz Et")
        print("4. Acil Durum Soğutması")
        print("5. Çıkış")

        s = input("\nSeçiminiz: ").strip()

        if s == "1":
            # Polimorfizm: Rastgele somut sınıf
            t = random.randint(0, 2)
            yeni_id = str(uuid.uuid4())

            if t == 0:
                nesne = VeriPaketi(yeni_id)
            elif t == 1:
                nesne = KaranlikMadde(yeni_id)
            else:
                nesne = AntiMadde(yeni_id)

            envanter.append(nesne)
            print(f"✓ {nesne.__class__.__name__} oluşturuldu.")
        
        elif s == "2":
            print(f"\n--- ENVANTER ({len(envanter)} nesne) ---")
            for x in envanter:
                print(x.durum_bilgisi())

        elif s == "3":
            arama_id = input("Nesne ID (ilk 8 karakter): ").strip()
            
            nesne = next((x for x in envanter if x.id.startswith(arama_id)), None)
            
            if nesne:
                nesne.analiz_et()
                
                if nesne.stabilite <= 0:
                    raise KuantumCokusuException(nesne.id)
            else:
                print(" Nesne bulunamadı!")

        elif s == "4":
            arama_id = input("Nesne ID (ilk 8 karakter): ").strip()
            
            nesne = next((x for x in envanter if x.id.startswith(arama_id)), None)
            
            if nesne:
                # Type Checking: isinstance ile IKritik kontrolü
                if isinstance(nesne, IKritik):
                    nesne.acil_sogutma()
                else:
                    print(" Bu nesne soğutulamaz! (IKritik değil)")
            else:
                print(" Nesne bulunamadı!")

        elif s == "5":
            print("Sistem kapatılıyor...")
            break

except KuantumCokusuException as e:
    print("\n" + "=" * 50)
    print(" SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR... 🚨")
    print(str(e))
    print("=" * 50)
except KeyboardInterrupt:
    print("\n\nSistem zorla sonlandırıldı.")