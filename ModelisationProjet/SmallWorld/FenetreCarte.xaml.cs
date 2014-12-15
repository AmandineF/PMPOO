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
using System.ComponentModel; //CancelEventsArg
using Wrapper;

namespace SmallWorld
{

    public partial class FenetreCarte : Window
    {
        private Jeu jeu;
        private Tour tour;

        private Rectangle[,] plateau;
        private Border[,] plateauUnite;
        private Unite[,] uniteSelect;
        private Grid[,] gridsSelect;
        private const int tailleCase = 96;
        private int caseSelectX;
        private int caseSelectY;
        private Grid uniteGrid;
        private Canvas mapGrid;
        private Border borderSelect;
        private Joueur joueur;
        private int nbTourInit;
        private Rectangle caseSelect;
        private Grid gridSelect;
        private Boolean deuxJoueursTour;

        public FenetreCarte(Jeu j, Joueur p)
        {
            InitializeComponent();
            this.gridSelect = new Grid();
            this.jeu = j;
            this.plateau = new Rectangle[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.plateauUnite = new Border[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.uniteSelect = new Unite[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.gridsSelect = new Grid[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.joueur = p;
            this.tour = new TourImpl(this.jeu, this.joueur);
            this.caseSelectX = -1;
            this.caseSelectY = -1;
            this.borderSelect = null;
            this.nbTourInit = 1;
            this.Menu.Text = "Tour n°" + this.nbTourInit + " - " + this.joueur.getPseudo();
            this.deuxJoueursTour = false;
            informationsJoueur();
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
                    l = l + 100;
                }
                t = t + 75;
            }

            mapGrid.Margin = new Thickness(0, 80, 0, 0);
            this.FinTour.Margin = new Thickness(0, (this.jeu.getCarte().getTaille() * 80) + 30, 0, 0);
            sp.Children.Add(mapGrid);
            ajoutUnite();
        }


        private void informationsJoueur()
        {
            string uniteRestantes = "";
            string pointVictoire = "";
            string peuple = "";
            this.pseudoJ1.Text = this.jeu.getJoueur1().getPseudo();
            peuple = "Peuple " + this.jeu.getJoueur1().getNomPeuple() + "\n";
            uniteRestantes = this.jeu.getJoueur1().getNbUnite() + " unités restantes\n";
            pointVictoire = this.jeu.getJoueur1().getPtVictoire() + " points.\n";
            this.infoJ1.Text = peuple + uniteRestantes + pointVictoire;

            this.pseudoJ2.Text = this.jeu.getJoueur2().getPseudo();
            peuple = "Peuple " + this.jeu.getJoueur2().getNomPeuple() + "\n";
            uniteRestantes = this.jeu.getJoueur2().getNbUnite() + " unités restantes\n";
            pointVictoire = this.jeu.getJoueur2().getPtVictoire() + " points.\n";
            this.infoJ2.Text = peuple + uniteRestantes + pointVictoire;
        }

        private void affichageCombat(string res)
        {
            deroulement.Text = res;
        }

        private void ajoutUnite()
        {
            int t = 3;
            int l = 10;
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
                    
                    if (units.Count() > 0) {

                        Border border = new Border();
                        border.Background = FabriqueImage.getInstance().getBrushUnite(units[0]);
                        TextBlock unitText = new TextBlock();
                            border.Width = 76;
                            border.Height = 76;

                        unitText.Margin = new Thickness(50, 30, 0, 0);
                        unitText.Text = "" + units.Count();
                        unitText.FontSize = 40;
                        unitText.Foreground = Brushes.Black;
                        unitText.FontFamily = new FontFamily("Showcard Gothic");
                        border.Child = unitText;
                        Canvas.SetLeft(border, l);
                        Canvas.SetTop(border, t);
                        border.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftUnite);
                        this.plateauUnite[x, y] = border;
                        this.mapGrid.Children.Add(border);
                    }
                    l = l + 100;
                }
                t = t + 75;
            }
        }

        private void effacementGrilleUnite()
        {
            if (this.uniteGrid != null)
            {
                for (int x = 0; x < this.uniteSelect.GetLength(0); x++)
                {
                    for (int y = 0; y < this.uniteSelect.GetLength(1); y++)
                    {
                        this.uniteGrid.Children.Remove(this.gridsSelect[x,y]);
                    }
                }
                this.gridsSelect = new Grid[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            }
        }


        //Quand on clique sur un hexagone qui contient des unités
        private void MouseLeftUnite(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Border;
            int posX = 0;
            int posY = 0;
            int coordX = 0;
            int coordY = 0;

            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);

            //On détermine les coordonnées de la case sélectionnée
            for (int x = 0; x < this.plateauUnite.GetLength(0); x++) {
                for (int y = 0; y < this.plateauUnite.GetLength(1); y++) {
                    if (this.plateauUnite[x, y] == rectangle) {
                        posX = x;
                        posY = y;
                        coordX = 75 * posX;
                        coordY = 100 * posY;
                        if ((x % 2) != 0)
                            coordY += 50;

                        var select = new Rectangle();
                        select.Fill = FabriqueImage.getInstance().getSelection(this.joueur.getPeuple());
                        select.Width = tailleCase;
                        select.Height = tailleCase;
                        Canvas.SetLeft(select, coordY);
                        Canvas.SetTop(select, coordX); 
                        this.mapGrid.Children.Add(select);   
                        this.caseSelect = select;
                    }
                }
            }

            this.caseSelectX = posX;
            this.caseSelectY = posY;
            nomAtrouver(sender,e);
        }

        //Quand on clique sur un hexagone qui ne contient pas d'unité
        private void MouseLeftMapRectangle(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            int posX = 0;
            int posY = 0;
            int coordX = 0;
            int coordY = 0;

            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);

            //On détermine les coordonnées de la case sélectionnée
            for (int x = 0; x < this.plateau.GetLength(0); x++) {
                for (int y = 0; y < this.plateau.GetLength(1); y++) {
                    if (this.plateau[x, y] == rectangle) {
                        posX = x;
                        posY = y;
                        coordX = 75 * posX;
                        coordY = 100 * posY;
                        if ((x % 2) != 0)
                            coordY += 50;

                        var select = new Rectangle();
                        select.Fill = FabriqueImage.getInstance().getSelection(this.joueur.getPeuple());
                        select.Width = tailleCase;
                        select.Height = tailleCase;
                        Canvas.SetLeft(select, coordY);
                        Canvas.SetTop(select, coordX);
                        this.mapGrid.Children.Add(select);
                        this.caseSelect = select;
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
            
                //On regarde si le déplacement est possible
                if (this.tour.deplacementPossible())
                {
                    //On déplace l'unité
                    string affichage = this.tour.deplacementUnite();
                    affichageCombat(affichage);
                    informationsJoueur();

                    //On met à jour la map
                    ajoutUnite();
                    effacerSelection();

                    //On met à jour la grille de la case sélectionnée
                    afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
                }
                else
                {
                    System.Windows.MessageBox.Show("Cette unité ne peut être déplacée en " + this.caseSelectX + "-" + this.caseSelectY + ".");
                    //A voir si on fait autrement : soit on deselectionne automatique soit on laisse l'utilisateur la retoucher pour deselectionner
                    effacerSelection();
                    this.tour.deselectionnerUnite();
                    afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
                    //intégrer un système de message pour dire pourquoi?
                }
             }
             else
             {
                 effacerSelection();
                 afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
             }
        }


        //Quand on clique sur une case qui contient des unités
        private void afficherUniteCase(List<Unite> lunite) {
            string vie = "";
            string attaque = "";
            string defense = "";
            string mouvement = "";
            int i = 0;
            int j = 0;
            int k = 0;
            int cpt = 0;
            effacementGrilleUnite();
            if (lunite.Count() > 0)
            {
                uniteGrid = new Grid();
                for (i = 0; i < 3; i++)
                {
                    uniteGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                    uniteGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
                }
                Grid.SetRow(uniteGrid, 0);
                Grid.SetColumn(uniteGrid, 1);
                this.barreInfo.Children.Add(uniteGrid);  

                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        if(cpt < lunite.Count()) {
                            //Mise en place de la grille interne
                            Grid grille = new Grid();

                            //grille.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
                            for (k = 0; k < 2; k++)
                            {
                                grille.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                            }
                            Grid.SetRow(grille, i);
                            Grid.SetColumn(grille, j);
                            grille.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftMapUnite);
                            uniteGrid.Children.Add(grille);                        
                        
                            //Ajout de l'image de l'unité à la grille interne
                            Rectangle image = new Rectangle();
                            image.Fill = FabriqueImage.getInstance().getBrushUnite(lunite[cpt]);
                            image.Width = 96;
                            image.Height = 96;
                            Grid.SetRow(image, 0);
                            Grid.SetColumn(image, 0);
                            grille.Children.Add(image);

                            //Ajout des caractéristiques de l'unité à la grille interne
                            TextBlock texte = new TextBlock();
                            texte.TextAlignment = TextAlignment.Center;
                            vie = "Vie - " + lunite[cpt].getVie();
                            attaque = "Attaque - " + lunite[cpt].getAttaque();
                            defense = "Defense - " + lunite[cpt].getDefense();
                            mouvement = "Mouvement - " + lunite[cpt].getMouvement();
                            texte.Text = vie + "\n" + attaque + "\n" + defense + "\n" + mouvement;
                            Grid.SetRow(texte, 1);
                            Grid.SetColumn(texte, 0);
                            grille.Children.Add(texte);
                            this.gridsSelect[i, j] = grille;
                            this.uniteSelect[i, j] = lunite[cpt];
                            cpt++;
                        }
                    }
                }
            }
        }

        private void effacerSelection()
        {
            this.tour.deselectionnerUnite();
            if (this.borderSelect != null)
                this.borderSelect.BorderBrush =  new SolidColorBrush(Color.FromArgb(255, 244, 234, 229));

        }



        //Quand on clique sur une des unités de la barre d'information
        unsafe private void MouseLeftMapUnite(object sender, MouseButtonEventArgs e)
        {
            effacerSelection();
            //Si le joueur essaye de sélectionner des unités qui ne sont pas à lui
            if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).estCaseEnnemie(this.joueur))
            {
                MessageBox.Show("Impossible de sélectionner une case ennemie.");
            }
            else
            {
                Grid grille = sender as Grid;
                int ligne = Grid.GetRow(grille);
                int colonne = Grid.GetColumn(grille);

                if (grille != this.gridSelect)
                {
                    //Mise en place de la bordure
                    Border bordure = new Border();
                    bordure.BorderBrush = Brushes.Black;
                    bordure.BorderThickness = new Thickness(1);
                    Grid.SetRow(bordure, ligne);
                    Grid.SetColumn(bordure, colonne);
                    uniteGrid.Children.Add(bordure);

                    //On enregistre l'unité sélectionnée
                    this.tour.selectionnerUnite(this.uniteSelect[ligne,colonne], this.caseSelectX, this.caseSelectY);

                    //On "entoure" les cases où l'unité peut se déplacer
                    int i,j;
                    int tailleMap = this.jeu.getCarte().getTaille();
                    bool** carteBool = this.tour.recupererCarteSuggestion();
                    for (i = 0; i < tailleMap; i++){
                        for (j = 0; j < tailleMap; j++){
                            if (carteBool[i][j] == true)
                            {
                                Console.Write("Case :" + i +" ; " + j + "------------");
                                var suggere = new Rectangle();
                                suggere.Fill = FabriqueImage.getInstance().getSelection(this.joueur.getPeuple());
                                suggere.Width = tailleCase;
                                suggere.Height = tailleCase;
                                int x = 75 * i;
                                int y = 100 * j;
                                if ((i % 2) != 0)
                                    y += 50;
                                Canvas.SetLeft(suggere, y);
                                Canvas.SetTop(suggere, x);
                                this.mapGrid.Children.Add(suggere);
                            }
                        }
                    }

                    //On enregistre la grille et la bordure sélectionnée 
                    this.borderSelect = bordure;
                    this.gridSelect = grille;
                }
                else
                {
                    this.gridSelect = null;
                }
            }
        
        }


        private void FinTour_Click(object sender, RoutedEventArgs e)
        {
             
            if (this.jeu.getNbTours() > 1 && (this.jeu.finDuJeu() == false))
            {
                if (this.deuxJoueursTour)
                {
                    this.jeu.getJoueur1().setPtVictoire(this.jeu.getJoueur1().calculerPoints() + this.jeu.getCarte().nbCasesColonisees(this.jeu.getJoueur1()));
                    this.jeu.getJoueur2().setPtVictoire(this.jeu.getJoueur2().calculerPoints() + this.jeu.getCarte().nbCasesColonisees(this.jeu.getJoueur2()));
                    this.jeu.decNbTours();
                    this.jeu.reinitialisation();
                    this.nbTourInit++;
                    this.deuxJoueursTour = false;
                }
                else
                {
                    this.deuxJoueursTour = true;
                }

                if (this.joueur == this.jeu.getJoueur1())
                {
                    this.joueur = this.jeu.getJoueur2();
                }
                else
                {
                    this.joueur = this.jeu.getJoueur1();
                }
                this.tour = new TourImpl(this.jeu, this.joueur);
                this.Menu.Text = "Tour n°" + this.nbTourInit + " - " + this.joueur.getPseudo();
                informationsJoueur();
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
                
            }
            else
            {
                if (this.jeu.getGagnant() == this.jeu.getJoueur1())
                {
                    MessageBox.Show("Fin de la partie - Le gagnant est " + this.jeu.getJoueur1().getPseudo() + ".");
                }
                else if (this.jeu.getGagnant() == this.jeu.getJoueur2())
                {
                    MessageBox.Show("Fin de la partie - Le gagnant est " + this.jeu.getJoueur2().getPseudo() + ".");
                }
                else
                {
                    MessageBox.Show("Fin de la partie - Égalité.");
                }
            }
        }
        bool isDataSaved = false;
        public void FenetreCarte_Closing(object sender, CancelEventArgs e)
        {
            if (!isDataSaved)
            {
                MessageBoxResult res1 = MessageBox.Show("Voulez-vous sauvegarder la partie avant de quitter ?", "Sauver ?", MessageBoxButton.YesNo);
                if (res1 == MessageBoxResult.Yes)
                {
                    //sauvegarder();
                    Application.Current.Shutdown();
                }
                else
                {
                    MessageBoxResult res2 = MessageBox.Show("Etes-vous sûr de vouloir quitter le jeu quand même ?", "Quitter ?", MessageBoxButton.YesNo);
                    if (res2 == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
            else
            {
                MessageBoxResult res3 = MessageBox.Show("Etes-vous sûr de vouloir quitter le jeu ?", "Quitter ?", MessageBoxButton.YesNo);
                if (res3 == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Current.Shutdown();
                }

            }
        }

    }
}
