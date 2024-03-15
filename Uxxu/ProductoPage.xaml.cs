using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
    /// Lógica de interacción para ProductoPage.xaml
    /// </summary>
    public partial class ProductoPage : Window
    {
        private UxxuEntities db = new UxxuEntities();
        List<Producto> products;
        List<Proveedor> proveedores;
        Producto productoAct;
        bool editing = false;
        string urlImagen = "";
        public ProductoPage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CargarProveedores();
            CargarProductos();
            cmbProveedores.SelectedIndex = 0;
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                //string imageUrl = ConvertToUrl(imagePath);
                urlImagen = imagePath;
                imgProducto.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath));
            }
        }

        private string ConvertToUrl(string imagePath)
        {
            // Suponiendo que la imagen se encuentra en el mismo directorio que la aplicación,
            // puedes obtener la URL relativa de esta manera
            string currentDirectory = Directory.GetCurrentDirectory();
            string relativePath = imagePath.Substring(currentDirectory.Length);

            // Reemplaza las barras invertidas con barras inclinadas para obtener una URL válida
            return relativePath.Replace('\\', '/');
        }

        private void CargarProveedores()
        {
            var proveedoresDb = db.Proveedor.ToList();
            proveedores = proveedoresDb;
            cmbProveedores.ItemsSource = proveedores;
        }

        private void CargarProductos()
        {
            var productosDb = db.Producto.ToList();
            products = productosDb;
            dataGridProductos.ItemsSource = products;
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!editing)
            {
                string nombreProducto = txtNombreProducto.Text;
                decimal precioProducto = decimal.Parse(txtPrecioProducto.Text);
                int stockProducto = int.Parse(txtStockProducto.Text);
                string urlProducto = urlImagen;
                int proveedorId = (int)cmbProveedores.SelectedValue;

                // Validar datos...

                Producto producto = new Producto()
                {
                    NombreProducto = nombreProducto,
                    Precio = precioProducto,
                    Stock = stockProducto,
                    UrlProducto = urlProducto,
                    IdProveedor = proveedorId
                };

                using (var db = new UxxuEntities())
                {
                    db.Producto.Add(producto);
                    await db.SaveChangesAsync();
                    CargarProductos();
                }
            }
            else
            {
                Producto producto = dataGridProductos.SelectedItem as Producto;
                using (db)
                {
                    var productoToUpdate = db.Producto.Find(producto.IdProducto);
                    productoToUpdate.NombreProducto = txtNombreProducto.Text;
                    productoToUpdate.Precio = decimal.Parse(txtPrecioProducto.Text);
                    productoToUpdate.Stock = int.Parse(txtStockProducto.Text);
                    productoToUpdate.UrlProducto = urlImagen;
                    productoToUpdate.IdProveedor = (int)cmbProveedores.SelectedValue;

                    await db.SaveChangesAsync();
                    CargarProductos();
                }
                editing = false;
            }
            txtNombreProducto.Text = "";
            txtPrecioProducto.Text = "";
            txtStockProducto.Text = "";
            imgProducto.Source = null;



        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Producto producto = dataGridProductos.SelectedItem as Producto;

            // Cargar datos del producto en el formulario...
            editing = true;
            txtNombreProducto.Text = producto.NombreProducto;
            txtPrecioProducto.Text = producto.Precio.ToString();
            txtStockProducto.Text = producto.Stock.ToString();
            // Crea una nueva instancia de BitmapImage
            BitmapImage bitmapImage = new BitmapImage();

            // Asigna la URL a la propiedad UriSource del BitmapImage
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(producto.UrlProducto);
            bitmapImage.EndInit();

            // Asigna el BitmapImage al Source del control Image
            imgProducto.Source = bitmapImage;
            cmbProveedores.SelectedValue = producto.IdProveedor;

            // Actualizar la lista de productos...

        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            Producto producto = dataGridProductos.SelectedItem as Producto;

            using (db)
            {
                var productoToRemove = db.Producto.Find(producto.IdProducto);
                db.Producto.Remove(productoToRemove);
                await db.SaveChangesAsync();
                CargarProductos();
            }

            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
