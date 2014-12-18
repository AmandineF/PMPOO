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
using ModelisationProjet;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

namespace SmallWorld
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();  
            
        }

        private void BoutonNouvellePartie(object sender, RoutedEventArgs e)
        {
            ConfigJoueur Fenetre = new ConfigJoueur(this);
            this.Content = Fenetre.Content;
        }
        private void BoutonChargerPartie(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".sw";
            dialog.Filter = "SmallWorld (*.sw) | *.sw | All files (*.*) | *.*";
            dialog.RestoreDirectory = true;
            Nullable<bool> res = dialog.ShowDialog();
            if (res == true)
            {
                this.chargerPartie(dialog.FileName);
            }
        }
        private void BoutonQuitter(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        public void chargerPartie(string nomDuFichier)
        {
            Stream stream = File.Open(nomDuFichier, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            Jeu partieSauvee = (Jeu)formatter.Deserialize(stream);
            stream.Close();
            new FenetreCarte(partieSauvee).Show();
            this.Close();
        }
    }
}

