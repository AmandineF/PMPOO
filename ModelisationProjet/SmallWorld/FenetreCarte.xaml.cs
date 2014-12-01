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
    /// Logique d'interaction pour FenetreCarte.xaml
    /// </summary>
    public partial class FenetreCarte : Window
    {
        private Jeu jeu;
        private Rectangle[,] plateau;
        private const int tailleCase = 96;
        
        public FenetreCarte(Jeu j)
        {
            InitializeComponent();
            this.jeu = j;
            int taille = this.jeu.getCarte().getTaille();
            this.plateau = new Rectangle[taille, taille];
        }

        private void creerCarte(object sender, RoutedEventArgs e)
        {
            int t = 0;
            int l = 200;
            for (int x = 0; x < this.jeu.getCarte().getTaille();  x++) {
                if((x % 2) != 0)
                    l = 250;
                else
                    l = 200;
                
                for (int y = 0; y < this.jeu.getCarte().getTaille(); y++) {
                    Case type = this.jeu.getCarte().getCase(x, y);
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = FabriqueImage.getInstance().getBrushCase(type);
                    Canvas.SetLeft(rectangle,l);
                    Canvas.SetTop(rectangle,t);
                    rectangle.Width = tailleCase;
                    rectangle.Height = tailleCase;
                    rectangle.StrokeThickness = 2;
                    plateau[x, y] = rectangle;
                    this.mapGrid.Children.Add(rectangle);
                    l = l + 96;
                }
                t = t + 70;
            }
            ajoutUnite();
      
        }
        
        private void ajoutUnite() {
            int t = 0;
            int l = 200;
            for(int x=0; x<this.plateau.GetLength(0); x++) {
                if ((x % 2) != 0)
                    l = 250;
                else
                    l = 200;
                
                for(int y=0; y<this.plateau.GetLength(1); y++) {
                    List<Unite> units= this.jeu.getCarte().getCase(x,y).getUnite();
                    int nb = units.Count();
                    if (nb > 0) {
                        var rectangle = new Rectangle();
                        rectangle.Fill = FabriqueImage.getInstance().getBrushUnite(units[0], nb);
                        rectangle.Width = tailleCase;
                        rectangle.Height = tailleCase;
                        Canvas.SetLeft(rectangle, l);
                        Canvas.SetTop(rectangle, t);
                        rectangle.StrokeThickness = 2;
                        this.mapGrid.Children.Add(rectangle);
                        this.plateau[x, y] = rectangle;
                        l = l + 96;
                    }
                }
                t = t + 70;
            }
        }
     
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
