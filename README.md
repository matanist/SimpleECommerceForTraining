# Simple E-Commerce API

Öğrenciler için tasarlanmış, ücretsiz ve açık kaynaklı bir e-ticaret REST API projesidir.

## Teknolojiler

- .NET 9 Web API
- Entity Framework Core 9
- SQL Server
- Swagger & OpenAPI Documentation
- AutoMapper
- FluentValidation
- JWT Authentication
- Repository Pattern
- Docker & Docker Compose

## Özellikler

- Kullanıcı Kayıt ve Giriş (JWT Token)
- Ürün Yönetimi (CRUD)
- Kategori Yönetimi
- Sipariş Yönetimi
- Rol Tabanlı Yetkilendirme (Admin/Customer)
- Sayfalama ve Filtreleme
- Hata Yönetimi

---

## Hızlı Başlangıç (Öğrenciler İçin)

Sadece `docker-compose.yml` dosyasını indirip çalıştırmanız yeterli!

### Adım 1: docker-compose.yml dosyasını indirin

Aşağıdaki içeriği `docker-compose.yml` olarak kaydedin:

```yaml
version: '3.8'

services:
  api:
    image: ghcr.io/matanist/simpleecommerce-api:latest
    container_name: simpleecommerce-api
    ports:
      - "5000:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=SimpleECommerceDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True
      - Jwt__Key=YourSuperSecretKeyThatIsAtLeast32CharactersLong!
      - Jwt__Issuer=SimpleECommerce
      - Jwt__Audience=SimpleECommerceUsers
      - Jwt__ExpirationHours=24
    depends_on:
      sqlserver:
        condition: service_healthy
    networks:
      - simpleecommerce-network
    restart: unless-stopped

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: simpleecommerce-db
    ports:
      - "1433:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Passw0rd
      - MSSQL_PID=Developer
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - simpleecommerce-network
    healthcheck:
      test: /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "SELECT 1" || exit 1
      interval: 10s
      timeout: 3s
      retries: 10
      start_period: 30s
    restart: unless-stopped

networks:
  simpleecommerce-network:
    driver: bridge

volumes:
  sqlserver-data:
```

### Adım 2: Docker Compose ile başlatın

```bash
docker-compose up -d
```

### Adım 3: API'ye erişin

Tarayıcınızda açın: **http://localhost:5000**

Swagger UI üzerinden tüm endpoint'leri görebilir ve test edebilirsiniz.

---

## Varsayılan Admin Kullanıcı

| Email | Şifre | Rol |
|-------|-------|-----|
| `admin@simpleecommerce.com` | `Admin123!` | Admin |

> Yeni kullanıcılar kayıt olduğunda otomatik olarak "Customer" rolü atanır.

---

## API Endpoints

### Auth (Kimlik Doğrulama)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| POST | `/api/auth/login` | Giriş yap | Herkese açık |
| POST | `/api/auth/register` | Kayıt ol | Herkese açık |
| GET | `/api/auth/me` | Mevcut kullanıcı bilgisi | Giriş gerekli |

### Categories (Kategoriler)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| GET | `/api/categories` | Tüm kategorileri listele | Herkese açık |
| GET | `/api/categories/{id}` | Kategori detayı | Herkese açık |
| POST | `/api/categories` | Yeni kategori ekle | Admin |
| PUT | `/api/categories/{id}` | Kategori güncelle | Admin |
| DELETE | `/api/categories/{id}` | Kategori sil | Admin |

### Products (Ürünler)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| GET | `/api/products` | Ürünleri listele | Herkese açık |
| GET | `/api/products/{id}` | Ürün detayı | Herkese açık |
| POST | `/api/products` | Yeni ürün ekle | Admin |
| PUT | `/api/products/{id}` | Ürün güncelle | Admin |
| DELETE | `/api/products/{id}` | Ürün sil | Admin |

### Orders (Siparişler)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| GET | `/api/orders` | Tüm siparişleri listele | Admin |
| GET | `/api/orders/my-orders` | Kendi siparişlerim | Giriş gerekli |
| GET | `/api/orders/{id}` | Sipariş detayı | Sahip/Admin |
| POST | `/api/orders` | Yeni sipariş oluştur | Giriş gerekli |
| PUT | `/api/orders/{id}/status` | Durumu güncelle | Admin |
| POST | `/api/orders/{id}/cancel` | Sipariş iptal | Sahip |

### Users (Kullanıcılar)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| GET | `/api/users` | Tüm kullanıcılar | Admin |
| GET | `/api/users/{id}` | Kullanıcı detayı | Sahip/Admin |
| PUT | `/api/users/{id}` | Kullanıcı güncelle | Sahip/Admin |
| DELETE | `/api/users/{id}` | Kullanıcı sil | Admin |

---

## Hazır Veriler

Uygulama başlatıldığında otomatik olarak şu veriler oluşturulur:

### Kategoriler
- Electronics, Clothing, Books, Home & Garden, Sports & Outdoors

### Örnek Ürünler
- Wireless Bluetooth Headphones, Smartphone, Laptop, T-Shirt, Jeans, Clean Code (Kitap), Yoga Mat ve daha fazlası...

---

## Proje Yapısı

```
SimpleECommerceForTraining/
├── src/
│   ├── SimpleECommerce.Core/           # Entity, DTO, Interface
│   ├── SimpleECommerce.DataAccess/     # DbContext, Repository
│   ├── SimpleECommerce.Business/       # Service, Validator, Mapper
│   └── SimpleECommerce.API/            # Controller, Middleware
├── docker-compose.yml                   # Öğrenciler için (GHCR image)
├── docker-compose.dev.yml              # Geliştirme için (local build)
└── README.md
```

---

## Geliştirici Kurulumu

Kaynak koddan çalıştırmak istiyorsanız:

```bash
# Projeyi klonlayın
git clone https://github.com/matanist/SimpleECommerceForTraining.git
cd SimpleECommerceForTraining

# Development compose ile başlatın
docker-compose -f docker-compose.dev.yml up -d --build
```

---

## Lisans

MIT License

## Katkıda Bulunma

Pull request'ler memnuniyetle karşılanır. Büyük değişiklikler için lütfen önce bir issue açın.
# Docker Hub Migration
