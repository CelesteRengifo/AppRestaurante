using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Fonts;
using System.IO;
using System.Reflection;

namespace AppRestaurante.Servicios
{
    public class CustomFontResolver : IFontResolver
    {
        public string DefaultFontName => "OpenSans#";

        public byte[] GetFont(string faceName)
        {
            var assembly = typeof(CustomFontResolver).GetTypeInfo().Assembly;

            // Reemplaza con el nombre correcto del recurso embebido
            var resource = "AppRestaurante.Fonts.OpenSans-Regular.ttf";

            using Stream stream = assembly.GetManifestResourceStream(resource);
            if (stream == null)
                throw new Exception($"No se encontró el recurso embebido: {resource}");

            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo("OpenSans#");
        }
    }
}


