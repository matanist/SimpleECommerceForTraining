# Simple E-Commerce API - Kullanım Kılavuzu

## Hızlı Başlangıç

### Docker ile Çalıştırma (Önerilen)

```bash
# Projeyi klonlayın
git clone https://github.com/YOUR_USERNAME/SimpleECommerceForTraining.git
cd SimpleECommerceForTraining

# Docker Compose ile başlatın
docker-compose up -d

# API'ye erişin
# http://localhost:5000
```

### Manuel Kurulum

```bash
# SQL Server çalışıyor olmalı
cd src/SimpleECommerce.API
dotnet run
```

---

## Varsayılan Kullanıcılar

| Rol | Email | Şifre |
|-----|-------|-------|
| **Admin** | `admin@simpleecommerce.com` | `Admin123!` |

> Yeni kullanıcılar `/api/auth/register` endpoint'i ile kayıt olduğunda otomatik olarak **Customer** rolü atanır.

---

## Swagger UI

API çalıştıktan sonra tarayıcıda açın:
- **Docker:** http://localhost:5000
- **Manuel:** https://localhost:5001

Swagger UI üzerinden tüm endpoint'leri test edebilirsiniz.

---

## Authentication (Kimlik Doğrulama)

### 1. Giriş Yapma

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@simpleecommerce.com",
  "password": "Admin123!"
}
```

**Yanıt:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2024-01-02T12:00:00Z",
    "user": {
      "id": 1,
      "email": "admin@simpleecommerce.com",
      "firstName": "Admin",
      "lastName": "User",
      "role": "Admin"
    }
  }
}
```

### 2. Token Kullanımı

Korumalı endpoint'lere erişmek için `Authorization` header'ı ekleyin:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 3. Yeni Kullanıcı Kaydı

```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123!",
  "phoneNumber": "+90 555 123 4567",
  "address": "Istanbul, Turkey"
}
```

---

## API Endpoint'leri

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
| GET | `/api/products` | Ürünleri listele (sayfalama) | Herkese açık |
| GET | `/api/products/{id}` | Ürün detayı | Herkese açık |
| POST | `/api/products` | Yeni ürün ekle | Admin |
| PUT | `/api/products/{id}` | Ürün güncelle | Admin |
| DELETE | `/api/products/{id}` | Ürün sil | Admin |

**Ürün Listeleme Parametreleri:**
```
GET /api/products?pageNumber=1&pageSize=10&categoryId=1&searchTerm=laptop
```

### Orders (Siparişler)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| GET | `/api/orders` | Tüm siparişleri listele | Admin |
| GET | `/api/orders/my-orders` | Kendi siparişlerimi listele | Giriş gerekli |
| GET | `/api/orders/{id}` | Sipariş detayı | Sahip veya Admin |
| GET | `/api/orders/by-number/{orderNumber}` | Sipariş no ile ara | Sahip veya Admin |
| POST | `/api/orders` | Yeni sipariş oluştur | Giriş gerekli |
| PUT | `/api/orders/{id}/status` | Sipariş durumu güncelle | Admin |
| POST | `/api/orders/{id}/cancel` | Sipariş iptal et | Sahip (Pending durumunda) |

### Users (Kullanıcılar)
| Method | Endpoint | Açıklama | Yetki |
|--------|----------|----------|-------|
| GET | `/api/users` | Tüm kullanıcıları listele | Admin |
| GET | `/api/users/{id}` | Kullanıcı detayı | Sahip veya Admin |
| PUT | `/api/users/{id}` | Kullanıcı güncelle | Sahip veya Admin |
| DELETE | `/api/users/{id}` | Kullanıcı sil | Admin |

---

## Örnek İşlemler

### Sipariş Oluşturma

```http
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "shippingAddress": "Kadıköy, Istanbul",
  "notes": "Lütfen kapıya bırakın",
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 3, "quantity": 1 }
  ]
}
```

### Ürün Ekleme (Admin)

```http
POST /api/products
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "name": "Yeni Ürün",
  "description": "Ürün açıklaması",
  "price": 199.99,
  "stockQuantity": 50,
  "categoryId": 1,
  "imageUrl": "https://example.com/image.jpg"
}
```

---

## Sipariş Durumları

| Durum | Açıklama |
|-------|----------|
| `Pending` (0) | Beklemede |
| `Processing` (1) | İşleniyor |
| `Shipped` (2) | Kargoya verildi |
| `Delivered` (3) | Teslim edildi |
| `Cancelled` (4) | İptal edildi |

---

## Hazır Veriler (Seed Data)

### Kategoriler
1. Electronics - Elektronik cihazlar
2. Clothing - Giyim
3. Books - Kitaplar
4. Home & Garden - Ev ve bahçe
5. Sports & Outdoors - Spor ve outdoor

### Örnek Ürünler
- Wireless Bluetooth Headphones - $79.99
- Smartphone 128GB - $699.99
- Laptop 15.6 inch - $1199.99
- Men's Cotton T-Shirt - $19.99
- Clean Code (Kitap) - $39.99
- Yoga Mat - $24.99
- ve daha fazlası...

---

## Hata Yanıtları

Tüm hatalar aşağıdaki formatta döner:

```json
{
  "success": false,
  "message": "Hata açıklaması",
  "errors": ["Detaylı hata 1", "Detaylı hata 2"]
}
```

| HTTP Kodu | Açıklama |
|-----------|----------|
| 200 | Başarılı |
| 201 | Oluşturuldu |
| 400 | Geçersiz istek (validation hatası) |
| 401 | Yetkisiz (token gerekli) |
| 403 | Erişim reddedildi (yetki yok) |
| 404 | Bulunamadı |
| 500 | Sunucu hatası |

---

## Bağlantı Bilgileri

### Docker Ortamı
- **API:** http://localhost:5000
- **SQL Server:** localhost:1433
  - User: `sa`
  - Password: `YourStrong@Passw0rd`
  - Database: `SimpleECommerceDb`

### Geliştirme Ortamı
- **API (HTTP):** http://localhost:5000
- **API (HTTPS):** https://localhost:5001
