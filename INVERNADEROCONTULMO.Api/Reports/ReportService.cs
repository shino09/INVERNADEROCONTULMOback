using DinkToPdf;
using DinkToPdf.Contracts;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Reports;

// Servicio de generación de reportes PDF
public interface IReportService
{
    // Genera PDF de factura para una venta
    byte[] GenerateFacturaPdf(Venta venta);
    // Genera PDF del libro diario
    byte[] GenerateLibroDiarioPdf(IEnumerable<AsientoContable> asientos, DateTime? desde, DateTime? hasta);
    // Genera PDF del libro mayor
    byte[] GenerateLibroMayorPdf(IEnumerable<object> cuentas, DateTime? desde, DateTime? hasta);
}

// Implementación del servicio de reportes PDF
public class ReportService : IReportService
{
    // Conversor HTML a PDF
    private readonly IConverter _converter;
    // Constructor que inyecta dependencias
    public ReportService(IConverter converter) => _converter = converter;

    // Genera un PDF de factura para una venta con todos sus detalles
    public byte[] GenerateFacturaPdf(Venta venta)
    {
        var detallesHtml = string.Join("", venta.Detalles.Select(d => $@"
            <tr><td>{d.Producto?.Nombre}</td><td>{d.Cantidad}</td><td>S/ {d.PrecioUnitario:N2}</td><td>S/ {d.Subtotal:N2}</td></tr>"));
        var html = $@"
<!DOCTYPE html><html><head><meta charset='utf-8'/>
<style>body{{font-family:Arial;padding:20px;}}h1{{color:#2e7d32;border-bottom:2px solid #2e7d32;}}.header{{text-align:center;margin-bottom:30px;}}table{{width:100%;border-collapse:collapse;margin:20px 0;}}th,td{{border:1px solid #ddd;padding:8px;text-align:left;}}th{{background-color:#2e7d32;color:white;}}.total{{font-size:18px;font-weight:bold;text-align:right;margin-top:20px;}}.footer{{margin-top:40px;text-align:center;font-size:12px;color:#666;}}</style></head><body>
<div class='header'><h1>INVERNADEROCONTULMO</h1><h2>FACTURA {venta.NumeroFactura}</h2></div>
<p><strong>Cliente:</strong> {venta.Cliente?.Nombre}</p><p><strong>Fecha:</strong> {venta.FechaVenta:dd/MM/yyyy HH:mm}</p>
<table><thead><tr><th>Producto</th><th>Cant.</th><th>P.Unit</th><th>Subtotal</th></tr></thead><tbody>{detallesHtml}</tbody></table>
<div class='total'><p>Subtotal: S/ {venta.Subtotal:N2}</p><p>Impuesto (18%): S/ {venta.Impuesto:N2}</p><p>TOTAL: S/ {venta.Total:N2}</p></div>
<p><strong>Método de Pago:</strong> {venta.MetodoPago}</p>
<div class='footer'><p>Gracias por su compra</p></div></body></html>";
        return ConvertHtmlToPdf(html);
    }

    // Genera un PDF del libro diario con asientos filtrados por fecha
    public byte[] GenerateLibroDiarioPdf(IEnumerable<AsientoContable> asientos, DateTime? desde, DateTime? hasta)
    {
        var rows = string.Join("", asientos.Select(a => $@"
            <tr><td>{a.FechaAsiento:dd/MM/yyyy}</td><td>{a.NumeroAsiento}</td><td>{a.CuentaContable}</td><td>{a.Descripcion}</td><td>S/ {a.Debe:N2}</td><td>S/ {a.Haber:N2}</td></tr>"));
        var periodo = $"{(desde?.ToString("dd/MM/yyyy") ?? "Inicio")} - {(hasta?.ToString("dd/MM/yyyy") ?? "Fin")}";
        var html = $@"
<!DOCTYPE html><html><head><meta charset='utf-8'/>
<style>body{{font-family:Arial;padding:20px;}}h1{{color:#2e7d32;}}table{{width:100%;border-collapse:collapse;}}th,td{{border:1px solid #ddd;padding:6px;text-align:left;font-size:11px;}}th{{background-color:#2e7d32;color:white;}}</style></head><body>
<h1>LIBRO DIARIO</h1><p>Periodo: {periodo}</p>
<table><thead><tr><th>Fecha</th><th>Asiento</th><th>Cuenta</th><th>Descripción</th><th>Debe</th><th>Haber</th></tr></thead><tbody>{rows}</tbody></table></body></html>";
        return ConvertHtmlToPdf(html);
    }

    // Genera un PDF del libro mayor con cuentas agrupadas y saldos
    public byte[] GenerateLibroMayorPdf(IEnumerable<object> cuentas, DateTime? desde, DateTime? hasta)
    {
        var rows = "";
        foreach (var c in cuentas)
        {
            var cuenta = (dynamic)c;
            rows += $@"<tr><td colspan='5' style='background:#e8f5e9;font-weight:bold;'>{cuenta.Cuenta}</td></tr>
<tr><td colspan='3'>Debe: S/ {cuenta.SaldoDebe:N2}</td><td>Haber: S/ {cuenta.SaldoHaber:N2}</td><td>Saldo: S/ {cuenta.SaldoFinal:N2}</td></tr>";
            foreach (var m in cuenta.Movimientos)
                rows += $"<tr><td>{m.FechaAsiento:dd/MM/yyyy}</td><td>{m.NumeroAsiento}</td><td>{m.Descripcion}</td><td>S/ {m.Debe:N2}</td><td>S/ {m.Haber:N2}</td></tr>";
        }
        var periodo = $"{(desde?.ToString("dd/MM/yyyy") ?? "Inicio")} - {(hasta?.ToString("dd/MM/yyyy") ?? "Fin")}";
        var html = $@"
<!DOCTYPE html><html><head><meta charset='utf-8'/>
<style>body{{font-family:Arial;padding:20px;}}h1{{color:#2e7d32;}}table{{width:100%;border-collapse:collapse;}}th,td{{border:1px solid #ddd;padding:5px;text-align:left;font-size:10px;}}th{{background-color:#2e7d32;color:white;}}</style></head><body>
<h1>LIBRO MAYOR</h1><p>Periodo: {periodo}</p>
<table><thead><tr><th>Fecha</th><th>Asiento</th><th>Descripción</th><th>Debe</th><th>Haber</th></tr></thead><tbody>{rows}</tbody></table></body></html>";
        return ConvertHtmlToPdf(html);
    }

    // Convierte una cadena HTML a PDF usando DinkToPdf
    private byte[] ConvertHtmlToPdf(string html)
    {
        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait },
            Objects = { new ObjectSettings { HtmlContent = html } }
        };
        return _converter.Convert(doc);
    }
}
