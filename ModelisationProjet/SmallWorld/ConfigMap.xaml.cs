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
using ModelisationProjet;

namespace SmallWorld
{
    /// <summary>
    /// Logique d'interaction pour ConfigMap.xaml
    /// </summary>
    public partial class ConfigMap : Window
    {
        private Jeu jeu;
        public ConfigMap()
        {
            InitializeComponent();
            CreateurPartie dieu = new CreateurPartie(4);
            this.jeu = dieu.creerPartie("Amandine", new PeupleNain(), "Frank", new PeupleOrc());
            this.jeu.getCarte().Dessin();
        }
        private void ButtonCloseClicked(object sender, RoutedEventArgs e)
        {
            FenetreCarte frm = new FenetreCarte(this.jeu);
            this.Content = frm.Content;
        }
    }
}
