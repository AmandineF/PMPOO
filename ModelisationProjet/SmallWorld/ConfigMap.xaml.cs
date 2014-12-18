﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
    /// Logique d'interaction pour ConfigMap.xaml
    /// </summary>
    public partial class ConfigMap : Window
    {
        private Jeu jeu;
        private Carte carte;
        private Peuple peuple1;
        private Peuple peuple2;
        private string pseudo1;
        private string pseudo2;
        private Window mwindow;
        public ConfigMap(Window mw, Peuple p1, Peuple p2, string ps1, string ps2)
        {
            InitializeComponent();
            this.peuple1 = p1;
            this.peuple2 = p2;
            this.pseudo1 = ps1;
            this.pseudo2 = ps2;
            this.mwindow = mw;
        }

        private void ChoixDemoCarte(object sender, RoutedEventArgs e)
        {
           // this.petite.BorderThickness = new Thickness(0);
            //this.normale.BorderThickness = new Thickness(0);
            //this.demo.BorderThickness = new Thickness(2);
            //this.demo.BorderBrush = Brushes.White;
            this.demo.Header = "Choisie !";
            this.petite.Header = "";
            this.normale.Header = "";
            this.demo.Background = Brushes.Blue;
            this.petite.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 43, 79));
            this.normale.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 43, 79));
            //this.dieu = new CreateurPartie(6);
            this.carte = new CarteDemo();
        }

        private void ChoixPetiteCarte(object sender, RoutedEventArgs e)
        {
            //this.petite.BorderThickness = new Thickness(2);
            //this.normale.BorderThickness = new Thickness(0);
            //this.demo.BorderThickness = new Thickness(0);
            //this.petite.BorderBrush = Brushes.White;
            this.petite.Header = "Choisie !";
            this.demo.Header = "";
            this.normale.Header = "";
            this.petite.Background = Brushes.Blue;
            this.demo.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 43, 79));
            this.normale.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 43, 79));
            //this.dieu = new CreateurPartie(10);
            this.carte = new CartePetite();
        }

        private void ChoixNormaleCarte(object sender, RoutedEventArgs e)
        {
           // this.petite.BorderThickness = new Thickness(0);
            //this.normale.BorderThickness = new Thickness(2);
            //this.demo.BorderThickness = new Thickness(0);
            //this.normale.BorderBrush = Brushes.White;
            this.normale.Header = "Choisie !";
            this.petite.Header = "";
            this.demo.Header = "";
            this.normale.Background = Brushes.Blue;
            this.petite.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 43, 79));
            this.demo.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(39, 43, 79));
            //this.dieu = new CreateurPartie(14);
            this.carte = new CarteNormale();
        }

        private void ButtonCloseClicked(object sender, RoutedEventArgs e)
        {
            if (this.carte == null)
            {
                MessageBox.Show("Merci de choisir une carte");
            }
            else
            {
                CreateurPartie dieu = new CreateurPartie(this.carte);
                this.jeu = dieu.creerPartie(this.pseudo1, this.peuple1, this.pseudo2, this.peuple2);    
                FenetreCarte frm = new FenetreCarte(this.jeu);
                this.mwindow.Close();
                frm.Show();
            }
           
        }
    
    }
}
