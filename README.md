![0modulith](https://github.com/user-attachments/assets/0f1f340e-6cb1-4bfd-aa05-f54109e5b865)

# EShop Modular Monolith

## Proje Açıklaması
Bu proje, güçlü, ölçeklenebilir ve sürdürülebilir uygulamalar geliştirmek amacıyla **Modüler Monolitik Mimari**, **Vertical Slice Architecture** ve **Domain Driven Design** (DDD) gibi modern mimari desenler kullanılarak geliştirilmiştir. Uygulama, modüller arası iletişimde güvenilir mesajlaşma için **Outbox Pattern** ve modüller arası senkron/asenkron iletişim için **RabbitMQ** kullanmaktadır.

## Özellikler
- **Modüller**:
  - **Catalog Modülü**: Asp.Net Core Minimal API'leri, DDD, CQRS, Entity Framework Core, PostgreSQL ve Serilog.
  - **Basket Modülü**: DDD, CQRS ve VSA ile Redis üzerinde cacheleme ve RabbitMQ üzerinden BasketCheckoutEvent için Outbox Pattern ile mesajlaşma.
  - **Identity Modülü**: **Keycloak** kullanarak modülleri güvence altına almak için **JwtBearer Token** entegrasyonu.
  - **Ordering Modülü**: **BasketCheckout** işlemi için **Outbox Pattern** ile mesajlaşma ve CQRS uygulamaları.

## Teknolojiler
- **.NET 8 ve C# 12**
- **Entity Framework Core** (Code-First, PostgreSQL)
- **RabbitMQ** ve **MassTransit** (Mesajlaşma)
- **Redis** (Cache)
- **Keycloak** (Kimlik doğrulama)
- **Serilog** (Loglama)
- **MediatR** (CQRS, DDD)
- **Carter** (Minimal API endpoints)
