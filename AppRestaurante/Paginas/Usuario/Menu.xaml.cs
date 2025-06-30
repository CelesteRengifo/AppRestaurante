using AppRestaurante.Modelos.Clases_pedidos;
using AppRestaurante.Servicios;
using System.Collections.ObjectModel;

namespace AppRestaurante.Paginas.Usuario;

public partial class Menu : ContentPage
{
    private readonly AuthService _authService = new(); // USAMOS AuthService

    ObservableCollection<Plato> platos = new();
    private ObservableCollection<Mesas> mesas = new();

    public Menu()
    {
        InitializeComponent();
        CargarMenuDesdeApi();
        CargarMesasDesdeApi();
    }

    private async Task CargarMesasDesdeApi()
    {
        try
        {
            var lista = await _authService.ObtenerMesasAsync(); // Cambiado

            mesas.Clear();
            foreach (var mesa in lista)
            {
                mesas.Add(mesa);
            }

            MesaPicker.ItemsSource = mesas;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error al cargar mesas", ex.Message, "OK");
        }
    }

    private async Task CargarMenuDesdeApi()
    {
        try
        {
            var lista = await _authService.ObtenerPlatosAsync(); // Cambiado

            var listaOrdenada = lista.OrderBy(p => p.Name).ToList();

            platos.Clear();
            foreach (var plato in listaOrdenada)
            {
                platos.Add(plato);
            }

            PlatosCollection.ItemsSource = platos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void StepperPlato_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var stepper = (Stepper)sender;
        var plato = (Plato)stepper.BindingContext;

        plato.Cantidad = (int)e.NewValue;
        OnPropertyChanged(nameof(plato.TextoCantidad));
    }

    private async void OnRealizarPedidoClicked(object sender, EventArgs e)
    {
        var mesaSeleccionada = (Mesas)MesaPicker.SelectedItem;

        if (mesaSeleccionada == null)
        {
            await DisplayAlert("Error", "Seleccione una mesa.", "OK");
            return;
        }

        int idMesa = mesaSeleccionada.Id;

        var platosSeleccionados = platos.Where(p => p.Cantidad > 0).ToList();

        if (!platosSeleccionados.Any())
        {
            await DisplayAlert("Error", "No se seleccion¨® ning¨²n plato.", "OK");
            return;
        }

        var resumenPlatos = platosSeleccionados
            .Select(p => $"{p.Name} x{p.Cantidad} - S/. {(p.Precio * p.Cantidad):F2}");

        var total = platosSeleccionados.Sum(p => p.Cantidad * p.Precio);

        try
        {
            foreach (var plato in platosSeleccionados)
            {
                var pedido = new Pedidos
                {
                    Plato = plato.Id,
                    Empleado = 1, // Puedes cambiar seg¨²n el usuario logueado
                    Mesa = idMesa,
                    Cantidad = plato.Cantidad,
                    Fecha = DateTime.Now,
                    Estado = "ordenado"
                };

                await _authService.EnviarPedidoAsync(pedido); // Cambiado
            }

            string mensaje = $"Mesa: {mesaSeleccionada.Id}\n\nPedido:\n" +
                             string.Join("\n", resumenPlatos) +
                             $"\n\nTotal: S/. {total:F2}";

            await DisplayAlert("Pedido realizado", mensaje, "OK");

            foreach (var p in platos)
            {
                p.Cantidad = 0;
            }

            PlatosCollection.ItemsSource = null;
            PlatosCollection.ItemsSource = platos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error al enviar pedido", ex.Message, "OK");
        }
    }
}