# 💱 Currency Tracker

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black)

**Современное решение для отслеживания курсов валют ЦБ РФ**

</div>

## 🚀 Обзор

Currency Tracker - это комплексное .NET решение, которое предоставляет:
- **Автоматический сбор данных** из API ЦБ РФ
- **RESTful API** с JWT аутентификацией
- **Курсы валют в реальном времени** с историческими данными
- **Генерацию QR-кодов** для текущих курсов

- ### Технологический стек
- **Бэкенд**: .NET 8.0, ASP.NET Core, Entity Framework Core
- **База данных**: PostgreSQL
- **Аутентификация**: JWT Bearer Tokens
- **Документация API**: Swagger/OpenAPI
- **Генерация QR-кодов**: JavaScript + qrcode-generator

## ✨ Возможности

### 🔄 Сбор данных
- Автоматический ежедневный сбор курсов валют из ЦБ РФ
- Настраиваемая загрузка исторических данных (N дней назад)
- Умная система обновления с дедупликацией данных

### 🌐 REST API
- `GET /currencies` - Список валют с фильтрацией и пагинацией
- `GET /currency/{code}` - Последний курс для конкретной валюты
- `GET /currency/qrcbr` - QR-код ссылающийся на текущие курсы ЦБ
- **JWT Bearer Authentication** для безопасного доступа

### 📊 Управление данными
- База данных PostgreSQL с оптимизированной схемой
- Entity Framework Core для доступа к данным
- Паттерн Repository для чистой абстракции данных

## 🛠️ Установка и настройка

### Предварительные требования
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) 16+
- [Git](https://git-scm.com/)

### Быстрый старт

1. **Клонируйте репозиторий**
```
   git clone https://github.com/SurmDen/CurrencyTracker.git
   cd /CurrencyTracker/CurrencyTrackerSolution
```

2. **Настройте подключение к базе данных**
```
  # Отредактируйте appsettings.json с вашими данными PostgreSQL
```

3. **Запустите приложение**
```
  # Терминал 1 - Запуск API
  cd CurrencyTracker.API
  dotnet run
  
  # Терминал 2 - Запуск загрузчика данных
  cd CurrencyTracker.Console
  dotnet run
```

**Использование API**
```
# Получение JWT токена
curl -X POST https://localhost:5001/login \
  -H "Content-Type: application/json"

# Регистрация пользователя
curl -X POST https://localhost:5001/user/create \
  -H "Content-Type: application/json"

# Получить все валюты на конкретную дату и (или) валюту
curl -X GET "https://localhost:5001/api/currencies?date=15/10/2024&valute=USD" \
  -H "Authorization: Bearer ваш-jwt-токен"

# Получить конкретную валюту (USD)
curl -X GET "https://localhost:5001/api/currencies/USD" \
  -H "Authorization: Bearer ваш-jwt-токен"

# Получить страницу с QR-кодом
curl -X GET "https://localhost:5001/currency/qrcbr"
  -H "Authorization: Bearer ваш-jwt-токен"
```
