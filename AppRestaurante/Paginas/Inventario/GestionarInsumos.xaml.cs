using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
using Microsoft.Maui.Controls.Shapes;

namespace AppRestaurante.Paginas.Inventario
{
    public partial class GestionarInsumos : ContentPage
    {
        private readonly InsumoService _insumoService;
        private List<Insumo> _todosLosInsumos;

        public GestionarInsumos()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _insumoService = new InsumoService();
            _todosLosInsumos = new List<Insumo>();
            CargarInsumosDesdeApi();
        }

        private async void CargarInsumosDesdeApi()
        {
            _todosLosInsumos = await _insumoService.ObtenerInsumosAsync();

            // Ordenar por ID descendente para que los más recientes estén arriba
            var ordenados = _todosLosInsumos.OrderByDescending(i => i.id).ToList();
            MostrarInsumos(ordenados);
        }

        private void MostrarInsumos(List<Insumo> insumos)
        {
            ListaInsumos.Children.Clear();

            foreach (var insumo in insumos)
            {
                var card = CrearVistaInsumo(insumo);
                ListaInsumos.Children.Add(card);
            }
        }

        private View CrearVistaInsumo(Insumo insumo)
        {
            var nombreLabel = new Label
            {
                Text = insumo.name,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                TextColor = Color.FromArgb("#795548")
            };

            var detalleLabel = new Label
            {
                Text = $"Stock {insumo.stock} {insumo.unidadMedida} • S/ {insumo.precio:0.00}",
                FontSize = 14,
                TextColor = Color.FromArgb("#795548"),
                Margin = new Thickness(0, 5, 0, 0)
            };

            var editarBtn = new Button
            {
                Text = "Editar",
                BackgroundColor = Color.FromArgb("#4E342E"),
                TextColor = Colors.White,
                Padding = new Thickness(10, 5),
                CornerRadius = 10,
                Margin = new Thickness(5, 0)
            };

            editarBtn.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new EditarInsumo(insumo));
            };

            var eliminarBtn = new Button
            {
                Text = "Eliminar",
                BackgroundColor = Color.FromArgb("#795548"),
                TextColor = Colors.White,
                Padding = new Thickness(10, 5),
                CornerRadius = 10,
                Margin = new Thickness(5, 0)
            };

            eliminarBtn.Clicked += async (s, e) =>
            {
                bool confirmado = await DisplayAlert("Confirmar", $"¿Deseas eliminar '{insumo.name}'?", "Sí", "No");
                if (confirmado)
                {
                    bool exito = await _insumoService.EliminarInsumoAsync(insumo.id);
                    if (exito)
                    {
                        _todosLosInsumos.Remove(insumo);
                        MostrarInsumos(_todosLosInsumos);
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el insumo", "OK");
                    }
                }
            };

            // Nuevo Label para el precio total
            var precioTotalLabel = new Label
            {
                Text = $"Total: S/ {(insumo.precio * insumo.stock):0.00}",
                FontSize = 13,
                TextColor = Color.FromArgb("#6D4C41"),
                Margin = new Thickness(0, 0, 0, 5)
            };

            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto)
            },
                RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto), // Fila 0: nombre y botones
                new RowDefinition(GridLength.Auto), // Fila 1: stock + precio
                new RowDefinition(GridLength.Auto)  // Fila 2: total
            },
                VerticalOptions = LayoutOptions.Center
            };

            grid.Add(nombreLabel, 0, 0);
            grid.Add(editarBtn, 1, 0);
            grid.Add(eliminarBtn, 2, 0);

            Grid.SetColumnSpan(detalleLabel, 3);
            grid.Add(detalleLabel, 0, 1);

            Grid.SetColumnSpan(precioTotalLabel, 3);
            grid.Add(precioTotalLabel, 0, 2);


            var border = new Border
            {
                BackgroundColor = Color.FromArgb("#FFF8E1"),
                Padding = 15,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = 20 },
                Content = grid
            };

            return border;
        }

        private void OnBuscarTextoCambiado(object sender, TextChangedEventArgs e)
        {
            var textoBusqueda = e.NewTextValue?.ToLower() ?? "";

            var filtrados = _todosLosInsumos
                .Where(i => i.name.ToLower().Contains(textoBusqueda))
                .OrderByDescending(i => i.id) // ? Aquí agregamos la ordenación
                .ToList();

            MostrarInsumos(filtrados);
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            // Aquí deberías redirigir a la página de agregar insumo
            await Navigation.PushAsync(new AgregarInsumos());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarInsumosDesdeApi();
        }
    }
}
