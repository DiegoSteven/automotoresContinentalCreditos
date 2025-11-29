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

Configurar también el email en `appsettings.json`:

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "SmtpUser": "tu-email@gmail.com",
  "SmtpPassword": "tu-app-password",
  "FromEmail": "tu-email@gmail.com",
  "FromName": "Automotores Continental"
}
```

**Nota:** Para Gmail, debes generar una "Contraseña de aplicación":
1. Ve a tu cuenta de Google → Seguridad
2. Activa la verificación en 2 pasos
3. En "Contraseñas de aplicaciones", genera una nueva
4. Usa esa contraseña en `SmtpPassword`

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

-- Solicitudes de Crédito Automotriz
INSERT INTO Solicitudes (NumeroSolicitud, NombreCliente, IdAsesor, IdFinanciera, MarcaVehiculo, ModeloVehiculo, AnioVehiculo, PrecioVehiculo, TipoVehiculo)
VALUES 
    ('SOL-2025-001245', 'Roberto Martínez', 1, 1, 'Toyota', 'Corolla XEI 1.8', 2024, 28500.00, 'Nuevo'),
    ('SOL-2025-001246', 'Ana López', 2, 2, 'Mazda', 'CX-5 Grand Touring', 2023, 35200.00, 'Nuevo'),
    ('SOL-2025-001247', 'Pedro Sánchez', 1, 3, 'Chevrolet', 'Tracker Premier', 2022, 24800.00, 'Usado');
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
  "montoAprobado": 24500,
  "tasa": 9.8,
  "observacion": "Crédito aprobado para Toyota Corolla XEI 2024. Cliente con buen historial crediticio.",
  "fechaRespuesta": "2025-11-28T10:25:00"
}
```

**Nota:** Para créditos automotrices aprobados, el sistema calculará automáticamente el plazo y cuota mensual según las políticas de la financiera.

#### Estado: CONDICIONADO

```json
{
  "numeroSolicitud": "SOL-2025-001246",
  "estado": "CONDICIONADO",
  "montoAprobado": 30000,
  "tasa": 11.5,
  "condiciones": [
    "Incrementar entrada al 25% del valor del vehículo (Mazda CX-5)",
    "Presentar garante con ingresos demostrables mínimos de $1,200",
    "Contratar seguro todo riesgo con cobertura contra robo e incendio"
  ],
  "observacion": "Aprobación condicionada para Mazda CX-5 Grand Touring 2023",
  "fechaRespuesta": "2025-11-28T11:00:00"
}
```

#### Estado: REQUIERE_DOCUMENTOS

```json
{
  "numeroSolicitud": "SOL-2025-001247",
  "estado": "REQUIERE_DOCUMENTOS",
  "listaDocumentos": [
    "Matrícula del vehículo Chevrolet Tracker 2022",
    "Certificado de avalúo del vehículo usado",
    "Certificado de ingresos actualizado (no mayor a 30 días)",
    "Comprobante de domicilio reciente"
  ],
  "observacion": "Documentación incompleta para crédito de vehículo usado",
  "fechaRespuesta": "2025-11-28T12:00:00"
}
```

#### Estado: NEGADO

```json
{
  "numeroSolicitud": "SOL-2025-001248",
  "estado": "NEGADO",
  "observacion": "Cliente presenta morosidad en créditos automotrices anteriores. Relación cuota-ingreso supera el 45% permitido.",
  "fechaRespuesta": "2025-11-28T14:00:00"
}
```

#### Estado: EN_PROCESO

```json
{
  "numeroSolicitud": "SOL-2025-001249",
  "estado": "EN_PROCESO",
  "observacion": "Solicitud en evaluación por comité de crédito. Verificando historial de pagos del cliente y tasación del vehículo.",
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

### Validaciones de Campos por Estado

- **APROBADO**: Requiere `montoAprobado` y `tasa`
- **CONDICIONADO**: Requiere `condiciones` (lista) y `montoAprobado`
- **NEGADO**: Requiere `observacion`
- **REQUIERE_DOCUMENTOS**: Requiere `listaDocumentos` (lista)
- **EN_PROCESO**: Sin validaciones estrictas

### Flujo de Estados (Control de Transiciones)

El sistema valida que los cambios de estado sigan un flujo lógico:

**Reglas principales:**
- `APROBADO` y `NEGADO` son estados **finales** (no permiten más cambios)
-  No se puede aprobar un crédito previamente negado
-  No se puede pasar de `REQUIERE_DOCUMENTOS` directo a `APROBADO` (debe reevaluarse)
- `CONDICIONADO` solo puede ir a `APROBADO` o `NEGADO`

**Ejemplo de flujo válido:**
```
EN_PROCESO → REQUIERE_DOCUMENTOS → EN_PROCESO → APROBADO ✓
```

**Ejemplo de flujo inválido:**
```
NEGADO → APROBADO ✗  (Error: crédito negado no puede aprobarse)
```

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


