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
    public partial class VentaPage : Window
    {
        private Cliente cliente;
        private List<VentaItem> ventaItems;
        int cantidad = 0;
        List<Producto> products;
        private decimal? total;
        private UxxuEntities db = new UxxuEntities();

        public VentaPage()
        {
            InitializeComponent();
            ventaItems = new List<VentaItem>();
            dataGridProductos.ItemsSource = ventaItems;
            txtCliente.Text = ""; // Set initial client text
            total = 0;
            txbCantidad.Text = cantidad.ToString();
            CargarProductos();
        }

        private void CargarProductos()
        {
            var productosDb = db.Producto.ToList();
            products = productosDb;
            cmbProducts.ItemsSource = products; // Bind products to combobox
            cmbProducts.SelectedIndex = 0;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string nombreCliente = txtCliente.Text;

            if (string.IsNullOrEmpty(nombreCliente))
            {
                MessageBox.Show("Ingrese el nombre del cliente");
                return;
            }

            cliente = await db.Cliente.FirstOrDefaultAsync(c => c.NIT.Contains(nombreCliente));

            if (cliente == null)
            {
                AddClient();
            }
            else
            {
                txtCliente.Text = cliente.Nombre + " " + cliente.Apellido;
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txbCantidad.Text, out int quantity))
            {
                cantidad = quantity-1;
            }
            else{
                cantidad--;
            }
            txbCantidad.Text = cantidad.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txbCantidad.Text, out int quantity))
            {
                cantidad = quantity+1;
            }
            else
            {
                cantidad++;
            }
            txbCantidad.Text = cantidad.ToString();
        }

        private void txbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txbCantidad.Text))
            {
                return;
            }
            if(int.TryParse(txbCantidad.Text, out int quan))
            {
                int cantidad = int.Parse(txbCantidad.Text);

                if (cantidad < 0)
                {
                    txbCantidad.Text = "0";
                }
            }
            else
            {
                txbCantidad.Text = "0";
            }
            
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Producto producto = (Producto)cmbProducts.SelectedItem;

            if (producto != null && cliente != null)
            {

                var existingItem = ventaItems.FirstOrDefault(i => i.Producto.IdProducto == producto.IdProducto);

                if (existingItem != null)
                {
                    // Si el producto ya existe, solo aumentar la cantidad
                    existingItem.Cantidad += cantidad;
                }
                else
                {
                    // Si el producto no existe, agregarlo a la lista
                    ventaItems.Add(new VentaItem(producto, cantidad));
                }
                dataGridProductos.Items.Refresh(); // Update data grid
                CalcularTotal();
            }
            else
            {
                if (cliente == null)
                {
                    MessageBox.Show("Debe buscar un cliente primero");
                }
            }
        }

        private void CalcularTotal()
        {
            total = 0;

            foreach (var ventaItem in ventaItems)
            {
                total += ventaItem.Producto.Precio * ventaItem.Cantidad;
            }

            var totalText = total.ToString() + " Bs.";
            txtTotal.Text = totalText; // Update total text on button
        }

        private async void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if (ventaItems.Count == 0)
            {
                MessageBox.Show("Debe agregar productos a la venta");
                return;
            }
            if (cliente == null)
            {
                AddClient();
                return;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Venta venta = new Venta()
                    {
                        TotalVenta = total,
                        IdUsuarioVenta = Logued.User.IdUsuarioVenta,
                        IdCliente = cliente.IdCliente,
                        FechaVenta = DateTime.Now
                    };
                    db.Venta.Add(venta);
                    await db.SaveChangesAsync();
                    int idVenta = venta.IdVenta;
                    foreach (var ventaItem in ventaItems)
                    {
                        DetalleVenta detalleVenta = new DetalleVenta(){ 
                            IdVenta = idVenta, 
                            IdProducto= ventaItem.Producto.IdProducto, 
                            Cantidad = ventaItem.Cantidad, 
                            PrecioVenta= ventaItem.Producto.Precio };
                        db.DetalleVenta.Add(detalleVenta);

                        // Update stock of the product
                        var productoToUpdate = db.Producto.Find(ventaItem.Producto.IdProducto);
                        productoToUpdate.Stock -= ventaItem.Cantidad;
                        await db.SaveChangesAsync();
                    }

                    transaction.Commit();
                    MessageBox.Show("Venta registrada exitosamente");
                    LimpiarVenta();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al registrar la venta: " + ex.Message);
                }
            }
        }

        private void LimpiarVenta()
        {
            txtCliente.Text = "";
            cliente = null;
            ventaItems.Clear();
            dataGridProductos.Items.Refresh();
            total = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        void AddClient()
        {
            // Mostrar un MessageBox para obtener la información del cliente
            var result = MessageBox.Show(
              "Ingrese la información del nuevo cliente:",
              "Nuevo Cliente",
              MessageBoxButton.OKCancel,
              MessageBoxImage.Information,
              MessageBoxResult.None,
              MessageBoxOptions.DefaultDesktopOnly);

            if (result == MessageBoxResult.OK)
            {
                string nombre = "";
                string apellido = "";
                string nit = "";
                string email = "";
                string telefono = "";
                InputDialog dialog = new InputDialog();
                dialog.Title = "Nuevo Cliente";
                dialog.Labels = new string[] { "Nombre", "Apellido", "NIT", "Email", "Teléfono" };
                dialog.TextBoxes = new TextBox[] { new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox() };
                dialog.Init();
                if (dialog.ShowDialog() == true)
                {
                    nombre = dialog.TextBoxes[0].Text;
                    apellido = dialog.TextBoxes[1].Text;
                    nit = dialog.TextBoxes[2].Text;
                    email = dialog.TextBoxes[3].Text;
                    telefono = dialog.TextBoxes[4].Text;
                }

                // Validar los datos del cliente
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(nit) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telefono))
                {
                    MessageBox.Show("Debe completar todos los campos");
                    return;
                }

                // Agregar el nuevo cliente a la base de datos
                var cliente = new Cliente() {
                    Nombre = nombre, 
                    Apellido =  apellido, 
                    NIT =  nit, 
                    Email = email, 
                    Telefono = telefono };
                
                db.Cliente.Add(cliente);
                db.SaveChanges();

                // Mostrar un mensaje de éxito
                MessageBox.Show("Cliente creado exitosamente");
            }
    }
    }

    public class VentaItem
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public VentaItem(Producto producto, int cantidad)
        {
            Producto = producto;
            Cantidad = cantidad;
        }
    }
}
