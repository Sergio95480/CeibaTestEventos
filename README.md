# CeibaTestEventos

API REST para la administración de eventos, lugares (venues) y reservas.

Proyecto desarrollado con **.NET 8** aplicando **Clean Architecture**, **Domain Driven Design (DDD)** y separación de responsabilidades por capas.

---

# Arquitectura

La solución está organizada siguiendo principios de Clean Architecture:


CeibaTestEventos
│
├── src
│
├── CeibaTestEventos.Domain
├── CeibaTestEventos.Application
├── CeibaTestEventos.Infrastructure
└── CeibaTestEventos.Api
│
└── tests
└── CeibaTestEventos.UnitTests


---

# Descripción de capas

## Domain

Contiene la lógica principal del negocio:

- Entidades
- Objetos de valor (Value Objects)
- Enumeraciones
- Excepciones de dominio
- Reglas de negocio

Entidades principales:

- Venue
- Event
- Reservation

Esta capa es independiente de frameworks y tecnologías externas.

---

## Application

Contiene los casos de uso de la aplicación:

- Commands
- Handlers
- DTOs
- Interfaces de repositorios

Patrones utilizados:

- CQRS
- Inyección de dependencias

---

## Infrastructure

Responsable de la implementación técnica:

- Entity Framework Core
- PostgreSQL
- Persistencia de datos
- Migraciones
- Implementación de repositorios

---

## API

Capa de exposición HTTP:

- Controllers
- Swagger/OpenAPI
- Configuración de servicios
- Manejo global de excepciones

---

# Tecnologías utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- Docker
- Swagger
- xUnit
- GitHub

---

# Funcionalidades implementadas

## Gestión de Venues

✅ Crear venue  
✅ Consultar venues  

---

## Gestión de Eventos

✅ Crear eventos  
✅ Consultar eventos  
✅ Validar capacidad del venue  
✅ Validar conflictos de horarios  
✅ Publicar eventos  
✅ Completar eventos  

Flujo de estados:


Draft
|
| Publicar
v
Published
|
| Completar
v
Completed


---

## Gestión de Reservas

✅ Crear reserva  
✅ Confirmar reserva  
✅ Generar código de confirmación  
✅ Cancelar reserva  
✅ Aplicar penalización por cancelación tardía  

Flujo de reserva:


Pending
|
Confirmar
|
Confirmed


Cancelación:

Más de 48 horas antes del evento:


Confirmed
|
Cancelled


Menos de 48 horas antes del evento:


Confirmed
|
Lost


---

# Reglas de negocio implementadas

| Código | Regla |
|---|---|
| RN01 | Un evento no puede superar la capacidad disponible del venue |
| RN02 | No pueden existir eventos activos con horarios superpuestos |
| RN03 | Validaciones según tipo de evento |
| RN04 | No se permiten reservas una hora antes del inicio del evento |
| RN05 | Eventos con precio superior a $100 permiten máximo 10 entradas por transacción |
| RN06 | Un evento publicado puede pasar al estado completado |
| RN07 | Las cancelaciones tardías se registran como perdidas |

---

# Ejecución local

## Requisitos

Instalar previamente:

- .NET 8 SDK
- Docker Desktop
- PostgreSQL (o ejecutarlo mediante Docker)

---

## Clonar repositorio

```bash
git clone https://github.com/Sergio95480/CeibaTestEventos.git

cd CeibaTestEventos
Restaurar dependencias
dotnet restore
Ejecutar base de datos
docker-compose up -d
Aplicar migraciones
dotnet ef database update \
--project src/CeibaTestEventos.Infrastructure \
--startup-project src/CeibaTestEventos.Api
Ejecutar API
dotnet run --project src/CeibaTestEventos.Api

Swagger estará disponible en:

http://localhost:5180/swagger
Pruebas automatizadas

Ejecutar:

dotnet test tests/CeibaTestEventos.UnitTests

Actualmente se validan las principales reglas de negocio:

✅ Publicación de eventos
✅ Validación de capacidad
✅ Restricción de reservas próximas al evento
✅ Cancelación normal de reservas
✅ Cancelación tardía con estado perdido
✅ Restricción de cantidad de entradas según precio

Resultado actual:

Total: 6
Superado: 6
Error: 0
Base de datos

Motor utilizado:

PostgreSQL

Migraciones:

InitialCreate
Manejo de excepciones

La aplicación cuenta con manejo global de excepciones.

Las excepciones de dominio son transformadas en respuestas HTTP controladas.

Ejemplo:

{
  "statusCode": 400,
  "message": "El venue ya tiene un evento programado en ese horario."
}
Historia del proyecto
9c52b56

Creación inicial del proyecto con Clean Architecture y DDD.

c11bdad

Implementación de creación de reservas y actualización de capacidad del evento.

8a6e2b0

Implementación del flujo de confirmación y cancelación de reservas.

97d5dcf

Implementación de pruebas unitarias para reglas de negocio.


Después ejecuta:

```powershell
git add README.md
git commit -m "Actualizar README con documentación completa del proyecto"
git push