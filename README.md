# SilSalon - ERP para Salón de Belleza ???????

![SilSalon Cover](https://via.placeholder.com/1200x400?text=SilSalon+ERP+-+Sistema+de+Gesti%C3%B3n+para+Salones+de+Belleza) *[Reemplaza con imagen real de portada]*

SilSalon es un sistema de planificación integral (ERP) moderno e intuitivo, diseñado específicamente para optimizar y gestionar la administración de salones de belleza. Desarrollado con la flexibilidad y potencia de **.NET 9** y **Blazor**, este sistema provee las herramientas necesarias para digitalizar por completo las operaciones del negocio.

## ?? Características Principales

*   **Punto de Venta (POS):** Registro rápido e intuitivo de transacciones por venta de productos y prestación de servicios.
*   **Ventas y Facturación:** Gestión de órdenes, aplicación de descuentos e historial de ventas detallado (`Sale`, `SaleItem`).
*   **Gestión de Inventario y Catálogo:** Control especializado de stock, registro de productos (`Product`) con soporte para múltiples variantes, tamaños o colores (`ProductVariant`).
*   **Servicios del Salón:** Definición de un catálogo de servicios del salón (`Service`) configurables con precios e información relevante.
*   **Relación con Clientes (CRM Básico):** Ficha de clientes (`Customer`), seguimiento de preferencias y fidelización.
*   **Compras y Proveedores:** Mapeo de ingresos de mercancía (`Purchase`, `PurchaseItem`) y cartera de proveedores (`Supplier`).
*   **Control Financiero:** Libro diario de caja, con módulo dedicado para registro de ingresos (`Income`) y egresos u honorarios (`Expense`).

## ?? Capturas de Pantalla del Sistema

> **Nota:** Recuerda actualizar los enlaces de estas imágenes (actualmente placeholders) con capturas de pantalla reales antes de publicar tu portafolio.

### Dashboard / Panel Principal
![Dashboard](https://via.placeholder.com/800x450/4A90E2/FFFFFF?text=Dashboard+Principal)
*Visualización global de ingresos, gastos y rendimiento general del salón del día.*

### Punto de Venta (POS)
![Punto de Venta](https://via.placeholder.com/800x450/4A90E2/FFFFFF?text=M%C3%B3dulo+de+Ventas+(POS))
*Interfaz de facturación fácil de usar.*

### Gestión de Inventario
![Inventario](https://via.placeholder.com/800x450/4A90E2/FFFFFF?text=Gesti%C3%B3n+de+Inventario)
*Catálogo de productos y seguimiento en tiempo real del stock.*

### Módulo Financiero
![Finanzas](https://via.placeholder.com/800x450/4A90E2/FFFFFF?text=Ingresos+y+Gastos)
*Registro y balance de ingresos y gastos operativos de la estética.*

## ??? Arquitectura del Sistema

El desarrollo de este sistema está fuertemente guiado por los principios de **Clean Architecture** (Arquitectura Limpia). Esto garantiza un software modular, testeable, mantenible y verdaderamente escalable.

El flujo de dependencias sigue la regla de apuntar siempre hacia el centro (el núcleo del negocio), evitando que cambios tecnológicos afecten la lógica empresarial.

La estructuración se divide de la siguiente manera:

1.  **Capa de Dominio (`Domain`):**
    *   **Núcleo Empresarial:** Contiene las entidades principales sin ninguna dependencia de librerías externas o tecnológicas (`Customer`, `Product`, `Service`, `Sale`, etc.).
    *   **Reglas Base:** Aquí se asientan enumeradores (`Enums`) y lógica intrínseca a los actores del salón de belleza.

2.  **Capa de Aplicación (`Application`):**
    *   **Casos de Uso:** Orquesta las reglas de negocio de la aplicación (Ej: un proceso completo de venta).
    *   **Servicios y Contratos:** Posee la carpeta `Interfaces` que define los contratos (abstracciones) para repositorios que las capas exteriores deberán implementar.
    *   **Transferencia de Datos:** Contiene todos los `DTOs` (Data Transfer Objects) y `Services` operando como puente.

3.  **Capa de Infraestructura (`Infrastructure`):**
    *   **Persistencia de datos:** Contiene el módulo `Data` y los `Repositories` reales. 
    *   Implementa estrictamente los contratos definidos por la capa de Aplicación. Su objetivo exclusivo es interactuar con agentes externos.

4.  **Capa de Presentación / UI (`Components` & `wwwroot`):**
    *   Desarrollada integralmente en **Blazor** (.NET 9).
    *   Contiene la maquetación web, estilos, diseño responsivo y la inyección de dependencias necesarias.
    *   Recibe la interacción del usuario final e invoca los métodos correspondientes expuestos por la capa de Aplicación.

## ??? Stack Tecnológico

*   **Framework Base:** .NET 9
*   **Arquitectura de UI:** Blazor (Componentes Web interactivos mediante C#)
*   **Aproximación Estructural:** Clean Architecture + Repository Pattern
*   **Contenedores:** Docker (Aplicación empaquetada lista para entornos nativos en la nube, con configuración en `Dockerfile`)

---

*Creado y diseñado para organizar y optimizar tareas diarias de administración en un salón de belleza.*