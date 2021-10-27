using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Calculadora {

    public partial class MainWindow : Window {
        private String currentOperation = "";
        private ObservableCollection<String> list;
        public MainWindow() {
            InitializeComponent();
            list = new ObservableCollection<String>();

        }
        public void AddResultado(String resultado) {
            if(list.Count == 5) list.Remove(list.First());
            list.Add(resultado);
            listViewResultados.ItemsSource = list;
        }
        public void ButtonNumberPressed(object sender, EventArgs e) {
            Button boton = (Button)sender;
            currentOperation += boton.Content.ToString();
            labelResult.Content = currentOperation;
        }
        public void ButtonActionPressed(object sender, EventArgs e) {
            Button boton = (Button)sender;
            string valorBoton = boton.Content.ToString();
            bool expresionTerminaOperacion = Regex.Match(currentOperation.Last().ToString(), "[/*+-]" ).Success;
            if(currentOperation != "" && expresionTerminaOperacion)
                        currentOperation = currentOperation[0..^1];
                
            currentOperation += valorBoton;
            labelResult.Content = currentOperation;
        }
        public void ButtonDelete(object sender, EventArgs e) {
            currentOperation = "";
            labelResult.Content = "0.00";
        }
        public void ButtonCommaPressed(object sender, EventArgs e) {

            Button boton = (Button)sender;
            string valorBoton = boton.Content.ToString();
            bool expresionContieneDecimal = Regex.Match(currentOperation, "[.]").Success;
            bool expresionAcabaEnNumero = Regex.Match(currentOperation, "\\d{1}$").Success;

            if(expresionContieneDecimal)
                return;

            if(!expresionAcabaEnNumero)
                currentOperation += "0";

            currentOperation += valorBoton;

            labelResult.Content = currentOperation;
        }
        public void ButtonResult(object sender, EventArgs e) {
            string[] actions = { "+", "-", "*", "/" };
            Button boton = (Button)sender;
            bool expresionAcabaEnOperacion = Regex.Match(currentOperation.Last().ToString(), "[+-/*]$").Success;
            if(currentOperation != "" && expresionAcabaEnOperacion)
                currentOperation = currentOperation.Substring(0, currentOperation.Length - 1);

            labelLastResult.Content = currentOperation;
            labelResult.Content = GetResult();
            AddResultado(currentOperation);

        }
        public void ButtonErase(object sender, EventArgs e) {
            if(currentOperation.Length < 2)
            {
                currentOperation = "";
                labelResult.Content = "0.00";
            } else
            {
                currentOperation = currentOperation.Substring(0, currentOperation.Length - 1);
                labelResult.Content = currentOperation;
            }
        }


        public void CopyButton(object sender, EventArgs e) {
            Clipboard.SetText(currentOperation);
            MessageBox.Show("Resultado " + currentOperation + " copiado correctamente", "Resultado copiado", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public string GetResult() {
            DataTable dataTable = new DataTable();
            var result = dataTable.Compute(currentOperation, "");
            currentOperation = result.ToString();
            return currentOperation;
        }

    }
}


