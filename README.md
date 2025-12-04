Kuantum Kaos Yönetimi
Kuantum Kaos Yönetimi, evrenin en tehlikeli ve kararsız maddelerini dijital ortamda saklayan bir simülasyon projesidir. Oyuncu, Omega Sektörü'ndeki Kuantum Veri Ambarı'nın vardiya amiri olarak göreve başlar. Amaç; depoya gelen nesneleri kabul etmek, analiz etmek ve stabilite seviyeleri sıfıra düşmeden günü tamamlamaktır.

Oynanış
Depoya rastgele VeriPaketi, KaranlikMadde veya AntiMadde eklenir. Stabilite her analizde düşer. Tehlikeli nesneler için Acil Durum Soğutması yapılabilir. Stabilite %0’ın altına düşerse Kuantum Çöküşü gerçekleşir ve oyun biter.

Teknik Gereksinimler
Abstract Class & Encapsulation: KuantumNesnesi sınıfı tüm nesnelerin atasıdır.
Interface Segregation: Tehlikeli nesneler IKritik arayüzünü uygular.
Inheritance & Polymorphism:
VeriPaketi → güvenli, stabilite -5
KaranlikMadde → tehlikeli, stabilite -15, soğutma yapılabilir
AntiMadde → çok tehlikeli, stabilite -25, özel uyarı mesajı
Custom Exception: Stabilite sıfırlandığında KuantumCokusuException fırlatılır.

Game Over
Herhangi bir nesnenin stabilitesi %0’ın altına düştüğünde:
SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...
Amaç
Bu proje, OOP prensiplerini (abstract class, interface, inheritance, polymorphism, encapsulation, exception handling) uygulamalı olarak göstermek için tasarlanmıştır. Hem eğlenceli bir simülasyon sunar hem de nesne yönelimli programlamanın temel yapı taşlarını pekiştirir.
