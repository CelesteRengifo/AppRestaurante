using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Globalization;
using AppRestaurante.Modelos;

namespace AppRestaurante.Servicios
{
    public class ExportadorExcel
    {
        public async Task<string> ExportarAExcelAsync(List<ReporteVenta> datos, DateTime? inicio = null, DateTime? fin = null, bool incluirFecha = false)
        {
            var nombreArchivo = $"Reporte_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Platos Vendidos");

                // Título
                ws.Cell("A1").Value = "Reporte de Platos Vendidos";
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A1").Style.Font.FontSize = 18;
                ws.Range("A1:C1").Merge();

                // Subtítulo
                string subtitulo = (inicio.HasValue && fin.HasValue)
                    ? (inicio.Value == fin.Value
                        ? $"Reporte del {inicio.Value:dd/MM/yyyy}"
                        : $"Del {inicio.Value:dd/MM/yyyy} al {fin.Value:dd/MM/yyyy}")
                    : "Reporte general del año";

                ws.Cell("A2").Value = subtitulo;
                ws.Range("A2:C2").Merge();

                // Encabezados
                int col = 1;
                ws.Cell(4, col++).Value = "Plato";
                ws.Cell(4, col++).Value = "Cantidad Vendida";
                if (incluirFecha) ws.Cell(4, col++).Value = "Fecha";

                ws.Range("A4:C4").Style.Font.Bold = true;

                int fila = 5;
                foreach (var item in datos)
                {
                    col = 1;
                    ws.Cell(fila, col++).Value = item.Plato;
                    ws.Cell(fila, col++).Value = item.CantidadVendida;
                    if (incluirFecha && item.Fecha.HasValue)
                        ws.Cell(fila, col++).Value = item.Fecha.Value.ToString("dd/MM/yyyy");
                    fila++;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(ruta);
            }

            return ruta;
        }

        public async Task<string> ExportarACsvAsync(List<ReporteVenta> datos, bool incluirFecha = false)
        {
            var nombreArchivo = $"Reporte_{DateTime.Now:yyyyMMddHHmmss}.csv";
            var ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

            using (var writer = new StreamWriter(ruta, false))
            {
                var encabezado = "Plato,CantidadVendida";
                if (incluirFecha) encabezado += ",Fecha";
                writer.WriteLine(encabezado);

                foreach (var item in datos)
                {
                    var fila = $"{item.Plato},{item.CantidadVendida}";
                    if (incluirFecha && item.Fecha.HasValue)
                        fila += $",{item.Fecha.Value:dd/MM/yyyy}";
                    writer.WriteLine(fila);
                }
            }

            return ruta;
        }
    }
}

