# CeibaTestEventos

API REST para la gestión de eventos, lugares del evento y reservas.

Proyecto desarrollado con **.NET 8**, aplicando principios de **Arquitectura Limpia (Clean Architecture)**, **Diseño dirigido por dominio (DDD)** y separación de responsabilidades por capas.

---

# Demo desplegada

API disponible en:

https://ceibatesteventos.onrender.com

Documentación interactiva:

https://ceibatesteventos.onrender.com/swagger

---

# Arquitectura del proyecto

La solución está organizada siguiendo principios de Clean Architecture:

```
CeibaTestEventos
│
├── src
│   │
│   ├── CeibaTestEventos.Domain
│   ├── CeibaTestEventos.Application
│   ├── CeibaTestEventos.Infrastructure
│   └── CeibaTestEventos.Api
│
└── tests
    └── CeibaTestEventos.IntegrationTests
```

---

# Descripción de capas

## Domain

Contiene las reglas principales del negocio.

Responsabilidades:

* Entidades del dominio
* Reglas de negocio
* Estados del proceso
* Excepciones de dominio
* Validaciones

Entidades principales:

* Evento
* Lugar del evento
* Reserva

Esta capa no depende de frameworks externos.

---

## Application

Contiene los casos de uso de la aplicación.

Incluye:

* Comandos
* Manejadores de casos de uso
* DTOs
* Interfaces de repositorios

Patrones utilizados:

* CQRS
* Inyección de dependencias

---

## Infrastructure

Implementa la parte técnica del sistema:

* Entity Framework Core
* PostgreSQL
* Migraciones
* Persistencia
* Implementación de repositorios

---

## API

Capa encargada de exponer los servicios HTTP.

Incluye:

* Controllers REST
* Swagger/OpenAPI
* Middleware global de excepciones
* Configuración de servicios

---

# Tecnologías utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core 8
* PostgreSQL
* Docker
* Swagger
* xUnit
* GitHub
* Render Cloud

---

# Modelo funcional

El sistema administra:

```
LUGAR DEL EVENTO
        |
        |
     EVENTO
        |
        |
     RESERVA
```

Flujo principal:

```
1. Crear lugar del evento
          ↓
2. Crear evento asociado
          ↓
3. Publicar evento
          ↓
4. Crear reserva
          ↓
5. Confirmar reserva
          ↓
6. Cancelar reserva
          ↓
7. Consultar reporte de ocupación
```

---

# Funcionalidades implementadas

# Gestión de lugares del evento

Permite registrar los espacios físicos donde se realizan eventos.

Ejemplos:

* Estadios
* Teatros
* Auditorios
* Centros de convenciones

## Crear lugar del evento

Endpoint:

```
POST /api/Venues
```

Ejemplo:

```json
{
  "nombre": "Movistar Arena",
  "ciudad": "Bogotá",
  "capacidad": 15000
}
```

Respuesta:

```json
{
  "id": "244d4087-998c-4b4a-98cf-a264d073c57e",
  "nombre": "Movistar Arena",
  "ciudad": "Bogotá",
  "capacidad": 15000
}
```

---

# Gestión de eventos

Permite crear y administrar eventos.

## Crear evento

Endpoint:

```
POST /api/Events
```

Ejemplo:

```json
{
  "venueId": "244d4087-998c-4b4a-98cf-a264d073c57e",
  "nombre": "Concierto Rock Bogotá",
  "tipoEvento": 1,
  "fechaInicio": "2026-08-15T20:00:00Z",
  "fechaFin": "2026-08-15T23:00:00Z",
  "precio": 85000,
  "capacidad": 5000
}
```

Respuesta:

```json
{
  "id": "25d2d547-d12a-4bb3-aa97-2d4c7a286a4e",
  "nombre": "Concierto Rock Bogotá",
  "estado": 1
}
```

---

## Publicar evento

Permite cambiar el estado:

```
Draft → Published
```

Endpoint:

```
POST /api/Events/{id}/publish
```

---

## Completar evento

Permite finalizar un evento publicado.

Estado:

```
Published → Completed
```

Endpoint:

```
POST /api/Events/{id}/complete
```

---

# Gestión de reservas

Permite administrar la compra y cancelación de entradas.

## Crear reserva

Endpoint:

```
POST /api/Reservations
```

Ejemplo:

```json
{
  "eventId": "25d2d547-d12a-4bb3-aa97-2d4c7a286a4e",
  "compradorEmail": "cliente@test.com",
  "cantidad": 2
}
```

Respuesta:

```json
{
  "id": "7c4ef35b-eff6-41ae-bc58-25688f2a471c",
  "eventId": "25d2d547-d12a-4bb3-aa97-2d4c7a286a4e",
  "cantidad": 2,
  "estado": 2,
  "codigoConfirmacion": "31B74208"
}
```

---

## Cancelar reserva

Reglas aplicadas:

### Cancelación normal

Más de 48 horas antes:

```
Confirmed → Cancelled
```

### Cancelación tardía

Menos de 48 horas:

```
Confirmed → Lost
```

Endpoint:

```
POST /api/Reservations/{id}/cancel
```

Respuesta:

```json
{
  "id": "7c4ef35b-eff6-41ae-bc58-25688f2a471c",
  "estado": 3
}
```

---

# Reporte de ocupación RF-06

El sistema genera información consolidada por evento:

Incluye:

* Total entradas vendidas confirmadas
* Entradas disponibles restantes
* Porcentaje de ocupación
* Total ingresos generados
* Estado actual del evento

Endpoint:

```
GET /api/Reports/events/{eventId}/occupation
```

Ejemplo respuesta:

```json
{
  "eventId": "25d2d547-d12a-4bb3-aa97-2d4c7a286a4e",
  "nombreEvento": "Concierto Rock Bogotá",
  "entradasVendidas": 2,
  "entradasDisponibles": 4998,
  "porcentajeOcupacion": 0.04,
  "ingresosTotales": 170000,
  "estado": "Published"
}
```

---

# Reglas de negocio implementadas

| Código | Regla                                                                          |
| ------ | ------------------------------------------------------------------------------ |
| RN01   | Un evento no puede superar la capacidad disponible del lugar                   |
| RN02   | No pueden existir eventos activos con horarios superpuestos                    |
| RN03   | Validaciones según tipo de evento                                              |
| RN04   | No se permiten reservas una hora antes del inicio del evento                   |
| RN05   | Eventos con precio superior a $100 permiten máximo 10 entradas por transacción |
| RN06   | Un evento publicado puede pasar al estado completado                           |
| RN07   | Cancelaciones tardías quedan registradas como perdidas                         |

---

# Pruebas automatizadas

Ejecutar:

```bash
dotnet test tests/CeibaTestEventos.UnitTests
```

y

```bash
dotnet test tests/CeibaTestEventos.IntegrationTests
```

Resultado actual:

```
Unit Tests
Total: 6
Superados: 6
Errores: 0


Integration Tests
Total: 3
Superados: 3
Errores: 0
```

Validaciones realizadas:

✅ Creación de eventos
✅ Creación de reservas
✅ Confirmación de reservas
✅ Cancelación normal
✅ Cancelación tardía
✅ Validación de capacidad
✅ Reporte de ocupación

---

# Base de datos

Motor utilizado:

```
PostgreSQL
```

Migración inicial:

```
InitialCreate
```

Tablas principales:

```
venues

events

reservations
```

---

# Ejecución local

Requisitos:

* .NET 8 SDK
* Docker Desktop
* PostgreSQL

Clonar:

```bash
git clone https://github.com/Sergio95480/CeibaTestEventos.git
```

Restaurar paquetes:

```bash
dotnet restore
```

Ejecutar base de datos:

```bash
docker-compose up -d
```

Aplicar migraciones:

```bash
dotnet ef database update \
--project src/CeibaTestEventos.Infrastructure \
--startup-project src/CeibaTestEventos.Api
```

Ejecutar API:

```bash
dotnet run --project src/CeibaTestEventos.Api
```

---

# Docker

Construcción:

```bash
docker build -t ceiba-test-eventos .
```

Ejecución:

```bash
docker run -p 5180:8080 ceiba-test-eventos
```

Swagger:

```
http://localhost:5180/swagger
```

---

# Manejo de errores

La aplicación cuenta con middleware global de excepciones.

Las reglas de dominio generan respuestas controladas:

Ejemplo:

```json
{
  "statusCode":400,
  "message":"No existen suficientes entradas disponibles."
}
```

---

# Historial del proyecto

## 9c52b56

Creación inicial del proyecto con Clean Architecture y DDD.

## c11bdad

Implementación de creación de reservas y actualización de capacidad del evento.

## 8a6e2b0

Implementación del flujo de confirmación y cancelación de reservas.

## 97d5dcf

Implementación de pruebas unitarias para reglas de negocio.

## Actualización actual

* Despliegue en Render
* Base de datos PostgreSQL en nube
* Pruebas de integración
* Reporte de ocupación RF-06
* Documentación completa de API

---

# Autor

Sergio

Proyecto técnico desarrollado como prueba de arquitectura backend con .NET 8.
