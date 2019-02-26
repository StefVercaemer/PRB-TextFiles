using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace TextFiles.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string bestandsPad;
        string bestandsInhoud;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        void LeesTeKiezenBestand()
        {
            bestandsPad = KiesBestand();
            bestandsInhoud = LeesBestand(bestandsPad);
            txtTekst.Text = bestandsInhoud;
        }

        string KiesBestand(string filter = "Text documents (.txt)|*.txt|Comma seperated values (.csv)|*.csv")
        {
            string gekozenBestandsPad;
            OpenFileDialog kiesBestand = new OpenFileDialog();
            //Enkel de bestanden met de doorgegeven extensie(s) worden getoond
            kiesBestand.Filter = filter;

            // Toon het dialoogvenster
            bool? result = kiesBestand.ShowDialog();
            //bool? betekent dat de boolean naast true en false ook de waarde null kan bevatten
            gekozenBestandsPad = kiesBestand.FileName;

            return gekozenBestandsPad;
        }

        string LeesBestand(string bestandsPad)
        {
            string inhoud = "";
            try
            {
                inhoud = File.ReadAllText(bestandsPad, Encoding.GetEncoding("iso-8859-1"));
            }
            catch (ArgumentException)
            {
                inhoud = "### Fout ###\n" + 
                         "Je hebt geen bestand geselecteerd";
            }

            catch (Exception ex)
            {
                inhoud = "### Fout ###\n" + ex.Message;
            }
            return inhoud;
        }

        string[] LeesBestandNaarArray(string bestandsPad)
        {
            string[] inhoud;
            try
            {
                inhoud = File.ReadAllLines(bestandsPad, Encoding.GetEncoding("iso-8859-1"));
            }
            catch (ArgumentException)
            {
                inhoud = new string[] {"### Fout ###\n" +
                          "Je hebt geen bestand geselecteerd"};
            }

            catch (Exception ex)
            {
                inhoud = new string[] { "### Fout ###\n" + ex.Message };
            }
            return inhoud;
        }

        private void BtnLeesTekst_Click(object sender, RoutedEventArgs e)
        {
            LeesTeKiezenBestand();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LeesTeKiezenBestand();
        }

        private void BtnLeesArray_Click(object sender, RoutedEventArgs e)
        {
            string[] inhoud;
            bestandsPad = KiesBestand();
            inhoud = LeesBestandNaarArray(bestandsPad);
            lstTekstArray.ItemsSource = inhoud;
        }
    }
}
