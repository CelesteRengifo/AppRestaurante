﻿using System.ComponentModel;

namespace AppRestaurante.Modelos.Clases_pedidos
{
    public class Plato : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public List<Dia> Dias { get; set; }


        private int cantidad;
        public int Cantidad
        {
            get => cantidad;
            set
            {
                if (cantidad != value)
                {
                    cantidad = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TextoCantidad));
                }
            }
        }

        public string TextoCantidad => $"Cantidad: {Cantidad}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
