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
using System.ComponentModel; // CancelEventArgs 

namespace SmallWorld
{
    /// <summary>
    /// Logique d'interaction pour ConfigJoueur.xaml
    /// </summary>
    public partial class ConfigJoueur : Window
    {
        private Peuple choixPeupleJ1;
        private Peuple choixPeupleJ2;

        private Window mwindow;
        public ConfigJoueur(Window mw)
        {
            InitializeComponent();
            this.mwindow = mw;
        }
        private void Valider_joueur(object sender, RoutedEventArgs e)
        {
            if (this.pseudoJ1.Text == "")
            {
                MessageBox.Show("Veuillez renseigner le pseudo du joueur 1");
            }
            else if (this.pseudoJ2.Text == "")
            {
                MessageBox.Show("Veuillez renseigner le pseudo du joueur 2");
            }
            else if (this.choixPeupleJ1 == null)
            {
                MessageBox.Show("Veuillez choisir un peuple pour le joueur 1");
            }
            else if (this.choixPeupleJ2 == null)
            {
                MessageBox.Show("Veuillez choisir un peuple pour le joueur 2");
            }
            else if (this.pseudoJ1.Text == this.pseudoJ2.Text)
            {
                MessageBox.Show("Veuillez choisir des pseudos différents");
            }
            else if (this.choixPeupleJ1.GetType() == this.choixPeupleJ2.GetType())
            {
                MessageBox.Show("Veuillez choisir des peuples différents");
            }
            else {
                ConfigMap Fenetre = new ConfigMap(this.mwindow, this.choixPeupleJ1, this.choixPeupleJ2, this.pseudoJ1.Text, this.pseudoJ2.Text);
                this.mwindow.Content = Fenetre.Content;
            }
            
        }

        private void ChoixNainJ1(object sender, RoutedEventArgs e)
        {
            this.OrcJ1.StrokeThickness = 0;
            this.ElfeJ1.StrokeThickness = 0;
            this.PirateJ1.StrokeThickness = 0;
            this.NainJ1.StrokeThickness = 2;
            this.NainJ1.Stroke = Brushes.White;
            this.choixPeupleJ1 = new PeupleNain();
        }
        private void ChoixOrcJ1(object sender, RoutedEventArgs e)
        {
            this.NainJ1.StrokeThickness = 0;
            this.ElfeJ1.StrokeThickness = 0;
            this.PirateJ1.StrokeThickness = 0;
            this.OrcJ1.StrokeThickness = 2;
            this.OrcJ1.Stroke = Brushes.White;
            this.choixPeupleJ1 = new PeupleOrc();
        }
        private void ChoixElfeJ1(object sender, RoutedEventArgs e)
        {
            this.NainJ1.StrokeThickness = 0;
            this.OrcJ1.StrokeThickness = 0;
            this.PirateJ1.StrokeThickness = 0;
            this.ElfeJ1.StrokeThickness = 2;
            this.ElfeJ1.Stroke = Brushes.White;
            this.choixPeupleJ1 = new PeupleElf();
        }
        private void ChoixPirateJ1(object sender, RoutedEventArgs e)
        {
            this.NainJ1.StrokeThickness = 0;
            this.OrcJ1.StrokeThickness = 0;
            this.ElfeJ1.StrokeThickness = 0;
            this.PirateJ1.StrokeThickness = 2;
            this.PirateJ1.Stroke = Brushes.White;
            this.choixPeupleJ1 = new PeuplePirate(); 
        }
        private void ChoixNainJ2(object sender, RoutedEventArgs e)
        {
            this.OrcJ2.StrokeThickness = 0;
            this.ElfeJ2.StrokeThickness = 0;
            this.PirateJ2.StrokeThickness = 0;
            this.NainJ2.StrokeThickness = 2;
            this.NainJ2.Stroke = Brushes.White;
            this.choixPeupleJ2 = new PeupleNain();
        }
        private void ChoixOrcJ2(object sender, RoutedEventArgs e)
        {
            this.NainJ2.StrokeThickness = 0;
            this.ElfeJ2.StrokeThickness = 0;
            this.PirateJ2.StrokeThickness = 0;
            this.OrcJ2.StrokeThickness = 2;
            this.OrcJ2.Stroke = Brushes.White;
            this.choixPeupleJ2 = new PeupleOrc();
        }
        private void ChoixElfeJ2(object sender, RoutedEventArgs e)
        {
            this.NainJ2.StrokeThickness = 0;
            this.OrcJ2.StrokeThickness = 0;
            this.PirateJ2.StrokeThickness = 0;
            this.ElfeJ2.StrokeThickness = 2;
            this.ElfeJ2.Stroke = Brushes.White;
            this.choixPeupleJ2 = new PeupleElf();
        }
        private void ChoixPirateJ2(object sender, RoutedEventArgs e)
        {
            this.NainJ2.StrokeThickness = 0;
            this.OrcJ2.StrokeThickness = 0;
            this.ElfeJ2.StrokeThickness = 0;
            this.PirateJ2.StrokeThickness = 2;
            this.PirateJ2.Stroke = Brushes.White;
            this.choixPeupleJ2 = new PeuplePirate();
        }
       

    }
}
