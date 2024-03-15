using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string[] Labels { get; set; }
        public TextBox[] TextBoxes { get; set; }

        public InputDialog()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "Nuevo Cliente";
            Width = 400;
            Height = 700;

        }

        public void Init()
        {
            var grid = new Grid();
            grid.Width = 400;
            grid.Height = 420;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Crear un contenedor para los campos de entrada
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(10);

            // Agregar las etiquetas y los campos de texto
            for (int i = 0; i < Labels.Length; i++)
            {
                Label label = new Label();
                label.Content = Labels[i];
                stackPanel.Children.Add(label);

                TextBox textBox = new TextBox();
                TextBoxes[i] = textBox;
                stackPanel.Children.Add(textBox);
            }

            // Crear botones de aceptar y cancelar
            Button btnAceptar = new Button();
            btnAceptar.Content = "Aceptar";
            btnAceptar.Click += BtnAceptar_Click;

            Button btnCancelar = new Button();
            btnCancelar.Content = "Cancelar";
            btnCancelar.Click += BtnCancelar_Click;

            // Agregar botones al contenedor
            stackPanel.Children.Add(btnAceptar);
            stackPanel.Children.Add(btnCancelar);

            // Agregar el contenedor a la ventana
            Content = stackPanel;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
