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
using System.Drawing;
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
        private Rectangle[,] plateauUnite;
        private Dictionary<Border,Unite> uniteSelectionnee;
        private const int tailleCase = 96;
        private Tour tour;
        private int caseSelectX;
        private int caseSelectY;
        private Grid uniteGrid;
        private Canvas mapGrid;
        private double distanceCote;
        private Border borderSelect;
        
        public FenetreCarte(Jeu j)
        {
            InitializeComponent();
            this.jeu = j;
            int taille = this.jeu.getCarte().getTaille();
            this.plateau = new Rectangle[taille, taille];
            this.plateauUnite = new Rectangle[taille, taille];
            this.uniteSelectionnee = new Dictionary<Border, Unite>();
            this.tour = new TourImpl(this.jeu, this.jeu.getPremierJoueur());
            this.caseSelectX = -1;
            this.caseSelectY = -1;
            this.borderSelect = null;
       
        }

        private void creerCarte(object sender, RoutedEventArgs e)
        {
            this.mapGrid = new Canvas();
            int t = 0;
            int l = 0;
            for (int x = 0; x < this.jeu.getCarte().getTaille();  x++) {
                if((x % 2) != 0)
                    l = 50;
                else
                    l = 0;
                
                for (int y = 0; y < this.jeu.getCarte().getTaille(); y++) {
                    Case type = this.jeu.getCarte().getCase(x, y);
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = FabriqueImage.getInstance().getBrushCase(type);
                    Canvas.SetLeft(rectangle,l);
                    Canvas.SetTop(rectangle,t);
                    rectangle.Width = tailleCase;
                    rectangle.Height = tailleCase;
                    rectangle.StrokeThickness = 2;

                    rectangle.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftMapRectangle);
                    plateau[x, y] = rectangle;
                    this.mapGrid.Children.Add(rectangle);
                    l = l + 96;
                }
                t = t + 70;
            }
            this.distanceCote = (this.Width - ((this.jeu.getCarte().getTaille()+1)*96))/2;
            mapGrid.Margin = new Thickness(distanceCote, 0, distanceCote, 0);
            sp.Children.Add(mapGrid);
            ajoutUnite();
      
        }

        private void ajoutUnite()
        {
            int t = 0;
            int l = 0;
            for (int x = 0; x < this.plateauUnite.GetLength(0); x++) {
                for (int y = 0; y < this.plateauUnite.GetLength(1); y++){
                    this.mapGrid.Children.Remove(this.plateauUnite[x, y]);
                }
            }

            for (int x = 0; x < this.plateauUnite.GetLength(0); x++)
            {
                if ((x % 2) != 0)
                    l = 50;
                else
                    l = 0;

                for (int y = 0; y < this.plateauUnite.GetLength(1); y++)
                {
                    List<Unite> units = this.jeu.getCarte().getCase(x, y).getUnite();
                    int nb = units.Count();
                    
                    if (nb > 0) {
                        var rectangle = new Rectangle();
                        rectangle.Fill = FabriqueImage.getInstance().getBrushUnite(units[0], nb);
                        rectangle.Width = tailleCase;
                        rectangle.Height = tailleCase;
                        Canvas.SetLeft(rectangle, l);
                        Canvas.SetTop(rectangle, t);
                        rectangle.StrokeThickness = 2;
                        rectangle.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftUnite);
                        this.plateauUnite[x, y] = rectangle;
                        this.mapGrid.Children.Add(rectangle);
                    }
                    l = l + 96;
                }
                t = t + 70;
            }
        }

        private void effacementGrilleUnite()
        {
            for (int i = 0; i < this.uniteGrid.RowDefinitions.Count(); i++)
            {
                this.uniteGrid.RowDefinitions.RemoveAt(i);
                for (int j = 0; j < this.uniteGrid.ColumnDefinitions.Count(); j++)
                {
                    this.uniteGrid.ColumnDefinitions.RemoveAt(j);
                    this.uniteGrid.Children.RemoveAt(j);
                }
            }

        }

        //Quand on clique sur un hexagone qui contient des unités
        private void MouseLeftUnite(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            int posX = 0;
            int posY = 0;
            //On détermine les coordonnées de la case sélectionnée
            for (int x = 0; x < this.plateauUnite.GetLength(0); x++) {
                for (int y = 0; y < this.plateauUnite.GetLength(1); y++) {
                    if (this.plateauUnite[x, y] == rectangle) {
                        posX = x;
                        posY = y;
                    }
                }
            }

                this.caseSelectX = posX;
                this.caseSelectY = posY;

            nomAtrouver(sender,e);
        }

        //Quand on clique sur un hexagone qui ne contient pas d'unité
        private void MouseLeftMapRectangle(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            int posX = 0;
            int posY = 0;

            //On détermine les coordonnées de la case sélectionnée
            for (int x = 0; x < this.plateau.GetLength(0); x++) {
                for (int y = 0; y < this.plateau.GetLength(1); y++) {
                    if (this.plateau[x, y] == rectangle) {
                        posX = x;
                        posY = y;
                    }
                }
            }
           
                this.caseSelectX = posX;
                this.caseSelectY = posY;
            nomAtrouver(sender, e);
        }

        //Méthode quand on clique sur une case hexagonale
        private void nomAtrouver(object sender, MouseEventArgs e)
        {

            var rectangle = sender as Rectangle;

             if (this.tour.getUniteSelect() != null) {
                //Si une unité est sélectionnée ça veut dire qu'on sélectionne une destination
                this.tour.selectionnerDestination(this.caseSelectX, this.caseSelectY);
                System.Windows.MessageBox.Show("Destination OK " + this.caseSelectX + "-" + this.caseSelectY);
               
                 //On regarde si le déplacement est possible
                if (this.tour.deplacementPossible())
                {
                    //On déplace l'unité
                    this.tour.deplacementUnite();
                    System.Windows.MessageBox.Show("Deplacement ok");
                    //On met à jour la map
                    ajoutUnite();
                    //On met à jour la grille de la case sélectionnée
                    //effacementGrilleUnite();
                    this.uniteGrid = null;
                    this.uniteSelectionnee.Clear();
                    afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
                    //MouseLeftUnite(sender,e);
                }
                else
                {
                    System.Windows.MessageBox.Show("Cette unité ne peut être déplacée en " + this.caseSelectX + "-" + this.caseSelectY + ".");
                    //intégrer un système de message pour dire pourquoi?
                }
             }
             else
             {
                 //effacementGrilleUnite();
                 sp.Children.Remove(this.uniteGrid);
                 this.uniteGrid = null;
                 this.uniteSelectionnee.Clear();
                 afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
             }
        }


        //Quand on clique sur une cas qui contient des unités
        private void afficherUniteCase(List<Unite> lunite)
        {
            if (lunite.Count() > 0)
            {
                uniteGrid = new Grid();
                uniteGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Pixel) });
                for (int i = 0; i < lunite.Count(); i++)
                {
                    uniteGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Pixel) });
                    
                    Border border = new Border();
                    border.Background = FabriqueImage.getInstance().getBrushUnite(lunite[i],1);
                    TextBlock unitText = new TextBlock();
                    string vie = "Vie - " + lunite[i].getVie();
                    string attaque = "Attaque - " + lunite[i].getAttaque();
                    string defense = "Défense - " + lunite[i].getDefense();
                    string mouvement = "Mouvement - " + lunite[i].getMouvement();
                    unitText.Text = vie + "\n" + attaque + "\n" + defense + "\n" + mouvement;
                    unitText.FontSize = 10;
                    border.Child = unitText;
                    border.Width = 80;
                    border.Height = 80;
                    border.BorderThickness = new Thickness(2);
                    Grid.SetRow(border, 1);
                    Grid.SetColumn(border, i);
                    border.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftMapUnite);
                    uniteGrid.Children.Add(border);
                  
                    //On enregistre les unités sélectionnées
                    this.uniteSelectionnee.Add(border, lunite[i]);
                  
                }
                this.uniteGrid.Margin = new Thickness(this.distanceCote,(this.jeu.getCarte().getTaille()*70) + 20,this.distanceCote,0);
                this.sp.Children.Add(uniteGrid);
            }
          
        }

        private void effacerSelection()
        {
            if (this.borderSelect != null)
                this.borderSelect.BorderBrush = Brushes.White;
        }

        //Quand on clique sur une des unités affichée
        private void MouseLeftMapUnite(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            int posX = Grid.GetColumn(border);
            int posY = Grid.GetRow(border);

            if (this.borderSelect == border)
            {
                effacerSelection();
                this.tour.deselectionnerUnite();
            }
            else
            {
                effacerSelection();
                border.BorderBrush = Brushes.Red; //Couleur à changer
                this.tour.selectionnerUnite(uniteSelectionnee[border], this.caseSelectX, this.caseSelectY);
                this.borderSelect = border;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
