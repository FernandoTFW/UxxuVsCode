using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para ProveedorPage.xaml
    /// </summary>
    public partial class ProveedorPage : Window
    {
        UxxuEntities db = new UxxuEntities();
        List<Proveedor> proveedores;
        Proveedor proveedorAct;
        bool editing = false;
        public ProveedorPage()
        {
            InitializeComponent();
            Actualizar();
            
            // Cargar proveedores de la base de datos...
        }
        public async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            
            if (!editing)
            {
                // Validar datos...
                string nombreProveedor = txtNombreProveedor.Text;
                string direccion = txtDireccion.Text;
                string telefono = txtTelefono.Text;
                string correoElectronico = txtCorreoElectronico.Text;
                Proveedor proveedor = new Proveedor()
                {
                    NombreProveedor = nombreProveedor,
                    Direccion = direccion,
                    Telefono = telefono,
                    CorreoElectronico = correoElectronico
                };

                // Mostrar un indicador de "cargando"...

                using (var db = new UxxuEntities())
                {
                    db.Proveedor.Add(proveedor);
                    await db.SaveChangesAsync();
                    Actualizar();
                }
                
            }
            else
            {
                using (db)
                {
                    var proveedorToUpdate = db.Proveedor.Find(proveedorAct.IdProveedor);
                    proveedorToUpdate.NombreProveedor = txtNombreProveedor.Text;
                    proveedorToUpdate.Direccion = txtDireccion.Text;
                    proveedorToUpdate.Telefono = txtTelefono.Text;
                    proveedorToUpdate.CorreoElectronico = txtCorreoElectronico.Text;
                    await db.SaveChangesAsync();
                    Actualizar();
                }
                
                txtNombreProveedor.Text = "";
                txtCorreoElectronico.Text = "";
                txtDireccion.Text = "";
                txtTelefono.Text = "";
                editing = false;
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            proveedorAct = dataGridProveedores.SelectedItem as Proveedor;
            txtNombreProveedor.Text = proveedorAct.NombreProveedor;
            txtCorreoElectronico.Text = proveedorAct.CorreoElectronico;
            txtDireccion.Text = proveedorAct.Direccion;
            txtTelefono.Text = proveedorAct.Telefono;
            editing = true;

        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            Proveedor proveedor = dataGridProveedores.SelectedItem as Proveedor;

            using (var db = new UxxuEntities())
            {
                var proveedorToRemove = db.Proveedor.Find(proveedor.IdProveedor);
                db.Proveedor.Remove(proveedorToRemove);
                await db.SaveChangesAsync();
                Actualizar();
            }
            
        }

        private void Actualizar()
        {
            proveedores = db.Proveedor.ToList();
            dataGridProveedores.ItemsSource = proveedores;
        }


    }
}
