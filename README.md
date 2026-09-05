# Prueba Técnica — Devsu 

Este proyecto es una simulación de un sistema bancario básico construido con **.NET 8** y **PostgreSQL**. Está diseñado utilizando una arquitectura de microservicios para garantizar escalabilidad e independencia.

##  ¿Qué hace este proyecto?

El sistema está dividido en dos servicios principales que se comunican entre sí para gestionar las operaciones bancarias:

1. **Servicio de Clientes (`ClientesService`)**:
   - Permite registrar, consultar, actualizar y eliminar clientes y personas.
   - Cada cliente tiene datos básicos como nombre, dirección, teléfono y una contraseña segura.

2. **Servicio de Cuentas (`CuentasService`)**:
   - Permite abrir cuentas bancarias (Ahorros o Corriente) asociadas a los clientes existentes.
   - Gestiona el registro de **movimientos** (depósitos y retiros).
   - Valida que haya saldo suficiente antes de realizar un retiro (arroja el error *"Saldo no disponible"* si los fondos son insuficientes).
   - Genera un **reporte del estado de cuenta** detallado de un cliente en un rango de fechas.

Ambos servicios funcionan de manera completamente independiente (cada uno con su propia base de datos) y se comunican en segundo plano de forma asíncrona (usando RabbitMQ) para mantener la información sincronizada.

##  Tecnologías Principales

- **Backend**: .NET 8 (C#), ASP.NET Core Web API
- **Base de Datos**: PostgreSQL
- **Mensajería**: RabbitMQ (mediante MassTransit)
- **Despliegue**: Docker y Docker Compose

---

## Cómo ejecutar el proyecto con Docker

La forma más rápida y recomendada de levantar todo el ecosistema (bases de datos, colas de mensajes y las APIs) es utilizando Docker Compose, ya que configura todo automáticamente.

### Requisitos previos
- Tener [Docker Desktop](https://www.docker.com/products/docker-desktop/) (o Docker Engine y Docker Compose) instalado y ejecutándose en tu equipo.

### Pasos para ejecutar:

1. Abre una terminal y navega hasta la carpeta raíz del proyecto (donde se encuentra el archivo `docker-compose.yml`).
2. Ejecuta el siguiente comando para construir y levantar todos los contenedores:

   ```bash
   docker compose up --build
   ```

3. Espera unos momentos a que todos los contenedores descarguen sus dependencias, se construyan e inicien. La base de datos se inicializará automáticamente con algunas tablas y datos base.
4. Una vez que la terminal muestre que los servicios están listos, podrás acceder a ellos a través de tu navegador:

   -  **API de Clientes (Swagger)**: [http://localhost:5001/swagger](http://localhost:5001/swagger)
   -  **API de Cuentas (Swagger)**: [http://localhost:5002/swagger](http://localhost:5002/swagger)
   -  **Panel de RabbitMQ**: [http://localhost:15672](http://localhost:15672) *(Usuario y contraseña: `guest` / `guest`)*

### Para detener el proyecto:

Si deseas detener la ejecución de todos los servicios, abre otra terminal en la misma carpeta y ejecuta:

```bash
docker compose down
```
*(Nota: Si deseas borrar también los datos guardados en la base de datos de Docker, puedes usar `docker compose down -v`)*

---

## Cómo probar los endpoints

Tienes dos formas principales de interactuar con el sistema:

1. **Usando Swagger (Directo en el navegador)**: 
   Abre las URLs de Swagger listadas arriba. Desde allí puedes ver qué datos requiere cada petición y ejecutarlas directamente.

2. **Usando Postman (Recomendado)**: 
   En la raíz del proyecto encontrarás el archivo `Postman_Collection.json`.
   - Abre Postman e importa ese archivo.
   - La colección ya viene configurada con las pruebas listas para ser ejecutadas en orden lógico (Crear cliente Crear cuenta  Registrar depósitos y retiros  Generar el reporte).
   - Los IDs se guardan automáticamente entre peticiones para hacer las pruebas mucho más fáciles.
