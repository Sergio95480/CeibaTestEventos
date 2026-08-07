# CeibaTestEventos

API REST para la administración de **venues, eventos y reservas**.

Proyecto desarrollado con **.NET 8**, aplicando principios de **Clean Architecture**, **Domain Driven Design (DDD)** y separación de responsabilidades por capas.

El objetivo principal es implementar una solución mantenible, escalable y orientada al dominio, donde las reglas de negocio estén centralizadas y protegidas dentro de la capa Domain.

---

# Arquitectura

La solución está organizada siguiendo el enfoque **Clean Architecture**:

```
CeibaTestEventos
│
├── src
│   │
│   ├── CeibaTestEventos.Domain
│   │
│   ├── CeibaTestEventos.Application
│   │
│   ├── CeibaTestEventos.Infrastructure
│   │
│   └── CeibaTestEventos.Api
│
└── tests
    │
    ├── CeibaTestEventos.UnitTests
    │
    └── CeibaTestEventos.IntegrationTests
```

---

# Descripción de capas

## Domain

Contiene el núcleo del negocio.

Responsabilidades:

- Entidades
- Value Objects
- Enumeraciones
- Excepciones de dominio
- Reglas de negocio

Entidades principales:

- Venue
- Event
- Reservation

Características:

- Independiente de frameworks.
- No depende de infraestructura.
- Contiene las decisiones importantes del negocio.

---

## Application

Contiene los casos de uso de la aplicación.

Responsabilidades:

- Commands
- Handlers
- DTOs
- Interfaces de repositorios
- Orquestación de procesos

Patrones utilizados:

- CQRS
- Dependency Injection

---

## Infrastructure

Implementa los detalles técnicos.

Responsabilidades:

- Entity Framework Core
- PostgreSQL
- Persistencia
- Migraciones
- Implementación de repositorios

---

## API

Capa de exposición HTTP.

Incluye:

- Controllers REST
- Swagger/OpenAPI
- Configuración de servicios
- Inyección de dependencias
- Middleware global de excepciones

---

# Tecnologías utilizadas

| Tecnología | Uso |
|---|---|
| .NET 8 | Framework principal |
| ASP.NET Core Web API | API REST |
| Entity Framework Core 8 | ORM |
| PostgreSQL | Base de datos |
| Docker | Contenedores |
| Swagger | Documentación API |
| xUnit | Pruebas automatizadas |
| GitHub | Control de versiones |

---

# Funcionalidades implementadas

## Gestión de Venues

Implementado:

✅ Crear venue  
✅ Consultar venues  
✅ Validación de datos obligatorios  

---

# Gestión de Eventos

Implementado:

✅ Crear eventos  
✅ Consultar eventos  
✅ Validación de capacidad del venue  
✅ Validación de conflictos de horarios  
✅ Publicación de eventos  
✅ Completar eventos  


Flujo de estados:

```
Draft
  |
  | Publicar
  v
Published
  |
  | Completar
  v
Completed
```

---

# Gestión de Reservas

Implementado:

✅ Crear reserva  
✅ Actualizar capacidad disponible del evento  
✅ Confirmar reserva  
✅ Generar código de confirmación  
✅ Cancelar reserva  
✅ Aplicar regla de cancelación tardía  


Flujo normal:

```
Pending
   |
   | Confirmar
   v
Confirmed
```


Cancelación con más de 48 horas:

```
Confirmed
    |
    v
Cancelled
```


Cancelación con menos de 48 horas:

```
Confirmed
    |
    v
Lost
```

---

# Reglas de negocio implementadas

| Código | Regla |
|---|---|
| RN01 | Un evento no puede superar la capacidad disponible del venue |
| RN02 | No pueden existir eventos activos con horarios superpuestos |
| RN03 | Validaciones según tipo de evento |
| RN04 | No se permiten reservas una hora antes del inicio del evento |
| RN05 | Eventos superiores a $100 permiten máximo 10 entradas por transacción |
| RN06 | Un evento publicado puede pasar al estado completado |
| RN07 | Cancelaciones tardías se registran como perdidas |

---

# Manejo global de excepciones

La aplicación cuenta con un middleware global para transformar excepciones de dominio en respuestas HTTP controladas.

Ejemplo de respuesta:

```json
{
  "statusCode": 400,
  "message": "El venue ya tiene un evento programado en ese horario."
}
```

Esto permite mantener respuestas consistentes para consumidores de la API.

---

# Ejecución local

## Requisitos

Instalar previamente:

- .NET 8 SDK
- Docker Desktop
- PostgreSQL (opcional si no se utiliza Docker)

---

# Clonar repositorio

```bash
git clone https://github.com/Sergio95480/CeibaTestEventos.git

cd CeibaTestEventos
```

---

# Restaurar dependencias

```bash
dotnet restore
```

---

# Ejecutar base de datos

```bash
docker-compose up -d
```

---

# Aplicar migraciones

```bash
dotnet ef database update \
--project src/CeibaTestEventos.Infrastructure \
--startup-project src/CeibaTestEventos.Api
```

---

# Ejecutar API

```bash
dotnet run --project src/CeibaTestEventos.Api
```

Swagger estará disponible en:

```
http://localhost:5180/swagger
```

---

# Pruebas automatizadas

El proyecto cuenta con pruebas unitarias y pruebas de integración utilizando **xUnit**.

Ejecutar todas las pruebas:

```bash
dotnet test
```

---

# Pruebas unitarias

Ubicación:

```
tests/CeibaTestEventos.UnitTests
```

Validan reglas del dominio:

✅ Publicación de eventos  
✅ Validación de capacidad  
✅ Restricción de reservas próximas al evento  
✅ Restricción de cantidad de entradas según precio  
✅ Confirmación de reservas  
✅ Cancelación normal y tardía  


Resultado actual:

```
Total: 6
Superado: 6
Error: 0
```

---

# Pruebas de integración

Ubicación:

```
tests/CeibaTestEventos.IntegrationTests
```

Las pruebas utilizan `WebApplicationFactory` para validar flujos completos mediante HTTP.


Escenarios implementados:

## Crear evento

Flujo:

```
Crear Venue
      |
Crear Event
      |
Validar HTTP 201
```


## Crear y confirmar reserva

Flujo:

```
Crear Venue
      |
Crear Event
      |
Publicar Event
      |
Crear Reservation
      |
Confirmar Reservation
```


## Cancelar reserva tardía

Flujo:

```
Crear Venue
      |
Crear Event
      |
Publicar Event
      |
Crear Reservation
      |
Confirmar Reservation
      |
Cancelar Reservation
      |
Validar estado Lost
```


Resultado actual:

```
Total: 3
Superado: 3
Error: 0
```

---

# Base de datos

Motor:

```
PostgreSQL
```

Nombre:

```
CeibaTestEventos
```

Migraciones:

```
InitialCreate
```

---

# Endpoints principales

## Venues

```
POST /api/Venues

GET /api/Venues
```

---

## Events

```
POST /api/Events

GET /api/Events

POST /api/Events/{id}/publish

POST /api/Events/{id}/complete
```

---

## Reservations

```
POST /api/Reservations

POST /api/Reservations/{id}/confirm

POST /api/Reservations/{id}/cancel
```

---

# Historia del proyecto

## 9c52b56

Creación inicial del proyecto con Clean Architecture y DDD.

---

## c11bdad

Implementación de creación de reservas y actualización de capacidad del evento.

---

## 8a6e2b0

Implementación del flujo de confirmación y cancelación de reservas.

---

## 97d5dcf

Implementación de pruebas unitarias para reglas de negocio.

---

## d4xxxxx

Implementación de pruebas de integración para flujos completos de eventos y reservas.

---

# Estado actual

Proyecto funcional con:

✅ Arquitectura Clean Architecture  
✅ Dominio con reglas de negocio encapsuladas  
✅ Persistencia PostgreSQL  
✅ API REST documentada con Swagger  
✅ Manejo global de excepciones  
✅ Pruebas unitarias  
✅ Pruebas de integración  

Pendiente como diferenciador:

- Despliegue en proveedor cloud con URL pública.