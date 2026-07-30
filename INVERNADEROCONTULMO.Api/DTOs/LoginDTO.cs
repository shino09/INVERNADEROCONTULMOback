namespace INVERNADEROCONTULMO.Api.DTOs;

// DTO para inicio de sesión
public class LoginDTO
{
    // Correo electrónico del usuario
    public string Email { get; set; }
    // Contraseña del usuario
    public string Password { get; set; }
}

// DTO para respuesta de inicio de sesión
public class LoginResponseDTO
{
    // Identificador del usuario
    public int Id { get; set; }
    // Nombre del usuario
    public string Nombre { get; set; }
    // Correo electrónico del usuario
    public string Email { get; set; }
    // Rol del usuario
    public string Rol { get; set; }
    // Token JWT de autenticación
    public string Token { get; set; }
}

// DTO para crear/actualizar producto
public class ProductoDTO
{
    // Nombre del producto
    public string Nombre { get; set; }
    // Descripción del producto
    public string Descripcion { get; set; }
    // Precio de compra del producto
    public decimal PrecioCompra { get; set; }
    // Precio de venta del producto
    public decimal PrecioVenta { get; set; }
    // Stock actual del producto
    public int StockActual { get; set; }
    // Stock mínimo del producto
    public int StockMinimo { get; set; }
    // Categoría del producto
    public string Categoria { get; set; }
}

// DTO para crear/actualizar cliente
public class ClienteDTO
{
    // Nombre del cliente
    public string Nombre { get; set; }
    // Documento del cliente
    public string Documento { get; set; }
    // Correo del cliente
    public string Email { get; set; }
    // Teléfono del cliente
    public string Telefono { get; set; }
    // Dirección del cliente
    public string Direccion { get; set; }
}

// DTO para crear una venta
public class VentaDTO
{
    // Identificador del cliente
    public int ClienteId { get; set; }
    // Método de pago
    public string MetodoPago { get; set; }
    // Detalles de la venta
    public List<DetalleVentaDTO> Detalles { get; set; }
}

// DTO para detalle de venta
public class DetalleVentaDTO
{
    // Identificador del producto
    public int ProductoId { get; set; }
    // Cantidad a vender
    public int Cantidad { get; set; }
}

// DTO para crear un pedido
public class PedidoDTO
{
    // Identificador del cliente
    public int ClienteId { get; set; }
    // Detalles del pedido
    public List<DetallePedidoDTO> Detalles { get; set; }
}

// DTO para detalle de pedido
public class DetallePedidoDTO
{
    // Identificador del producto
    public int ProductoId { get; set; }
    // Cantidad a pedir
    public int Cantidad { get; set; }
}

// DTO para crear una compra
public class CompraDTO
{
    // Nombre del proveedor
    public string Proveedor { get; set; }
    // Detalles de la compra
    public List<DetalleCompraDTO> Detalles { get; set; }
}

// DTO para detalle de compra
public class DetalleCompraDTO
{
    // Identificador del producto
    public int ProductoId { get; set; }
    // Cantidad a comprar
    public int Cantidad { get; set; }
    // Precio unitario de compra
    public decimal PrecioUnitario { get; set; }
}
