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
        string foutMelding;
        //string bestandsPad;

        public MainWindow()
        {
            InitializeComponent();

        }

        void ToonFoutmelding()
        {
            if (foutMelding == "" || foutMelding == null)
            {
                tbkErrors.Visibility = Visibility.Hidden;
            }
            else
            {
                tbkErrors.Visibility = Visibility.Visible;
            }
            tbkErrors.Text = foutMelding;
        }

        string LeesBestand(string bestandsPad)
        {
            string bestandsInhoud = "";
            
            foutMelding = "";

            try
            {
                // Er wordt een instance aangemaakt van de StreamReader-class
                using (StreamReader sr = new StreamReader(bestandsPad))
                {
                    bestandsInhoud = sr.ReadToEnd();
                }
                // na het using statement wordt de StreamReader gesloten en wordt het geheugen vrijgegeven.
            }
            catch (FileNotFoundException)
            {
                foutMelding = $"Het bestand {bestandsPad} is niet gevonden.";
            }
            catch (IOException)
            {
                foutMelding = $"Het bestand {bestandsPad} kan niet geopend worden.\nProbeer het te sluiten.";
            }
            catch (Exception e)
            {
                foutMelding = $"Er is een fout opgetreden. {e.Message}";
            }
            return bestandsInhoud;
        }

        bool SchrijfBestand(string tekst, string pad, string bestandsNaam)
        {
            bool isSuccesvolWeggeschreven = false;
            string bestandsPad;
            foutMelding = "";

            if (Directory.Exists(pad))
            {
                bestandsPad = pad + bestandsNaam;
                try
                {
                    // Er wordt een instance aangemaakt van de StreamWriter-class
                    using (StreamWriter sw = new StreamWriter(bestandsPad))
                    {
                        sw.Write(tekst);
                        sw.Close();
                    }
                    // na het using statement wordt de StreamWriter gesloten en wordt het geheugen vrijgegeven.
                    isSuccesvolWeggeschreven = true;
                }
                catch (IOException)
                {
                    foutMelding = $"Het bestand {bestandsPad} kan niet weggeschreven worden.\nProbeer het geopende bestand op die locatie te sluiten.";
                }
                catch (Exception e)
                {
                    foutMelding = $"Er is een fout opgetreden. {e.Message}";
                    throw;
                }
            }


            return isSuccesvolWeggeschreven;
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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            txtTekst.Text = LeesBestand(AppDomain.CurrentDomain.BaseDirectory + "dib.ini");
            txtBestandsnaam.Text = "db.ini";
            ToonFoutmelding();
        }

        private void BtnLeesArray_Click(object sender, RoutedEventArgs e)
        {
            string[] inhoud;
            string bestandsPad = KiesBestand();
            inhoud = LeesBestandNaarArray(bestandsPad);
            lstTekstArray.ItemsSource = inhoud;
        }

        private void BtnLeesBestand_Click(object sender, RoutedEventArgs e)
        {
            string bestandsNaam;
            bestandsNaam = txtBestandsnaam.Text;
            
            txtTekst.Text = LeesBestand(AppDomain.CurrentDomain.BaseDirectory + bestandsNaam);

            ToonFoutmelding();
        }

        private void BtnKiesBestand_Click(object sender, RoutedEventArgs e)
        {
            string bestandsPad = KiesBestand();
            string bestandsInhoud = LeesBestand(bestandsPad);
            txtTekst.Text = bestandsInhoud;
            ToonFoutmelding();
        }

        private void BtnSchrijfBestand_Click(object sender, RoutedEventArgs e)
        {
            string tekst = txtTekst.Text;
            string bestandsNaam = txtBestandsnaam.Text;
            string bestandsPad = AppDomain.CurrentDomain.BaseDirectory + bestandsNaam;
            SchrijfBestand(tekst, bestandsPad);
            ToonFoutmelding();
        }
    }
}
