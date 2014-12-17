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
        private Border caseSelect;
        private Grid gridSelect;
        private Boolean deuxJoueursTour;
        private Border[,] plateauConseil;
        private Border caseClic;
        private Dictionary<Border, Unite> findUnite;
        public FenetreCarte(Jeu j, Joueur p)
        {
            InitializeComponent();
            this.findUnite = new Dictionary<Border,Unite>();
            this.gridSelect = new Grid();
            this.jeu = j;
            this.plateauConseil = new Border[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
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
            this.caseClic = null;
            this.mapGrid = new Canvas();
            this.mapGrid.MouseLeave += new MouseEventHandler(this.echapCase);
           // this.barreInfo.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.flecheAction);
            informationsJoueur();
        }
       
        private void creerCarte(object sender, RoutedEventArgs e)
        {
            int t = 0;
            int l = 0;
            
            for (int x = 0; x < this.jeu.getCarte().getTaille(); x++)
            {
                if ((x % 2) != 0)
                    l = 50;
                else
                    l = 0;

                for (int y = 0; y < this.jeu.getCarte().getTaille(); y++)
                {
                    Border hexagone = new Border();
                    hexagone.Background = FabriqueImage.getInstance().getBrushCase(this.jeu.getCarte().getCase(x, y));
                    Canvas.SetLeft(hexagone, l);
                    Canvas.SetTop(hexagone, t);
                    hexagone.Width = tailleCase;
                    hexagone.Height = tailleCase;
                    //plateau[x, y] = hexagone;
                    hexagone.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
                    hexagone.MouseEnter += new MouseEventHandler(this.survolCase);
                    //hexagone.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.flecheAction);
                    //hexagone.MouseLeave += new MouseEventHandler(this.echapCase);
                    this.mapGrid.Children.Add(hexagone);
                    l = l + 100;
                }
                t = t + 75;
            }

            this.mapGrid.Margin = new Thickness(0, 80, 0, 0);
            KeyboardNavigation.SetTabNavigation(this.mapGrid, KeyboardNavigationMode.Cycle);
            this.FinTour.Margin = new Thickness(0, (this.jeu.getCarte().getTaille() * 80) + 30, 0, 0);
            sp.Children.Add(mapGrid);
            ajoutUnite();
        }

        private void ajoutUnite()
        {
            int t = 0;
            int l = 10;
            
            for (int x = 0; x < this.plateauUnite.GetLength(0); x++)
            {
                for (int y = 0; y < this.plateauUnite.GetLength(1); y++)
                {
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

                    if (units.Count() > 0)
                    {

                        Border hexagoneUnite = new Border();
                        hexagoneUnite.Background = FabriqueImage.getInstance().getBrushUnite(units[0]);
                        TextBlock unitText = new TextBlock();
                        hexagoneUnite.Width = tailleCase;
                        hexagoneUnite.Height = tailleCase;

                        unitText.Margin = new Thickness(50, 30, 0, 0);
                        unitText.Text = "" + units.Count();
                        unitText.FontSize = 40;
                        unitText.Foreground = Brushes.Black;
                        unitText.FontFamily = new FontFamily("Showcard Gothic");
                        hexagoneUnite.Child = unitText;
                        Canvas.SetLeft(hexagoneUnite, l);
                        Canvas.SetTop(hexagoneUnite, t);
                        hexagoneUnite.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
                        hexagoneUnite.MouseEnter += new MouseEventHandler(this.survolCase);
                        //hexagoneUnite.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.flecheAction);
                        //hexagoneUnite.MouseLeave += new MouseEventHandler(this.echapCase);
                        this.plateauUnite[x, y] = hexagoneUnite;
                        this.mapGrid.Children.Add(hexagoneUnite);
                    }
                    l = l + 100;
                }
                t = t + 75;
            }
        }

        private int getY(double coordY)
        {
            if (coordY == 0)
                return 0;
            return (int)(coordY / 75);
        }

        private int getX(double coordX)
        {
            if (coordX == 0)
                return 0;
            if ((coordX % 2) != 0)
                coordX -= 50;
            return (int)(coordX / 100);
        }

        private void clicGaucheCase(object sender, MouseButtonEventArgs e)
        {
            var selection = sender as Border;
            if (this.tour.getUniteSelect() == null)
            {
                if (this.caseClic != null)
                    this.mapGrid.Children.Remove(this.caseClic);

                Border bordure = new Border();
                bordure.Background = FabriqueImage.getInstance().getSelection(true);
                bordure.Width = tailleCase;
                bordure.Height = tailleCase;
                Canvas.SetLeft(bordure, Canvas.GetLeft(selection));
                Canvas.SetTop(bordure, Canvas.GetTop(selection));

                bordure.Focusable = true;
                //bordure.IsVisible = true;
               // bordure.KeyDown += new KeyEventHandler(this.flecheAction);
                bordure.KeyDown += new KeyEventHandler(this.flecheAction);
               // bordure.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.flecheAction);
                this.mapGrid.Children.Add(bordure);
                this.caseClic = bordure;
                this.caseSelectX = this.getX(Canvas.GetLeft(selection));
                this.caseSelectY = this.getY(Canvas.GetTop(selection));
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
            }
            else
            {
                this.caseSelectY = this.getX(Canvas.GetLeft(selection));
                this.caseSelectX = this.getY(Canvas.GetTop(selection));
                this.tour.selectionnerDestination(this.caseSelectX, this.caseSelectY);
                if (this.tour.deplacementPossible(this.caseSelectX, this.caseSelectY))
                {
                    this.deroulement.Text = this.tour.deplacementUnite();
                    effacerSelection();
                    effacerSuggestion();
                    ajoutUnite();
                    this.clicGaucheCase(sender, e);
                    
                }
                else
                {
                    MessageBox.Show("Impossible de se déplacer en (" + this.caseSelectX + " - " + this.caseSelectY + ")");
                }

            }
            
        }

        private void survolCase(object sender, MouseEventArgs e)
        {
            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);
            var selection = sender as Border;
            Border bordure = new Border();
            bordure.Background = FabriqueImage.getInstance().getSelection(true);
            bordure.Width = tailleCase;
            bordure.Height = tailleCase;
            Canvas.SetLeft(bordure, Canvas.GetLeft(selection));
            Canvas.SetTop(bordure, Canvas.GetTop(selection));
            bordure.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
            this.mapGrid.Children.Add(bordure);
            this.caseSelect = bordure;
            List<Unite> listeUnite = this.jeu.getCarte().getCase(this.getX(Canvas.GetLeft(selection)), this.getY(Canvas.GetTop(selection))).getUnite();
            if (listeUnite.Count() > 0)
            {
                afficherUniteCase(listeUnite);
            }
            else if(this.caseClic != null)
            {
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
            }
            
        }
        
        private void echapCase(object sender, MouseEventArgs e)
        {
            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);
        }
        
        private void flecheAction(object sender, KeyboardEventArgs e)
        {
            MessageBox.Show("oui");
            var selection = sender as Border;
           
                Border bordure = new Border();
                bordure.Background = FabriqueImage.getInstance().getSelection(true);
                bordure.Width = tailleCase;
                bordure.Height = tailleCase;
                this.caseSelectY = this.getX(Canvas.GetLeft(selection));
                if (this.caseSelectY > 0)
                    this.caseSelectY += 75;
                Canvas.SetLeft(bordure, this.caseSelectY);
                Canvas.SetTop(bordure, Canvas.GetTop(selection));
                bordure.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
                this.mapGrid.Children.Add(bordure);
                this.caseSelect = bordure;

        }

        private void effacementGrilleUnite()
        {
            if (this.uniteGrid != null)
            {
                for (int x = 0; x < this.uniteSelect.GetLength(0); x++)
                {
                    for (int y = 0; y < this.uniteSelect.GetLength(1); y++)
                    {
                        this.uniteGrid.Children.Remove(this.gridsSelect[x, y]);
                    }
                }
                this.gridsSelect = new Grid[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            }
        }

        private void effacerSuggestion()
        {
            for (int i = 0; i < this.plateauConseil.GetLength(0); i++)
            {
                for (int j = 0; j < this.plateauConseil.GetLength(1); j++)
                {
                    this.mapGrid.Children.Remove(plateauConseil[i, j]);
                }
            }
        }

        private void effacerSelection()
        {
            this.tour.deselectionnerUnite();
            if (this.borderSelect != null)
                this.borderSelect.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 244, 234, 229));

        }

            
        //Quand on clique sur une case qui contient des unités
        private void afficherUniteCase(List<Unite> lunite)
        {
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
                        if (cpt < lunite.Count())
                        {
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
                    this.tour.selectionnerUnite(this.uniteSelect[ligne, colonne], this.caseSelectX, this.caseSelectY);

                    //On "entoure" les cases où l'unité peut se déplacer
                    int i, j;
                    int tailleMap = this.jeu.getCarte().getTaille();
                    bool** carteBool = this.tour.recupererCarteSuggestion();
                    for (i = 0; i < tailleMap; i++)
                    {
                        for (j = 0; j < tailleMap; j++)
                        {
                            if (carteBool[i][j] == true)
                            {
                                //Console.Write("Case :" + i +" ; " + j + "------------");
                                var suggere = new Border();
                                suggere.Background = FabriqueImage.getInstance().getSuggere();
                                suggere.Width = tailleCase;
                                suggere.Height = tailleCase;
                                suggere.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
                                suggere.MouseEnter += new MouseEventHandler(this.survolCase);
                                int x = 75 * i;
                                int y = 100 * j;
                                if ((i % 2) != 0)
                                    y += 50;
                                Canvas.SetLeft(suggere, y);
                                Canvas.SetTop(suggere, x);
                                this.mapGrid.Children.Add(suggere);
                                this.plateauConseil[i, j] = suggere;
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
