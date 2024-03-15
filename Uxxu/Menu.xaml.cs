using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnProvider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var proveedorPage = new ProveedorPage();
            proveedorPage.Show();
            this.Close();
        }

        private void btnVenta_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ventaPage = new VentaPage();
            ventaPage.Show();
            this.Close();
        }

        private void btnProduct_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var productoPage = new ProductoPage();
            productoPage.Show();
            this.Close();
        }

        private void btnReport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var productoPage = new ReporteVenta();
            productoPage.Show();
            this.Close();
        }
    }
}
