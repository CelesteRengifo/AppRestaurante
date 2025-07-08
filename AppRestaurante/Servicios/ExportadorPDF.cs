using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Globalization;
using AppRestaurante.Modelos;
using PdfSharpCore.Fonts;

namespace AppRestaurante.Servicios
{
    public class ExportadorPDF
    {
        public async Task<string> GenerarReportePDFAsync(List<ReporteVenta> datos, DateTime? inicio = null, DateTime? fin = null, bool incluirFecha = false)
        {
            // 👉 Registrar el resolver solo si aún no está registrado
            if (GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new CustomFontResolver();
            }

            var documento = new PdfDocument();
            var pagina = documento.AddPage();
            var gfx = XGraphics.FromPdfPage(pagina);

            // ✅ Usa la fuente OpenSans embebida
            var fuenteTitulo = new XFont("OpenSans", 18, XFontStyle.Bold);
            var fuenteSubtitulo = new XFont("OpenSans", 12, XFontStyle.Regular);
            var fuenteContenido = new XFont("OpenSans", 10, XFontStyle.Regular);

            double y = 40;

            gfx.DrawString("Reporte de Platos Vendidos", fuenteTitulo, XBrushes.Black, new XRect(0, y, pagina.Width, 40), XStringFormats.TopCenter);
            y += 30;

            string subtitulo = (inicio.HasValue && fin.HasValue)
                ? (inicio.Value == fin.Value
                    ? $"Reporte del {inicio.Value:dd/MM/yyyy}"
                    : $"Del {inicio.Value:dd/MM/yyyy} al {fin.Value:dd/MM/yyyy}")
                : "Reporte general del año";

            gfx.DrawString(subtitulo, fuenteSubtitulo, XBrushes.Black, new XRect(40, y, pagina.Width - 80, 20), XStringFormats.TopLeft);
            y += 30;

            foreach (var item in datos)
            {
                string linea = $"- {item.Plato}: {item.CantidadVendida} vendidos";

                if (incluirFecha && item.Fecha.HasValue)
                    linea += $" • {item.Fecha.Value:dd/MM/yyyy}";

                gfx.DrawString(linea, fuenteContenido, XBrushes.Black, new XRect(40, y, pagina.Width - 80, 20), XStringFormats.TopLeft);
                y += 20;
            }

            var nombreArchivo = $"Reporte_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);
            using (var stream = File.Create(ruta))
            {
                documento.Save(stream);
            }

            return ruta;
        }

    }
}
