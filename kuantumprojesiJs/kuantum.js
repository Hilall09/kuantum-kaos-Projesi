const readline = require("readline");
const { v4: uuidv4 } = require("uuid");

// CUSTOM EXCEPTION
class KuantumCokusuException extends Error {
    constructor(id) {
        super(`KUANTUM ÇÖKÜŞÜ! Nesne patladı: ${id}`);
        this.name = "KuantumCokusuException";
    }
}

// ABSTRACT BASE CLASS (JavaScript'te abstract simülasyonu)
class KuantumNesnesi {
    #id;  // Private field (ES2022)

    constructor(id, stab, tehlike) {
        if (new.target === KuantumNesnesi) {
            throw new Error("KuantumNesnesi soyut bir sınıftır!");
        }
        this.#id = id;
        this._stabilite = stab;
        this.tehlike = tehlike;
    }

    // Getter (ID sadece okunabilir)
    get id() {
        return this.#id;
    }

    // Encapsulation: Property ile 0-100 sınırlaması
    get stabilite() {
        return this._stabilite;
    }

    set stabilite(v) {
        this._stabilite = Math.max(0, Math.min(100, v));
    }

    // Abstract metot (override zorunlu)
    analizEt() {
        throw new Error("analizEt() metodu override edilmelidir!");
    }

    durumBilgisi() {
        return `[${this.id.substring(0, 8)}...] Stabilite: ${this.stabilite.toFixed(1)}% | Tehlike: ${this.tehlike}`;
    }
}

// INTERFACE simülasyonu (Symbol ile marker interface)
const IKritik = Symbol("IKritik");

// CONCRETE CLASS 1: Güvenli veri
class VeriPaketi extends KuantumNesnesi {
    constructor(id) {
        super(id, 100, 1);
    }

    analizEt() {
        this.stabilite -= 5;
        console.log(" Veri içeriği okundu.");
    }
}

// CONCRETE CLASS 2: Karanlık madde (IKritik marker)
class KaranlikMadde extends KuantumNesnesi {
    constructor(id) {
        super(id, 100, 7);
        this[IKritik] = true;  // Interface marker
    }

    analizEt() {
        this.stabilite -= 15;
        console.log(" Karanlık madde analiz edildi.");
    }

    acilDurumSogutmasi() {
        this.stabilite = Math.min(100, this.stabilite + 50);
        console.log(" Soğutma aktif! +50 stabilite");
    }
}

// CONCRETE CLASS 3: Anti madde (IKritik marker)
class AntiMadde extends KuantumNesnesi {
    constructor(id) {
        super(id, 100, 10);
        this[IKritik] = true;  // Interface marker
    }

    analizEt() {
        console.log("Evrenin dokusu titriyor...");
        this.stabilite -= 25;
    }

    acilDurumSogutmasi() {
        this.stabilite = Math.min(100, this.stabilite + 50);
        console.log(" ACİL SOĞUTMA! +50 stabilite");
    }
}

// Readline interface
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

function soru(prompt) {
    return new Promise(resolve => rl.question(prompt, resolve));
}

// MAIN LOOP
(async () => {
    const envanter = [];

    try {
        while (true) {
            console.log("KUANTUM AMBARI KONTROL PANELİ");
            console.log("1. Yeni Nesne Ekle");
            console.log("2. Envanteri Listele");
            console.log("3. Nesneyi Analiz Et");
            console.log("4. Acil Durum Soğutması");
            console.log("5. Çıkış");

            const s = await soru("\nSeçiminiz: ");

            if (s === "1") {
                // Polimorfizm: Rastgele somut sınıf
                const t = Math.floor(Math.random() * 3);
                const yeniId = uuidv4();

                let nesne;
                if (t === 0) nesne = new VeriPaketi(yeniId);
                else if (t === 1) nesne = new KaranlikMadde(yeniId);
                else nesne = new AntiMadde(yeniId);

                envanter.push(nesne);
                console.log(` ${nesne.constructor.name} eklendi.`);
            }
            else if (s === "2") {
                console.log(`\n ENVANTER (${envanter.length} nesne)`);
                envanter.forEach(x => console.log(x.durumBilgisi()));
            }
            else if (s === "3") {
                const aramaId = await soru("Nesne ID (ilk 8 karakter): ");
                const nesne = envanter.find(x => x.id.startsWith(aramaId.trim()));

                if (nesne) {
                    nesne.analizEt();

                    if (nesne.stabilite <= 0) {
                        throw new KuantumCokusuException(nesne.id);
                    }
                } else {
                    console.log(" Nesne bulunamadı");
                }
            }
            else if (s === "4") {
                const aramaId = await soru("Nesne ID (ilk 8 karakter): ");
                const nesne = envanter.find(x => x.id.startsWith(aramaId.trim()));

                if (nesne) {
                    // Type Checking: Symbol marker ile IKritik kontrolü
                    if (nesne[IKritik]) {
                        nesne.acilDurumSogutmasi();
                    } else {
                        console.log(" Bu nesne soğutulamaz! (IKritik değil)");
                    }
                } else {
                    console.log(" Nesne bulunamadı");
                }
            }
            else if (s === "5") {
                console.log("Sistem kapatılıyor");
                rl.close();
                process.exit(0);
            }
        }
    }
    catch (error) {
        if (error instanceof KuantumCokusuException) {
            console.log("\n" + "=".repeat(50));
            console.log(" SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR");
            console.log(error.message);
            console.log("=".repeat(50));
        } else {
            console.error("Beklenmeyen hata:", error);
        }
        rl.close();
        process.exit(1);
    }
})();