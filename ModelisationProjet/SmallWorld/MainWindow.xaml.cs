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

namespace SmallWorld
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Jeu jeu;
        public MainWindow()
        {
            InitializeComponent();
            CreateurPartie dieu = new CreateurPartie(4);
            this.jeu = dieu.creerPartie("Amandine", new PeupleNain(), "Frank", new PeupleOrc());
            this.jeu.getCarte().Dessin();
        }

        private void ButtonCloseClicked(object sender, RoutedEventArgs e)
        {
            FenetreCarte frm = new FenetreCarte(this.jeu);
            frm.Show();
            this.Close();
        }

    }
}

