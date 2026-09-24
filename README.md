Sistema de Inventarios
Aplicación web para la gestión de inventario desarrollada con Angular, ASP.NET Core Web API, Entity Framework Core y SQL Server.
El sistema permite administrar productos y categorías, controlar existencias mediante movimientos de entrada y salida, y proteger el acceso mediante autenticación basada en JWT.
________________________________________
Tecnologías
Frontend
•	Angular 22
•	TypeScript
•	HTML5
•	CSS3
•	Angular Reactive Forms
•	Angular Router
•	HttpClient
•	Interceptors
•	Route Guards
Backend
•	.NET 10
•	ASP.NET Core Web API
•	Entity Framework Core
•	SQL Server
•	JWT Bearer Authentication
•	ASP.NET Core PasswordHasher
•	OpenAPI
Herramientas
•	Git
•	GitHub
•	Visual Studio Code
•	SQL Server Management Studio
•	PowerShell
________________________________________
Funcionalidades
Autenticación
•	Inicio de sesión mediante usuario y contraseña.
•	Contraseñas almacenadas mediante hash.
•	Generación de tokens JWT.
•	Protección de rutas mediante Angular Guards.
•	Envío automático del JWT mediante HTTP Interceptor.
•	Protección de endpoints mediante [Authorize].
•	Cierre de sesión.
•	Detección de expiración del token.
Productos
•	Listar productos.
•	Consultar un producto por ID.
•	Crear productos.
•	Editar productos.
•	Eliminar productos.
•	Validación de campos.
•	Asociación de productos con categorías.
Categorías
•	Crear categorías.
•	Listar categorías.
•	Consultar una categoría.
•	Eliminar categorías.
•	Relación uno a muchos entre categorías y productos.
Movimientos de inventario
•	Registrar entradas.
•	Registrar salidas.
•	Actualización automática del stock.
•	Validación de stock disponible.
•	Registro histórico de movimientos.
•	Registro del stock anterior y resultante.
•	Registro de fecha y motivo.
________________________________________

Arquitectura
El proyecto utiliza una arquitectura separada entre frontend, backend y base de datos:
Angular (Frontend)
        ↓
ASP.NET Core Web API (Backend)
        ↓
Entity Framework Core
        ↓
SQL Server (Base de datos)

Flujo de productosUsuario
  ↓
Página de Productos
  ↓
ProductosService
  ↓
HTTP Request
  ↓
ProductosController
  ↓
Entity Framework Core
  ↓
SQL Server
  ↓
Respuesta JSON
  ↓
Angular muestra los productos


C:\USERS\CHRIS\DOCUMENTS\INVENTARIO\INVENTARIO-ANGULAR\SRC\APP
|   app.config.ts
|   app.css
|   app.html
|   app.routes.ts
|   app.spec.ts
|   app.ts
|
+---core
|   +---guards
|   |       auth.guard.ts
|   |
|   +---interceptors
|   |       auth.interceptor.ts
|   |
|   +---models
|   |       auth.model.ts
|   |
|   \---services
|           auth.service.ts
|
+---features
|   +---auth
|   |   \---login
|   |           login.css
|   |           login.html
|   |           login.spec.ts
|   |           login.ts
|   |
|   +---dashboard
|   |       dashboard.css
|   |       dashboard.html
|   |       dashboard.spec.ts
|   |       dashboard.ts
|   |
|   \---productos
|       +---components
|       +---models
|       |       categoria.cs
|       |       categoria.model.ts
|       |       producto.cs
|       |       producto.model.ts
|       |       usuario.cs
|       |
|       +---pages
|       |   +---categorias
|       |   |       categorias.css
|       |   |       categorias.html
|       |   |       categorias.spec.ts
|       |   |       categorias.ts
|       |   |
|       |   \---productos
|       |           productos.css
|       |           productos.html
|       |           productos.spec.ts
|       |           productos.ts
|       |
|       \---services
|               categorias.ts
|               productos.spec.ts
|               productos.ts
|
\---shared
    +---components
    +---directives
    \---pipes


Modelo de datos
La base de datos utiliza las siguientes entidades:
Usuario
--------
Id
NombreUsuario
PasswordHash


Categoria
---------
Id
Nombre
Descripcion


Producto
--------
Id
Codigo
Nombre
Descripcion
Precio
Stock
CategoriaId


Movimiento
---------
Id
ProductoId
Tipo
Cantidad
StockAnterior
StockResultante
Motivo
Fecha
Relaciones:
Categoria
   │
   │ 1
   │
   └───────────< N
                 Producto
                    │
                    │ 1
                    │
                    └───────────< N
                                  Movimiento
________________________________________
Requisitos
Antes de ejecutar el proyecto necesitas tener instalado:
•	Node.js compatible con Angular 22.
•	Angular CLI.
•	.NET 10 SDK.
•	SQL Server.
•	SQL Server Management Studio (opcional, recomendado).
•	Git.
Puedes comprobar las versiones:
node --version
npm --version
ng version
dotnet --version
________________________________________
Instalación
1. Clonar el repositorio
git clone https://github.com/TU_USUARIO/Inventario.git
cd Inventario
________________________________________
2. Configurar SQL Server
Crear una base de datos llamada:
InventarioDb
La aplicación utiliza actualmente una cadena de conexión similar a:
"DefaultConnection": "Server=localhost;Database=InventarioDb;Trusted_Connection=True;TrustServerCertificate=True;"
Si utilizas otra instancia de SQL Server, modifica el valor de Server.
________________________________________
3. Configurar el backend
Entrar al proyecto:
cd Inventario.Api
Restaurar dependencias:
dotnet restore
Aplicar las migraciones:
dotnet ef database update
Ejecutar la API:
dotnet run
La API está configurada actualmente para ejecutarse en:
http://localhost:5062
OpenAPI:
http://localhost:5062/openapi/v1.json
________________________________________
4. Configurar Angular
En otra terminal:
cd Inventario-angular
npm install
Ejecutar Angular:
ng serve
La aplicación estará disponible normalmente en:
http://localhost:4200
________________________________________
Usuario inicial
Para crear un usuario de prueba se puede utilizar el endpoint:
POST /api/auth/registrar
Ejemplo:
{
  "nombreUsuario": "admin",
  "password": "123456"
}
Después utilizar:
POST /api/auth/login
con:
{
  "nombreUsuario": "admin",
  "password": "123456"
}
La respuesta contiene un JWT:
{
  "token": "eyJ...",
  "usuario": "admin"
}
________________________________________
Endpoints
Autenticación
Registrar usuario
POST /api/auth/registrar
Iniciar sesión
POST /api/auth/login
________________________________________
Productos
Todos los endpoints requieren autenticación.
Obtener productos
GET /api/productos
Obtener un producto
GET /api/productos/{id}
Crear producto
POST /api/productos
Ejemplo:
{
  "codigo": "LAP-002",
  "nombre": "Laptop Dell",
  "descripcion": "Laptop para oficina",
  "precio": 18000,
  "stock": 10,
  "categoriaId": 1
}
Actualizar producto
PUT /api/productos/{id}
Eliminar producto
DELETE /api/productos/{id}
________________________________________
Categorías
Obtener categorías
GET /api/categorias
Obtener categoría
GET /api/categorias/{id}
Crear categoría
POST /api/categorias
Ejemplo:
{
  "nombre": "Laptops",
  "descripcion": "Computadoras portátiles"
}
Eliminar categoría
DELETE /api/categorias/{id}
________________________________________
Movimientos
Obtener movimientos
GET /api/movimientos
Registrar movimiento
POST /api/movimientos
Entrada:
{
  "productoId": 1,
  "tipo": "Entrada",
  "cantidad": 5,
  "motivo": "Compra a proveedor"
}
Salida:
{
  "productoId": 1,
  "tipo": "Salida",
  "cantidad": 3,
  "motivo": "Venta"
}
Las salidas son rechazadas cuando la cantidad solicitada supera el stock disponible.
________________________________________
Seguridad
La aplicación implementa autenticación mediante JWT.
Las rutas de Angular están protegidas mediante:
auth.guard.ts
Las peticiones HTTP incluyen automáticamente el token mediante:
auth.interceptor.ts
Los endpoints protegidos utilizan:
[Authorize]
Las contraseñas no se almacenan en texto plano, sino mediante hash utilizando PasswordHasher.
________________________________________
Persistencia
Entity Framework Core se utiliza como ORM para interactuar con SQL Server.
Las modificaciones estructurales de la base de datos se gestionan mediante migraciones:
dotnet ef migrations add NombreMigracion
dotnet ef database update
Las migraciones actuales incluyen cambios relacionados con:
InitialCreate
AgregarUsuarios
AgregarCategorias
AgregarMovimientos
________________________________________
Control de inventario
El stock no depende únicamente de editar el valor manualmente.
Los movimientos generan cambios sobre el stock:
Entrada
Stock anterior + Cantidad
        ↓
Stock resultante


Salida
Stock anterior - Cantidad
        ↓
Stock resultante
Ejemplo:
Stock inicial: 10

Entrada de 5
10 + 5 = 15

Salida de 3
15 - 3 = 12
Cada operación queda registrada en Movimientos.
________________________________________
Próximas mejoras
El proyecto está preparado para continuar creciendo. Algunas mejoras previstas son:
•	Dashboard con estadísticas reales.
•	Gestión de usuarios y roles.
•	Búsqueda y filtrado de productos.
•	Paginación.
•	Ordenamiento de productos.
•	Historial de movimientos con filtros.
•	Reportes de inventario.
•	Alertas de stock bajo.
•	Componentes Angular reutilizables.
•	Manejo centralizado de errores.
•	Configuración mediante variables de entorno.
•	Pruebas unitarias e integración.
•	Docker.
•	Despliegue en la nube.
________________________________________
Objetivo del proyecto
Este proyecto fue desarrollado como una aplicación práctica para aplicar conocimientos de:
•	Desarrollo frontend.
•	Desarrollo backend.
•	APIs REST.
•	Bases de datos relacionales.
•	Entity Framework Core.
•	Autenticación y autorización.
•	JWT.
•	Arquitectura de software.
•	Git y GitHub.
El objetivo es construir progresivamente un sistema de inventarios completo utilizando tecnologías modernas y buenas prácticas de desarrollo.
________________________________________

