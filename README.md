# API de Respuestas de Crédito Automotriz

Sistema para recibir y procesar respuestas de crédito enviadas por financieras externas.

## Tecnologías

- .NET 8
- SQL Server
- Entity Framework Core
- JWT Authentication
- Swagger

## Configuración

### 1. Base de Datos

Actualizar la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=CreditosDB;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
}
```

### 2. Crear Base de Datos

Crear la base de datos manualmente en SQL Server:

```sql
CREATE DATABASE CreditosDB;
```

### 3. Aplicar Migraciones

Las tablas se crean automáticamente con Entity Framework Migrations.

En la Consola del Administrador de Paquetes de Visual Studio:

```powershell
Add-Migration Inicial
Update-Database
```

### 4. Insertar Datos de Prueba

Ejecutar en SQL Server:

```sql
-- Financieras
INSERT INTO Financieras (Nombre, Codigo, TiempoEsperaMinutos, Activa)
VALUES 
    ('Banco Pichincha', 'PICHINCHA', 30, 1),
    ('Banco Guayaquil', 'GUAYAQUIL', 45, 1),
    ('Produbanco', 'PRODUBANCO', 60, 1);

-- Asesores
INSERT INTO Asesores (Nombre, Email, Telefono, Activo)
VALUES 
    ('Juan Pérez', 'juan.perez@automotores.com', '0991234567', 1),
    ('María González', 'maria.gonzalez@automotores.com', '0997654321', 1);

-- Solicitudes
INSERT INTO Solicitudes (NumeroSolicitud, NombreCliente, IdAsesor, IdFinanciera)
VALUES 
    ('SOL-2025-001245', 'Roberto Martínez', 1, 1),
    ('SOL-2025-001246', 'Ana López', 2, 2),
    ('SOL-2025-001247', 'Pedro Sánchez', 1, 3);
```

## Ejecución

### Ejecutar la API

En Visual Studio: Presionar `F5` o `Ctrl + F5`

La API estará disponible en: `https://localhost:7XXX` (el puerto se muestra en la consola)

Swagger UI: `https://localhost:7XXX/swagger`

## Uso de la API

### 1. Registrar Usuario

**POST** `/api/auth/register`

```json
{
  "username": "admin_continental",
  "password": "Continental2025!"
}
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin_continental",
  "expiracion": "2025-11-29T04:30:00Z"
}
```

### 2. Login

**POST** `/api/auth/login`

```json
{
  "username": "admin_continental",
  "password": "Continental2025!"
}
```

### 3. Enviar Respuesta de Crédito

**POST** `/api/creditos/respuesta`

**Headers:**
```
Authorization: Bearer {TOKEN_OBTENIDO_EN_LOGIN}
Content-Type: application/json
```

#### Estado: APROBADO

```json
{
  "numeroSolicitud": "SOL-2025-001245",
  "estado": "APROBADO",
  "montoAprobado": 14500,
  "tasa": 15.6,
  "observacion": "Cliente califica sin restricciones",
  "fechaRespuesta": "2025-11-28T10:25:00"
}
```

#### Estado: CONDICIONADO

```json
{
  "numeroSolicitud": "SOL-2025-001246",
  "estado": "CONDICIONADO",
  "montoAprobado": 12000,
  "tasa": 16.5,
  "condiciones": [
    "Presentar garante con ingresos mínimos de $800",
    "Incrementar entrada al 30%",
    "Seguro de desgravamen obligatorio"
  ],
  "observacion": "Aprobación sujeta a condiciones",
  "fechaRespuesta": "2025-11-28T11:00:00"
}
```

#### Estado: REQUIERE_DOCUMENTOS

```json
{
  "numeroSolicitud": "SOL-2025-001247",
  "estado": "REQUIERE_DOCUMENTOS",
  "listaDocumentos": [
    "Cédula de identidad actualizada",
    "Planilla de luz (últimos 3 meses)",
    "Certificado de ingresos"
  ],
  "observacion": "Documentación incompleta",
  "fechaRespuesta": "2025-11-28T12:00:00"
}
```

#### Estado: NEGADO

```json
{
  "numeroSolicitud": "SOL-2025-001247",
  "estado": "NEGADO",
  "observacion": "Cliente presenta historial crediticio negativo",
  "fechaRespuesta": "2025-11-28T14:00:00"
}
```

#### Estado: EN_PROCESO

```json
{
  "numeroSolicitud": "SOL-2025-001247",
  "estado": "EN_PROCESO",
  "observacion": "Solicitud en análisis de comité",
  "fechaRespuesta": "2025-11-28T13:00:00"
}
```

**Nota:** El tiempo de espera para EN_PROCESO se obtiene del campo `TiempoEsperaMinutos` de la tabla Financieras.

### 4. Consultar Respuesta

**GET** `/api/creditos/respuesta/{numeroSolicitud}`

**Headers:**
```
Authorization: Bearer {TOKEN}
```

**Ejemplo:**
```
GET /api/creditos/respuesta/SOL-2025-001245
```

## Validaciones

- **APROBADO**: Requiere `montoAprobado` y `tasa`
- **CONDICIONADO**: Requiere `condiciones` (lista) y `montoAprobado`
- **NEGADO**: Requiere `observacion`
- **REQUIERE_DOCUMENTOS**: Requiere `listaDocumentos` (lista)
- **EN_PROCESO**: Sin validaciones estrictas

## Códigos de Respuesta HTTP

- `200 OK`: Operación exitosa
- `400 Bad Request`: Validación fallida o JSON mal formado
- `401 Unauthorized`: Token no proporcionado o inválido
- `404 Not Found`: Solicitud no encontrada
- `500 Internal Server Error`: Error en el servidor

## Seguridad

La API implementa autenticación mediante **JSON Web Tokens (JWT)**:

### Flujo de Autenticación

1. **Registro**: Usuario se registra con username y password
2. **Login**: Usuario obtiene un token JWT válido por 8 horas
3. **Uso**: Token se envía en el header `Authorization: Bearer {token}` en cada request
4. **Validación**: El middleware de JWT valida automáticamente el token en cada request

### Características de Seguridad

- Passwords hasheados con SHA256
- Tokens firmados con clave secreta (configurada en `appsettings.json`)
- Validación de issuer, audience y tiempo de expiración
- Endpoints protegidos con atributo `[Authorize]`
- Tokens expiran automáticamente después de 8 horas

### Configuración JWT

La clave secreta se configura en `appsettings.json`:

```json
"Jwt": {
  "Key": "AutomotoresContinental_SecretKey_2025_MuySegura_32Caracteres!",
  "Issuer": "AutomotoresContinental",
  "Audience": "FinancierasExternas"
}
```

**Nota de Producción**: En producción, la clave JWT debe almacenarse en variables de entorno o Azure Key Vault, no en `appsettings.json`.


