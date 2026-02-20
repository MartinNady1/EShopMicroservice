# 🛒 EShopMicroservice

A production-ready e-commerce application built with **.NET**, following **Microservices Architecture**, **Domain-Driven Design (DDD)**, **CQRS**, and **Event-Driven Communication** patterns. Each service is independently deployable, containerized with Docker, and communicates either via REST, gRPC, or RabbitMQ message broker.

---

## 📐 High-Level Architecture

```
                        ┌─────────────────────────────────────────────────┐
                        │                  Clients (Web/Mobile)           │
                        └──────────────────────┬──────────────────────────┘
                                               │
                        ┌──────────────────────▼──────────────────────────┐
                        │             API Gateway (YARP)                  │
                        │        Single entry point – routes &            │
                        │        aggregates requests to services          │
                        └───┬──────────┬──────────┬──────────┬────────────┘
                            │          │          │          │
               ┌────────────▼──┐  ┌────▼────┐  ┌─▼──────┐  ┌▼──────────┐
               │   Catalog      │  │ Basket  │  │Discount│  │ Ordering  │
               │  Microservice  │  │ Service │  │Service │  │  Service  │
               │  (PostgreSQL)  │  │ (Redis) │  │(SQLite)│  │(SQL Svr)  │
               └───────────────┘  └────┬────┘  └────▲───┘  └─────▲─────┘
                                       │  gRPC       │            │
                                       └─────────────┘            │
                                                                   │
                        ┌──────────────────────────────────────────┘
                        │           RabbitMQ (via MassTransit)
                        │     Basket publishes BasketCheckout event
                        │     Ordering consumes and creates Order
                        └──────────────────────────────────────────
```

---

## 🧩 Services Overview

### 1. 📦 Catalog Microservice

**Purpose:** Manages the product catalog — creating, reading, updating, and deleting products.

**Architecture Pattern:** Vertical Slice Architecture with CQRS (no traditional layered architecture)

**Internal Structure:**
- Features are organized by use case (each feature = one folder with Command/Query + Handler + Validator + Endpoint)
- Uses the Mediator pattern to decouple request handling from the API layer

**Technologies & Libraries:**

| Tool | Purpose |
|------|---------|
| ASP.NET Core (.NET 8) | Web API framework |
| Marten | Document database ORM on top of PostgreSQL |
| PostgreSQL | Primary database for product storage |
| MediatR | Mediator pattern — routes Commands/Queries to Handlers |
| FluentValidation | Input validation in pipeline behaviors |
| Carter | Minimal API endpoint organization |
| Mapster | Fast object-to-object mapping |
| Docker | Containerization |

**Database:** PostgreSQL (via Marten — used as a document store, not relational)

**Why PostgreSQL + Marten?** Marten turns PostgreSQL into a document/event store, giving the flexibility of NoSQL (document storage, querying) while keeping PostgreSQL's reliability and ACID compliance.

**Key Endpoints:**
- `GET /products` — Get all products (with pagination)
- `GET /products/{id}` — Get product by ID
- `GET /products/category/{category}` — Get products by category
- `POST /products` — Create product
- `PUT /products` — Update product
- `DELETE /products/{id}` — Delete product

---

### 2. 🛍️ Basket Microservice

**Purpose:** Manages the shopping cart. When a user adds items to their cart, the basket service also calls the Discount service (via gRPC) in real-time to apply any available discounts before saving.

**Architecture Pattern:** Repository Pattern + CQRS with MediatR

**Key Flow:**
1. User updates their basket
2. Basket service calls **Discount gRPC service** to get current discount rates
3. Final price is calculated and basket is saved to Redis
4. On checkout, a `BasketCheckoutEvent` is published to **RabbitMQ**
5. The basket is deleted from Redis after the event is published

**Technologies & Libraries:**

| Tool | Purpose |
|------|---------|
| ASP.NET Core (.NET 8) | Web API framework |
| Redis | In-memory cache for basket storage (fast reads/writes) |
| MassTransit | Abstraction layer over RabbitMQ for event publishing |
| RabbitMQ | Message broker for publishing `BasketCheckoutEvent` |
| gRPC / Grpc.Net.Client | Synchronous inter-service communication with Discount service |
| MediatR | CQRS mediator |
| Carter | Minimal API routing |
| Mapster | Object mapping |
| Docker | Containerization |

**Database:** Redis (in-memory key-value store)

**Why Redis?** Basket data is temporary, session-based, and needs extremely fast read/write performance. Redis is purpose-built for this — baskets are stored as key-value pairs with the user ID as the key.

**Key Endpoints:**
- `GET /basket/{userName}` — Get basket
- `POST /basket` — Store/Update basket (calls Discount gRPC internally)
- `DELETE /basket/{userName}` — Delete basket
- `POST /basket/checkout` — Checkout (publishes event to RabbitMQ)

---

### 3. 💰 Discount Microservice

**Purpose:** Provides discount/coupon data for products. Called synchronously by the Basket service via gRPC when calculating final prices.

**Architecture Pattern:** Simple CRUD with gRPC server

**Why gRPC?** The Basket service needs real-time, synchronous, high-performance communication to get discount data before saving a basket. gRPC is faster than REST (uses Protocol Buffers / binary serialization) and is ideal for internal service-to-service communication.

**Technologies & Libraries:**

| Tool | Purpose |
|------|---------|
| ASP.NET Core (.NET 8) | gRPC server host |
| gRPC / Protobuf | Contract-first, binary communication protocol |
| Dapper | Lightweight micro-ORM for database queries (faster than EF Core for simple CRUD) |
| SQLite | Lightweight database for discount/coupon data |
| Docker | Containerization |

**Database:** SQLite

**Why SQLite?** The discount service is simple — just coupon lookup by product name. SQLite is lightweight, requires no server process, and is more than sufficient for this use case.

**gRPC Contract (proto):**
```protobuf
service DiscountProto {
  rpc GetDiscount (GetDiscountRequest) returns (CouponModel);
  rpc CreateDiscount (CreateDiscountRequest) returns (CouponModel);
  rpc UpdateDiscount (UpdateDiscountRequest) returns (CouponModel);
  rpc DeleteDiscount (DeleteDiscountRequest) returns (DeleteDiscountResponse);
}
```

---

### 4. 📋 Ordering Microservice

**Purpose:** Handles the complete order lifecycle. Consumes the `BasketCheckoutEvent` from RabbitMQ asynchronously and creates an order in the database.

**Architecture Pattern:** DDD (Domain-Driven Design) + CQRS + Clean Architecture

This is the most architecturally rich service in the project. It demonstrates:
- **Entities, Value Objects, and Aggregates** (DDD building blocks)
- **Domain Events** (raised internally when order state changes)
- **Clean Architecture layers** — Domain → Application → Infrastructure → API
- **CQRS** — Commands (CreateOrder, UpdateOrder, DeleteOrder) and Queries (GetOrders, GetOrdersByCustomer, GetOrdersByName) are fully separated

**Technologies & Libraries:**

| Tool | Purpose |
|------|---------|
| ASP.NET Core (.NET 8) | Web API framework |
| Entity Framework Core | ORM for database access and migrations |
| SQL Server | Relational database for persistent order storage |
| MediatR | CQRS mediator + Domain Event dispatching |
| MassTransit | RabbitMQ consumer — listens for `BasketCheckoutEvent` |
| RabbitMQ | Message broker (consumed, not published) |
| FluentValidation | Command validation |
| Mapster | Object mapping (DTOs ↔ Domain models) |
| Docker | Containerization |

**Database:** SQL Server

**Why SQL Server + EF Core?** Orders are transactional, relational data with strict consistency requirements (ACID). SQL Server with EF Core provides full relational modeling, migrations, and query power needed for order management.

**DDD Building Blocks Implemented:**
- `Order` — Aggregate Root
- `OrderItem` — Entity within the Order aggregate
- `OrderName`, `Address`, `Payment` — Value Objects (immutable, compared by value)
- `OrderCreatedEvent`, `OrderUpdatedEvent` — Domain Events

**Clean Architecture Layers:**
```
Ordering.Domain       → Entities, Value Objects, Domain Events, Enums
Ordering.Application  → Use Cases, CQRS Handlers, Interfaces, DTOs
Ordering.Infrastructure → EF Core DbContext, Repositories, Message Consumers
Ordering.API          → Controllers/Endpoints, Dependency Injection setup
```

**Key Endpoints:**
- `GET /orders` — Get all orders
- `GET /orders/customer/{customerId}` — Get orders by customer
- `GET /orders/order-name/{name}` — Get orders by name
- `POST /orders` — Create order
- `PUT /orders` — Update order
- `DELETE /orders/{id}` — Delete order

---

### 5. 🚪 API Gateway (YARP)

**Purpose:** Single entry point for all client requests. Routes incoming requests to the appropriate downstream microservice and provides cross-cutting concerns.

**What it does:**
- Reverse proxy — routes `/catalog-service/**` → Catalog, `/basket-service/**` → Basket, etc.
- Load balancing across multiple service instances
- Request aggregation (BFF pattern)
- Rate limiting

**Technologies & Libraries:**

| Tool | Purpose |
|------|---------|
| YARP (Yet Another Reverse Proxy) | Microsoft's high-performance reverse proxy library |
| ASP.NET Core (.NET 8) | Hosting the gateway |
| Docker | Containerization |

**Why YARP over Ocelot?** YARP is Microsoft's official reverse proxy, deeply integrated with ASP.NET Core middleware pipeline, highly performant, and actively maintained.

**Configuration (appsettings.json):**
```json
{
  "ReverseProxy": {
    "Routes": {
      "catalog-route": {
        "ClusterId": "catalog-cluster",
        "Match": { "Path": "/catalog-service/{**catch-all}" }
      }
    },
    "Clusters": {
      "catalog-cluster": {
        "Destinations": {
          "destination1": { "Address": "http://catalog.api" }
        }
      }
    }
  }
}
```

---

## 🗃️ Database Summary

| Service | Database | Type | Why |
|---------|----------|------|-----|
| Catalog | PostgreSQL (via Marten) | Document Store | Flexible product schemas, document queries |
| Basket | Redis | In-Memory Key-Value | Ultra-fast session/cart data |
| Discount | SQLite | Embedded Relational | Simple CRUD, no server overhead |
| Ordering | SQL Server | Relational | Transactional order data, ACID compliance |

---

## 📨 Inter-Service Communication

### Synchronous (Request/Response)
- **REST (HTTP)** — Client → API Gateway → Services
- **gRPC** — Basket → Discount (for real-time discount calculation)

### Asynchronous (Event-Driven)
- **RabbitMQ via MassTransit** — Basket publishes `BasketCheckoutEvent` → Ordering consumes it

```
Basket Service ──[BasketCheckoutEvent]──► RabbitMQ ──► Ordering Service
                  (MassTransit publish)   (exchange)    (MassTransit consume)
```

---

## 🏛️ Design Patterns Used

| Pattern | Where Used | Why |
|---------|-----------|-----|
| CQRS | Catalog, Basket, Ordering | Separates read/write operations for clarity and scalability |
| Mediator | All services (MediatR) | Decouples request senders from handlers |
| Repository | Basket, Ordering | Abstracts data access from business logic |
| Domain Events | Ordering | Decoupled side-effects when aggregate state changes |
| Aggregate Root | Ordering (Order) | Enforces invariants for the Order entity cluster |
| Value Objects | Ordering | Immutable, equality-by-value types (Address, Payment) |
| Vertical Slice | Catalog | Each feature is a self-contained slice (Command + Handler + Endpoint) |
| Pipeline Behavior | All services | Cross-cutting concerns (validation, logging) via MediatR pipeline |

---

## 🐳 Infrastructure & DevOps

### Docker
Every service has its own `Dockerfile`. The entire application is orchestrated with `docker-compose`.

```bash
# Start all services
docker-compose up -d

# Stop all services
docker-compose down
```

### docker-compose Services
- `catalog.api` — Catalog microservice
- `basket.api` — Basket microservice
- `discount.grpc` — Discount gRPC service
- `ordering.api` — Ordering microservice
- `apigateway` — YARP API Gateway
- `catalogdb` — PostgreSQL instance
- `basketdb` — Redis instance
- `discountdb` — SQLite (embedded)
- `orderdb` — SQL Server instance
- `messagebroker` — RabbitMQ instance

---

## 📦 NuGet Packages Summary

| Package | Purpose |
|---------|---------|
| `MediatR` | CQRS Mediator pattern |
| `FluentValidation` | Request/command validation |
| `Mapster` | Object-to-object mapping |
| `Carter` | Minimal API module organization |
| `Marten` | PostgreSQL document store ORM |
| `StackExchange.Redis` | Redis client |
| `MassTransit.RabbitMQ` | Message broker abstraction |
| `Grpc.AspNetCore` | gRPC server hosting |
| `Grpc.Net.Client` | gRPC client |
| `Dapper` | Lightweight micro-ORM |
| `Microsoft.EntityFrameworkCore.SqlServer` | EF Core with SQL Server |
| `Yarp.ReverseProxy` | API Gateway reverse proxy |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run the Application

```bash
# Clone the repository
git clone https://github.com/MartinNady1/EShopMicroservice.git
cd EShopMicroservice

# Start all services via Docker Compose
docker-compose up -d
```

### Service URLs

| Service | URL |
|---------|-----|
| API Gateway | http://localhost:6000 |
| Catalog API | http://localhost:6000/catalog-service |
| Basket API | http://localhost:6000/basket-service |
| Ordering API | http://localhost:6000/ordering-service |
| RabbitMQ Management | http://localhost:15672 (guest/guest) |

---

## 📁 Project Structure

```
src/
├── Services/
│   ├── Catalog/
│   │   └── Catalog.API/
│   ├── Basket/
│   │   └── Basket.API/
│   ├── Discount/
│   │   └── Discount.Grpc/
│   └── Ordering/
│       ├── Ordering.Domain/
│       ├── Ordering.Application/
│       ├── Ordering.Infrastructure/
│       └── Ordering.API/
├── ApiGateways/
│   └── YarpApiGateway/
└── BuildingBlocks/
    └── BuildingBlocks/          ← Shared kernel (exceptions, behaviors, etc.)
```

---

## 📚 Key Concepts Learned

- **Microservices Architecture** — Independent, loosely coupled services with their own databases
- **Domain-Driven Design (DDD)** — Modeling software around business domains using Aggregates, Entities, and Value Objects
- **CQRS** — Separating Commands (write) from Queries (read) for better scalability and clarity
- **Event-Driven Architecture** — Services communicate asynchronously through events (RabbitMQ), reducing coupling
- **Vertical Slice Architecture** — Organizing code by feature rather than technical layer
- **API Gateway Pattern** — Single entry point for all clients using YARP
- **gRPC** — High-performance synchronous inter-service communication using binary Protocol Buffers
- **Clean Architecture** — Strict dependency rules ensuring domain logic is independent of frameworks and infrastructure
- **Containerization** — Docker for packaging services and Docker Compose for local orchestration

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

*Built as part of a Udemy course on Microservices Architecture with .NET*
