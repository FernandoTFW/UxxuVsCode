using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Uxxu
{
    /// <summary>
    /// Lógica de interacción para ReporteVenta.xaml
    /// </summary>
    public partial class ReporteVenta : Window
    {
        private List<Venta> ventas;
        private List<DetalleVenta> detalleVenta;
        private UxxuEntities db = new UxxuEntities();
        private DateTime fechaDesde;
        private DateTime fechaHasta;

        public ReporteVenta()
        {
            InitializeComponent();
            ventas = new List<Venta>();
            detalleVenta = new List<DetalleVenta>();
            dgVentas.ItemsSource = ventas;
            dgDetalleVenta.ItemsSource = detalleVenta;
            fechaDesde = DateTime.Now.AddDays(-7);
            fechaHasta = DateTime.Now;
            dpFechaDesde.SelectedDate = fechaDesde;
            dpFechaHasta.SelectedDate = fechaHasta;
            CargarVentas();
        }

        private void BtnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            if (dpFechaDesde.SelectedDate != null && dpFechaHasta.SelectedDate != null)
            {
                fechaDesde = dpFechaDesde.SelectedDate.Value;
                fechaHasta = dpFechaHasta.SelectedDate.Value;

                CargarVentas();
            }
            else
            {
                MessageBox.Show("Debe seleccionar las fechas de filtro");
            }
        }

        private async void CargarVentas()
        {
            ventas.Clear();
            ventas = await db.Venta.Where(v => v.FechaVenta >= fechaDesde && v.FechaVenta <= fechaHasta).Include(v => v.Cliente).ToListAsync();
            dgVentas.ItemsSource = ventas;
        }

        private void DgVentas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVentas.SelectedIndex >= 0)
            {
                Venta ventaSeleccionada = (Venta)dgVentas.SelectedItem;
                CargarDetalleVenta(ventaSeleccionada.IdVenta);
            }
        }

        private async void CargarDetalleVenta(int idVenta)
        {
            detalleVenta.Clear();

            var detalleVentaDb = await db.DetalleVenta.Where(dv => dv.IdVenta == idVenta).Include(dv => dv.Producto).ToListAsync();
            detalleVenta = detalleVentaDb;
            dgDetalleVenta.ItemsSource = detalleVenta;

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
