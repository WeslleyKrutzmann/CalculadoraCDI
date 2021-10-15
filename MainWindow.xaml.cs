using System;
using System.Collections.Generic;
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

namespace CalculadoraCDI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MESES_ANO = 12;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.CalcularValorRendimento();
        }

        /// <summary>
        /// Método para calcular o rendimento do CDI. Sem retorno, ele popula o resultado diretamente na propriedade "MainWindow.txbRendimentoTotal".
        /// </summary>
        private void CalcularValorRendimento()
        {
            try
            {
                var valorCDI = this.NormalizarValor(this.txtValorCDI.Text, nameof(this.txtValorCDI));
                var rendimentoCDI = this.NormalizarValor(this.txtRendimentoCDI.Text, nameof(this.txtRendimentoCDI));
                var mesesRendimento = (int)this.NormalizarValor(this.txtMesesRendimento.Text, nameof(this.txtMesesRendimento));
                var valorDepositado = this.NormalizarValor(this.txtValorDepositado.Text, nameof(this.txtValorDepositado));

                var totalCDI = (valorCDI * rendimentoCDI) / 100;
                var valorCDIPorMes = totalCDI / MESES_ANO;
                var valorTotalRendimento = valorDepositado;

                for (var i = 0; i < mesesRendimento; i++)
                {
                    valorTotalRendimento += (valorTotalRendimento * valorCDIPorMes) / 100;
                }

                this.txbRendimentoTotal.Text = String.Format("Total (R$): {0:0.##}", valorTotalRendimento);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Método para normalização de valores. Recebe um object e retorna o objeto com o tipo informado na chamada do método <T>.
        /// </summary>
        private T NormalizarValor<T>(object valor, string nomePropriedade)
        {
            if (valor != null)
            {
                if (this.IsValorValido(valor.ToString()))
                {
                    return (T)valor;
                }
            }

            throw new ArgumentNullException($"O valor da propriedade {nomePropriedade} não pode ser vazio ou nulo.");
        }

        /// <summary>
        /// Método para normalização de valores. Recebe uma string e retorna um decimal.
        /// </summary>
        private decimal NormalizarValor(string strValor, string nomePropriedade)
        {
            if (!String.IsNullOrEmpty(strValor))
            {
                if (this.IsValorValido(strValor))
                {
                    if (decimal.TryParse(strValor, out var valor))
                    {
                        return valor;
                    }
                }
            }

            throw new ArgumentNullException($"O valor da propriedade {nomePropriedade} não pode ser vazio ou nulo.");
        }

        /// <summary>
        /// Método para verificar se o valor informado é válido por meio da utilização de Regex para sequencias numéricas inteiras e decimais.
        /// </summary>
        private bool IsValorValido(string strValor)
        {
            var regexDecimal = new Regex(@"^[0-9\-.\-,]?$");
            
            if (regexDecimal.IsMatch(strValor))
            {
                return true;
            }

            var regexInt = new Regex(@"^[0-9]");
            return regexInt.IsMatch(strValor);
        }
    }
}
