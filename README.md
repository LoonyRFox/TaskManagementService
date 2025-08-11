# Task Management & Log Notification Services

Данный проект реализует два микросервиса для управления задачами и логирования уведомлений с использованием .NET, PostgreSQL и Kafka. Сервисы построены по принципам чистой архитектуры и контейнеризованы с помощью Docker для удобства развертывания и масштабирования.

- **Task Management Service** — сервис для управления задачами пользователя, включает регистрацию, обновление и удаление задач. Имеет  реализованный паттерн Outbox для надежной доставки событий.
## Основные возможности

- Надежное сохранение событий в Outbox в рамках транзакций базы данных
- Гибкая доставка сообщений через Kafka или HTTP в зависимости от конфигурации
- Фоновая обработка Outbox сообщений с повторными попытками при ошибках

---

## Конфигурация Outbox

В `appsettings.json` задается режим доставки сообщений и параметры для Kafka или HTTP.

```json
"Outbox": {
  "DeliveryMode": "Kafka", // Возможные значения: "Kafka" или "Http"
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    //"SaslMechanism": "Plain",
    //"SecurityProtocol": "SaslPlaintext",
    "NotificationProducerOptions": {
      "ClientId": "Notification",
      "Topic": "service_notifications_push_main_topic",
      "RetryOnFailedDelayMs": 3000
    }
  },
  "Http": {
    "BaseUrl": "http://localhost:5022",
    "CreateLogRecordEndpoint": "/api/logs/create"
  }
}
```

- **Task Management Service Loging** — сервис для логирования уведомлений о событиях в системе, поддерживает асинхронное получение сообщений через Kafka.

Вместе с инфраструктурой (PostgreSQL, Kafka и Zookeeper) сервисы обеспечивают надежный и масштабируемый механизм обработки данных и событий.

---

## 🛠 Запуск через Docker Compose

В корне проекта находится `docker-compose.yml`, который поднимает следующие сервисы:

- **PostgreSQL** — база данных для сервисов.
- **Zookeeper** — координационный сервис для Kafka.
- **Kafka** — брокер сообщений.
- **TaskManagementServiceLoging** — сервис логирования уведомлений.
- **TaskManagementService** — сервис управления задачами.

---
## 📦 Технологии

- **.NET 8**
- **Entity Framework Core** (Code First + Migrations)
- **Kafka**
- **REST API**
- **PostgreSQL**
- **Docker** (для контейнеризации)
- **ASP.NET Core Web API**
- **Automapper**
- **FluentValidation**
- **Refit**
- **Serilog**
- **Swagger**

## 📁 Структура проектов

### TaskManagementServiceLogging
- **TaskManagementServiceLogging.Domain** — Доменные модели и бизнес-логика
- **TaskManagementServiceLogging.Application** — Логика приложения, валидация, CQRS, MediatR, сервисы
- **TaskManagementServiceLogging.Infrastructure** — Инфраструктура: доступ к данным, интеграция с Kafka, другие внешние сервисы
- **TaskManagementServiceLogging** — Точка входа: WebAPI, конфигурация, контроллеры

---

### TaskManagementService
- **TaskManagementService.Domain** — Доменные модели и бизнес-логика
- **TaskManagementService.Application** — Бизнес-логика, сервисы, валидация, CQRS, MediatR
- **TaskManagementService.Messaging** — Компоненты работы с сообщениями (Kafka, RabbitMQ, и т.п.)
- **TaskManagementService.Persistence** — Работа с базой данных: EF Core, миграции, репозитории
- **TaskManagementService.Identity** — Управление пользователями, аутентификация, авторизация
- **TaskManagementService.FunctionalTest** — Функциональные тесты
- **TaskManagementService** — Точка входа: WebAPI, контроллеры, конфигурация

---

### Дополнительные проекты
- **TaskManagementService.Resources** — Ресурсы, локализация, статические файлы 

## 🛠 Установка и запуск

### 🧪 Docker Compose
- Перейти в корневую директорию, где содержится файл docker-compose.yml
- Выполнить команды
```bash
docker-compose build
docker-compose up
```
### Порядок работы
  1. Перейти в браузре на страницу http://localhost:5250/swagger
  2. Выполнить запрос POST /api/v1/Account/Start 
     -- Будет создан случайный пользователь и сгенерирован BjwToken
```json
{
  "data": {
    "id": "01989751-f6eb-7d0b-b707-9a87fb8ef745",
    "userName": "pLIpWTw",
    "email": null,
    "roles": [],
    "isVerified": false,
    "jwToken": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjAxOTg5NzUxLWY2ZWItN2QwYi1iNzA3LTlhODdmYjhlZjc0NSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJwTElwV1R3IiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJLWFhWNDRHR1RVVkxJM1dOV0YyREhEVjU1NlVDQVJXTiIsImV4cCI6MTc1NDg4NjI4OCwiaXNzIjoiQ29yZUlkZW50aXR5IiwiYXVkIjoiQ29yZUlkZW50aXR5VXNlciJ9.A5B8kowJn5mTH8y_z6FbIfW29pQTqqi12vVlrI0rRiU"
  },
  "success": true,
  "errors": null
}
```
  3. Авторизоваться при помощи полученного токена
  4. Можно выполнять действия  с Task (Добавление, Изменение, Удаление, Извлечение) 😊
## docker-compose.yml

```yaml
version: '3.9'

services:
  # PostgreSQL
  postgres:
    image: postgres:16
    container_name: postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: pgSte
    ports:
      - "5434:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - backend

  # Kafka + Zookeeper (confluent)
  zookeeper:
    image: confluentinc/cp-zookeeper:7.5.0
    container_name: zookeeper
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000
    ports:
      - "2181:2181"
    networks:
      - backend

  kafka:
    image: confluentinc/cp-kafka:7.5.0
    container_name: kafka
    depends_on:
      - zookeeper
    ports:
      - "9092:9092"
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:9092,PLAINTEXT_HOST://localhost:9092
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
    networks:
      - backend

  # Log Service
  log-service:
    build:
      context: ./TaskManagementServiceLoging
      dockerfile: Dockerfile
    container_name: log-service
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      DatabaseOptions__Host: postgres
      DatabaseOptions__Port: 5432
      DatabaseOptions__Name: log-service
      DatabaseOptions__User: postgres
      DatabaseOptions__Password: pgSte
      Kafka__BootstrapServers: kafka:9092
    ports:
      - "5258:80"
    depends_on:
      - postgres
      - kafka
    networks:
      - backend

  # Task Service
  task-service:
    build:
      context: ./TaskManagementService
      dockerfile: Dockerfile
    container_name: task-service
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      DatabaseOptions__Host: postgres
      DatabaseOptions__Port: 5432
      DatabaseOptions__Name: task-service
      DatabaseOptions__User: postgres
      DatabaseOptions__Password: pgSte
      IdentityDatabaseOptions__Host: postgres
      IdentityDatabaseOptions__Port: 5432
      IdentityDatabaseOptions__Name: identity-db
      IdentityDatabaseOptions__User: postgres
      IdentityDatabaseOptions__Password: pgSte
      Kafka__BootstrapServers: kafka:9092
    ports:
      - "5250:80"
    depends_on:
      - postgres
      - kafka
    networks:
      - backend

volumes:
  postgres_data:

networks:
  backend:
    driver: bridge
