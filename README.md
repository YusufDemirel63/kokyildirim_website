# MAK Akademi — ASP.NET Core 10 MVC

Muhammet Ali Kökyıldırım'ın matematik özel ders platformu.

## Teknoloji Yığını

- **Framework:** ASP.NET Core 10 MVC
- **Dil:** C#
- **Veritabanı:** SQL Server 2025 Express
- **ORM:** Entity Framework Core 9
- **Auth:** ASP.NET Core Identity

## Proje Yapısı

```
MakAkademi/
├── Areas/
│   └── Admin/              # Yönetim paneli (Area)
│       ├── Controllers/    # Dashboard, Blog, Rezervasyon, Yorumlar, Mesajlar, ZamanDilimleri
│       └── Views/
├── Controllers/            # Public: Home, About, Lessons, Blog, Contact, Reservation, Testimonials
├── Data/                   # ApplicationDbContext (EF Core)
├── Middleware/             # PageViewMiddleware (ziyaretçi istatistikleri)
├── Migrations/             # EF Core migration dosyaları
├── Models/
│   ├── Entities/           # Veritabanı varlıkları
│   └── ViewModels/         # Razor view modelleri
├── Services/               # İş mantığı katmanı (DI)
│   └── Interfaces/
├── Views/                  # Razor (.cshtml) view'ları
│   ├── Shared/             # _Layout.cshtml, _AdminLayout.cshtml
│   ├── Home, About, Blog, Contact, Lessons, Reservation, Testimonials
└── wwwroot/                # CSS, JS, görseller
```

## Kurulum

### Gereksinimler
- .NET 10 SDK
- SQL Server 2025 Express (veya üzeri)

### Adımlar

1. Bağlantı string'ini yapılandırın (`appsettings.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MakAkademi;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. Migration'ı uygulayın (ilk çalıştırmada otomatik yapılır):
   ```bash
   cd MakAkademi
   dotnet ef database update
   ```

3. Uygulamayı başlatın:
   ```bash
   dotnet run
   ```

4. Admin hesabı oluşturun (ilk kurulumda):
   ```
   https://localhost:7100/setup-admin?username=admin&email=admin@makakademi.com&password=Admin1234!&secret=mak2024guvenlik
   ```

5. Admin paneline giriş yapın:
   ```
   https://localhost:7100/yonetim/giris
   ```

## Özellikler

- **Public:** Ana sayfa, Hakkımda, Dersler, Blog, Yorumlar, İletişim, Rezervasyon
- **Admin Paneli:** Dashboard (istatistikler), Zaman Dilimleri CRUD, Rezervasyon yönetimi, Blog CRUD, Yorum yönetimi, Mesaj listesi
- **Takvim Sistemi:** JavaScript tabanlı interaktif takvim, API üzerinden müsait slotları çeker
- **Ziyaretçi Sayacı:** PageViewMiddleware ile her sayfa ziyareti kaydedilir
