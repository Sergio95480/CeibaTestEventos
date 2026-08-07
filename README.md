# CeibaTestEventos

API REST para administración de eventos, venues y reservas.

Proyecto desarrollado con .NET 8 aplicando Clean Architecture, Domain Driven Design y separación por capas.

---

# Arquitectura

La solución está organizada en:


src
├── CeibaTestEventos.Domain
├── CeibaTestEventos.Application
├── CeibaTestEventos.Infrastructure
└── CeibaTestEventos.Api


## Domain

Contiene:

- Entidades
- Value Objects
- Enumeraciones
- Reglas de negocio

Entidades principales:

- Venue
- Event
- Reservation


## Application

Contiene los casos de uso:

- Commands
- Handlers
- Interfaces de repositorios

Patrón utilizado:

- CQRS


## Infrastructure

Responsable de:

- Entity Framework Core
- PostgreSQL
- Persistencia
- Implementación de repositorios


## API

ASP.NET Core Web API:

- Controllers
- Swagger
- Dependency Injection

---

# Tecnologías

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8.0.8
- PostgreSQL
- Swagger
- Docker


# Estado del proyecto

## Implementado

✅ Crear Venue  
✅ Consultar Venues  
✅ Crear Event  
✅ Validación capacidad Venue  
✅ Validación conflictos de horarios  
✅ Validaciones de dominio  
✅ Persistencia PostgreSQL  
✅ Migraciones EF Core  


## En desarrollo

🚧 API de Reservations

Pendiente:

- Crear reserva
- Confirmar reserva
- Cancelar reserva
- Código de confirmación


# Reglas de negocio

| Código | Regla |
|---|---|
| RN01 | Evento no puede superar capacidad del venue |
| RN02 | No pueden existir eventos activos con horarios superpuestos |
| RN03 | Eventos weekend no pueden iniciar después de las 22:00 |
| RN04 | No reservas una hora antes del evento |
| RN05 | Eventos > $100 máximo 10 entradas |
| RN06 | Evento pasa a completado después de su fecha fin |
| RN07 | Cancelaciones tardías se registran como perdidas |


# Ejecución

Restaurar paquetes:


dotnet restore


Compilar:


dotnet build


Ejecutar API:


dotnet run --project src/CeibaTestEventos.Api


Swagger disponible en:


/swagger


---

# Base de datos

Motor:

PostgreSQL

Database:


CeibaTestEventos


Migraciones:


InitialCreate


---

# Historia del proyecto

## 9c52b56

Initial project Ceiba setup with Clean Architecture and DDD

Base arquitectónica y dominio inicial.