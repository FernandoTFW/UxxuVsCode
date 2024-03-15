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
            Height = 250;

        }

        public void Init()
        {
            var grid = new Grid();
            grid.Width = 400;
            grid.Height = 420;
            Content = grid;

            for (int i = 0; i < Labels.Length; i++)
            {
                var label = new Label();
                label.Content = Labels[i];
                label.Margin = new Thickness(5);
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);

                var textBox = new TextBox();
                textBox.Margin = new Thickness(5);
                Grid.SetRow(textBox, i);
                Grid.SetColumn(textBox, 1);

                grid.Children.Add(label);
                grid.Children.Add(textBox);
            }

            var buttonOk = new Button();
            buttonOk.Content = "Aceptar";
            buttonOk.Margin = new Thickness(5);
            buttonOk.Click += ButtonOk_Click;
            Grid.SetRow(buttonOk, Labels.Length);
            Grid.SetColumn(buttonOk, 1);

            var buttonCancel = new Button();
            buttonCancel.Content = "Cancelar";
            buttonCancel.Margin = new Thickness(5);
            buttonCancel.Click += ButtonCancel_Click;
            Grid.SetRow(buttonCancel, Labels.Length);
            Grid.SetColumn(buttonCancel, 2);

            grid.Children.Add(buttonOk);
            grid.Children.Add(buttonCancel);
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
