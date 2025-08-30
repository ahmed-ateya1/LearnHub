# LearnHub(Microservices Architecture)
![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet?logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue?logo=docker)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Message%20Broker-ff6600?logo=rabbitmq&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-316192?logo=postgresql&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-Cache-dc382d?logo=redis&logoColor=white)
![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)

## 📌 Purpose and Scope
LearnHub is a **microservices-based e-learning platform** built with **.NET 9.0**.  
It provides a complete ecosystem for **course creation, student enrollment, order management, and real-time notifications**.  

This project demonstrates:
- Distributed system design  
- Clean Architecture & Domain-Driven Design (DDD)  
- Event-driven communication with RabbitMQ  
- API Gateway with YARP  
- Polyglot persistence with PostgreSQL, SQL Server, and Redis  

---

## 🚀 Platform Overview
LearnHub enables:
- **Instructors** → Create and manage courses, quizzes, and assessments  
- **Students** → Browse, purchase, and enroll in courses  
- **System** → Manages lifecycle from browsing → enrollment → progress tracking  

### 🔑 Core Business Capabilities
- ✅ Course creation and management  
- ✅ Student registration & authentication  
- ✅ Course catalog browsing & search  
- ✅ Shopping cart & order processing  
- ✅ Course enrollment  
- ✅ Real-time notifications (SignalR)  
- ✅ Quiz and assessment management  

---

## 🏗️ System Architecture

LearnHub follows a **distributed microservices architecture**:  

- **API Gateway Layer** → YARP API Gateway (Port 1000)  
- **Core Business Services** → Independent microservices per domain  
- **Data Layer** → Dedicated data stores (SQL Server, PostgreSQL, Redis)  
- **Messaging Infrastructure** → RabbitMQ for async events  

### Architecture Diagram
<img width="1579" height="562" alt="image" src="https://github.com/user-attachments/assets/541b5978-7d0a-4290-a917-199d863d8649" />

### Sequence Flow Example
<img width="1654" height="810" alt="image" src="https://github.com/user-attachments/assets/ab6e886a-97b4-4ade-a0fd-36c1c7d6b25d" />

### Data Base Schema
<img width="1204" height="653" alt="image" src="https://github.com/user-attachments/assets/d276cb07-fe78-4af6-a2bf-57886354041c" />


---

## 🧩 Microservice Composition

| Service              | Container Name      | Port  | Database                | Responsibility                  |
|----------------------|--------------------|------:|------------------------|--------------------------------|
| **Users.API**        | `usersapi`         | 5000  | PostgreSQL (UserDb)    | Authentication, user management |
| **Course.API**       | `coursesapi`       | 7000  | SQL Server (CourseDb)  | Courses, lectures, quizzes      |
| **Order.API**        | `orderapi`         | 8000  | SQL Server (OrderDb)   | Order processing, payments      |
| **Cart.API**         | `shoppingcart-api` | 9000  | Redis                  | Shopping cart management        |
| **Enrollment.API**   | `enrollmentapi`    | 5555  | PostgreSQL (EnrollDb)  | Course enrollments              |
| **Notification.API** | `notificationapi`  | 6000  | PostgreSQL (NotifDb)   | Real-time notifications         |

---

## 🛠️ Technology Stack

### Core Technologies
- **.NET 9.0** – Primary runtime  
- **ASP.NET Core** – Web API framework  
- **Carter** – Minimal API routing  
- **MediatR** – CQRS & request handling  
- **Entity Framework Core** – ORM for SQL Server  
- **Marten** – Event store on PostgreSQL  

### Infrastructure
- **YARP** – API Gateway  
- **RabbitMQ** – Message broker  
- **PostgreSQL** – User, Enrollment, Notifications  
- **SQL Server** – Courses, Orders  
- **Redis** – Caching & shopping cart  
- **Docker & Docker Compose** – Containerization  

### Communication Patterns
- **HTTP/REST** – Synchronous calls  
- **SignalR** – Real-time updates  
- **RabbitMQ** – Event-driven async messaging  
- **Swagger/OpenAPI** – API documentation  

---
## 📦 Development & Deployment

### Container Orchestration
- **Service Containers** → Each microservice isolated  
- **Database Containers** → PostgreSQL, SQL Server, Redis  
- **Infrastructure** → RabbitMQ for async messaging  

**Networks**:
- `LearnHubNetwork` → Main communication layer  
- `UserNetwork`, `CourseNetwork`, `OrderNetwork` → Service isolation  
- `NotificationNetwork`, `EnrollmentNetwork` → Specialized domains  

---

## 📚 Core Business Domains

- **User Management** → Authentication & profiles  
- **Educational Content** → Courses, lectures, quizzes  
- **Commerce** → Shopping cart, orders, payments  
- **Communication** → Notifications & real-time updates  

---

## ▶️ Getting Started

### Prerequisites
- Docker & Docker Compose  
- .NET 9.0 SDK  
- SQL Server & PostgreSQL clients  

### Run Locally
```bash
# Clone repo
git clone https://github.com/ahmed-ateya1/LearnHub
cd learnhub

# Build and start services
docker-compose up --build
```

---


## 🤝 Contribution
Contributions are welcome!  
Please open an issue or submit a PR for improvements.

## 📜 License
This project is licensed under the MIT License.
