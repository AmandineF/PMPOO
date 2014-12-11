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

namespace SmallWorld
{
    /// <summary>
    /// Logique d'interaction pour ConfigJoueur.xaml
    /// </summary>
    public partial class ConfigJoueur : Window
    {
        public ConfigJoueur()
        {
            InitializeComponent();
        }
        private void Button_valider_joueur(object sender, RoutedEventArgs e)
        {
           ConfigMap Fenetre = new ConfigMap();
           this.Content = Fenetre.Content; 

        }
    }
}
