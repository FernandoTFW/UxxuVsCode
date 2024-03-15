using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uxxu
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UxxuEntities context = new UxxuEntities();
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = txtUsuario.Text;
            string contrasena = txtContrasena.Password;

            // Mostrar un indicador de "cargando"
            // ...

            bool autenticado = await AutenticarAsync(nombreUsuario, contrasena);

            // Ocultar el indicador de "cargando"
            // ...

            if (autenticado)
            {
                // Mostrar la ventana Menu
                
                Menu menu = new Menu();
                menu.Show();
                this.Close();
            }
            else
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
                lblError.Visibility = Visibility.Visible;
            }
        }
        public static async Task<bool> AutenticarAsync(string nombreUsuario, string contrasena)
        {
            using (var contexto = new UxxuEntities())
            {
                var usuario = await contexto.UsuariosVenta.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Contrasena == contrasena);
                if (usuario != null)
                {
                    Logued.User = usuario;
                }
                return usuario != null;
            }
        }
    }
}
