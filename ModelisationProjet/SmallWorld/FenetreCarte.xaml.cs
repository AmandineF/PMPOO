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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Runtime.Serialization;

namespace SmallWorld
{

    public partial class FenetreCarte : Window
    {
        private Jeu jeu;
        private Tour tour;
        private const int numMaxImg = 12;
        private int numImgJ1;
        private int numImgJ2;
        private Border[,] plateauUnite;
        private Unite[,] uniteSelect;
        private Grid[,] gridsSelect;
        private const int tailleCase = 96;
        private int caseSelectX;
        private int caseSelectY;
        private Grid uniteGrid;
        // private Canvas mapGrid;
        private Border borderSelect;
        private Joueur joueur;
        private int nbTourInit;
        private Border caseSelect;
        private Grid gridSelect;
        private Boolean deuxJoueursTour;
        private Border[,] plateauConseil;
        private Border caseClic;
        private int ligneSelect;
        private int colonneSelect;
        private string nomDuFichier;
        private bool BoutonMenu;
        private bool BoutonAnnuler;
        private bool mapInfo;

        public FenetreCarte(Jeu j)
        {
            InitializeComponent();
            this.ligneSelect = 0;
            this.colonneSelect = 0;
            this.numImgJ1 = 1;
            this.numImgJ2 = 2 ;
            this.gridSelect = new Grid();
            this.jeu = j;
            this.plateauConseil = new Border[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.plateauUnite = new Border[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.uniteSelect = new Unite[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.gridsSelect = new Grid[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.joueur = this.jeu.getPremierJoueur();
            this.tour = new TourImpl(this.jeu, this.joueur);
            this.caseSelectX = -1;
            this.caseSelectY = -1;
            this.borderSelect = null;
            this.nbTourInit = 1;
            this.Menu.Text = "Manche " + this.nbTourInit + "/" + (this.jeu.getNbTours() + this.nbTourInit - 1);
            this.BoutonMenu = false;
            this.BoutonAnnuler = false;
            this.mapInfo = false;
            this.deuxJoueursTour = false;
            this.caseClic = null;
            this.KeyDown += new KeyEventHandler(this.gestionClavier);
            this.mapGrid.MouseLeave += new MouseEventHandler(this.echapCase);
            informationsJoueur();
            this.mapGrid.Focus();
            ImageBrush brush1 = new ImageBrush();
            brush1.ImageSource = new BitmapImage(new Uri(@"Ressources/Joueurs/player" + this.numImgJ1 + ".png", UriKind.Relative));
            this.ImgJ1.Fill = brush1;
            ImageBrush brush2 = new ImageBrush();
            brush2.ImageSource = new BitmapImage(new Uri(@"Ressources/Joueurs/player" + this.numImgJ2 + ".png", UriKind.Relative));
            this.ImgJ2.Fill = brush2;
            if (this.jeu.getCarte() is CarteNormale)
            {
                this.uiScaleSlider.Value = 0.5;
            }
            else if (this.jeu.getCarte() is CartePetite)
            {
                this.uiScaleSlider.Value = 0.7;
            }
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
                    hexagone.Cursor = Cursors.Hand;
                    hexagone.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
                    hexagone.MouseEnter += new MouseEventHandler(this.survolCase);
                    this.mapGrid.Children.Add(hexagone);
                    l = l + 100;
                }
                t = t + 75;
            }

            this.mapGrid.Width = this.jeu.getCarte().getTaille() * 100;
            this.mapGrid.Height = this.jeu.getCarte().getTaille() * 75;
            this.mapGrid.HorizontalAlignment = HorizontalAlignment.Center;
            this.mapGrid.VerticalAlignment = VerticalAlignment.Center;

            ajoutUnite();
            if (this.jeu.getCarte().getCase(0, 0).estCase(this.joueur))
            {
                this.caseSelectX = 0;
                this.caseSelectY = 0;
            }
            else
            {
                this.caseSelectX = this.jeu.getCarte().getTaille() - 1;
                this.caseSelectY = this.jeu.getCarte().getTaille() - 1;
            }
            selectionCase(false);
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
                        hexagoneUnite.Cursor = Cursors.Hand;

                        hexagoneUnite.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
                        hexagoneUnite.MouseEnter += new MouseEventHandler(this.survolCase);
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
            if ((coordX % 100) != 0)
                coordX -= 50;
            return (int)(coordX / 100);
        }


        private void clicGaucheCase(object sender, MouseButtonEventArgs e)
        {
            var selection = sender as Border;
            
            this.caseSelectX = this.getX(Canvas.GetLeft(selection));
            this.caseSelectY = this.getY(Canvas.GetTop(selection));
            if (this.caseClic == selection || this.tour.getUniteSelect() != null)
            {
                selectionCase(true);
                deplacement();
            }
            else
            {
                selectionCase(false);
            }
            
            
        }
        private void selectionCase(bool b)
        {

            if (this.caseClic != null)
                this.mapGrid.Children.Remove(this.caseClic);

            Border bordure = new Border();
            if (b)
            {
                bordure.Background = FabriqueImage.getInstance().getSelection(true);
            }
            else
            {
                bordure.Background = FabriqueImage.getInstance().getSelection(false);
            }
            //On sélectionne la case pour le déplacement
            bordure.Width = tailleCase;
            bordure.Height = tailleCase;
            bordure.Cursor = Cursors.Hand;
            bordure.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
            bordure.MouseEnter += new MouseEventHandler(this.survolCase);
            int ligne1 = 0;
            if (this.caseSelectY % 2 != 0)
            {
                ligne1 = (this.caseSelectX * 100) + 50;
            }
            else
            {
                ligne1 = this.caseSelectX * 100;
            }

            Canvas.SetLeft(bordure, ligne1);
            Canvas.SetTop(bordure, this.caseSelectY * 75);
            this.mapGrid.Children.Add(bordure);
            this.caseClic = bordure;
           // if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
          
        }

        private void deplacement()
        {
            if (this.tour.getUniteSelect() != null)
            {
                this.tour.selectionnerDestination(this.caseSelectY, this.caseSelectX);
                if (this.tour.deplacementPossible(this.caseSelectY, this.caseSelectX))
                {
                    this.deroulement.Text = this.tour.deplacementUnite();
                    effacerSelection();
                    effacerSuggestion();
                    ajoutUnite();
                }
                else
                {
                    this.deroulement.Text = "Impossible de se déplacer en (" + this.caseSelectY + " - " + this.caseSelectX + ")";
                    effacerSelection();
                }
                if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                    afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
            }

        }

        private void gestionClavier(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {

                case Key.Enter:
                    if (this.mapInfo)
                    {
                        //On sélectionne l'unité
                        //Mise en place de la bordure
                        effacerSelection();
                        Border bordure = new Border();
                        bordure.BorderBrush = Brushes.Blue;
                        bordure.BorderThickness = new Thickness(2);
                        Grid.SetRow(bordure, this.ligneSelect);
                        Grid.SetColumn(bordure, this.colonneSelect);
                        uniteGrid.Children.Add(bordure);
                        this.borderSelect = bordure;
                        //On enregistre l'unité sélectionnée
                        this.tour.selectionnerUnite(this.uniteSelect[this.ligneSelect, this.ligneSelect], this.caseSelectX, this.caseSelectY);
                    }
                    else
                    {
                        selectionCase(true);
                        deplacement();
                    }
                    break;
                //Gestion des déplacements gauche
                case Key.L:
                    //Aller à gauche dans la barre d'info
                    if (this.mapInfo)
                    {
                        if (this.caseClic != null)
                        {
                            if (this.colonneSelect > 0)
                                this.colonneSelect--;
                            else if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() >= ((this.colonneSelect + 1) * 2))
                                this.colonneSelect = 2;

                            clicUnite(this.ligneSelect, this.colonneSelect);
                        }
                    }
                    else
                    {
                        if (this.caseSelectX > 0)
                            this.caseSelectX = this.caseSelectX - 1;
                        else
                            this.caseSelectX = this.jeu.getCarte().getTaille() - 1;

                        selectionCase(false);
                        if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                            afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
                    }
                    break;
                //Gestion des déplacements droite
                case Key.R:
                    //Aller à droite dans la barre d'info
                    if (this.mapInfo)
                    {
                        if (this.caseClic != null)
                        {
                            if (this.colonneSelect < 2 && this.colonneSelect < (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() - 1))
                            {
                                this.colonneSelect++;
                            }
                            else
                            {
                                this.colonneSelect = 0;
                            }
                            clicUnite(this.ligneSelect, this.colonneSelect);

                        }
                    }
                    else
                    {
                        if (this.caseSelectX < this.jeu.getCarte().getTaille() - 1)
                            this.caseSelectX = this.caseSelectX + 1;
                        else
                            this.caseSelectX = 0;

                        selectionCase(false);
                        if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                            afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
                    }
                    break;
                //Gestion des déplacements vers le bas
                case Key.U:

                    if (this.mapInfo)
                    {
                        //Aller en haut dans la barre d'info
                        if (this.caseClic != null)
                        {
                            if (this.ligneSelect > 0)
                            {
                                this.ligneSelect--;
                            }
                            else if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 6)
                            {
                                this.ligneSelect = 2;
                            }
                            else if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 3)
                            {
                                this.ligneSelect = 1;
                            }

                            clicUnite(this.ligneSelect, this.colonneSelect);
                        }
                       
                    }
                    else
                    {
                        if (this.caseSelectY > 0)
                            this.caseSelectY = this.caseSelectY - 1;
                        else
                            this.caseSelectY = this.jeu.getCarte().getTaille() - 1;

                        selectionCase(false);
                        if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                            afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
                    }
                    break;

                //Gestion des déplacements vers le bas
                case Key.D:
                    if (this.mapInfo)
                    {
                        //Aller en bas dans la barre d'info
                        if (this.caseClic != null)
                        {
                            if (this.ligneSelect == 2)
                            {
                                this.ligneSelect = 1;
                            }
                            else if (this.ligneSelect == 1)
                            {
                                this.ligneSelect = 0;
                            }
                            else if (this.ligneSelect == 0 && this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 6)
                            {
                                this.ligneSelect = 2;
                            }
                            else if (this.ligneSelect == 0 && this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 3)
                            {
                                this.ligneSelect = 1;
                            }

                            clicUnite(this.ligneSelect, this.colonneSelect);
                        } 
                    }
                    else
                    {
                        if (this.caseSelectY < this.jeu.getCarte().getTaille() - 1)
                            this.caseSelectY = this.caseSelectY + 1;
                        else
                            this.caseSelectY = 0;

                        selectionCase(false);
                        if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                            afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
                        
                    }
                    break;

                case Key.Tab:
                    if (this.mapInfo)
                    {
                        this.mapInfo = false;
                    }
                    else
                    {
                        this.mapInfo = true;
                        if (caseClic != null && (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 0))
                        {
                            clicUnite(this.ligneSelect, this.colonneSelect);
                        }
                    }
                    break;

                case Key.OemPlus:
                    this.uiScaleSlider.Value -= 0.1;
                    break;
                case Key.Z:
                    this.uiScaleSlider.Value += 0.1;
                    break;
                default:
                    return;

                    
            }
        }

        private void survolCase(object sender, MouseEventArgs e)
        {
            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);

            var selection = sender as Border;
            Border bordure = new Border();
            bordure.Background = FabriqueImage.getInstance().getSelection(false);
            bordure.Width = tailleCase;
            bordure.Height = tailleCase;
            bordure.Cursor = Cursors.Hand;
            bordure.MouseLeftButtonDown += new MouseButtonEventHandler(this.clicGaucheCase);
            Canvas.SetLeft(bordure, Canvas.GetLeft(selection));
            Canvas.SetTop(bordure, Canvas.GetTop(selection));
            this.mapGrid.Children.Add(bordure);
            this.caseSelect = bordure;

            if (this.jeu.getCarte().getCase(this.getY(Canvas.GetTop(selection)), this.getX(Canvas.GetLeft(selection))).getUnite().Count() > 0)
            {
                afficherUniteCase(this.jeu.getCarte().getCase(this.getY(Canvas.GetTop(selection)), this.getX(Canvas.GetLeft(selection))).getUnite());
            }
            else if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 0)
            {
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
            }

        }

        private void echapCase(object sender, MouseEventArgs e)
        {
            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);
            if (this.caseClic == null && (this.caseClic != null && this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() == 0))
                effacementGrilleUnite();
            if (this.caseClic != null)
            {
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
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
                this.borderSelect.BorderBrush = new SolidColorBrush(Color.FromArgb(150, 244, 234, 229));

        }


        //Quand on clique sur une case qui contient des unités
        private void afficherUniteCase(List<Unite> lunite)
        {
            //effacerSelection();
            string vie = "";
            string attaque = "";
            string defense = "";
            string mouvement = "";
            int i = 0;
            int j = 0;
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
                this.uniteGrid.Focus();

                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        if (cpt < lunite.Count())
                        {
                            //Mise en place de la grille interne
                            Grid grille = new Grid();
                            grille.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                            grille.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                            Grid.SetRow(grille, i);
                            Grid.SetColumn(grille, j);
                            grille.MouseLeftButtonDown += new MouseButtonEventHandler(this.MouseLeftMapUnite);
                            grille.Cursor = Cursors.Hand;
                            uniteGrid.Children.Add(grille);

                            //Ajout de l'image de l'unité à la grille interne
                            Rectangle image = new Rectangle();
                            image.Fill = FabriqueImage.getInstance().getBrushUnite(lunite[cpt]);
                            image.Width = tailleCase;
                            image.Height = tailleCase;
                            image.HorizontalAlignment = HorizontalAlignment.Center;
                            Grid.SetRow(image, 0);
                            Grid.SetColumn(image, 0);
                            grille.Children.Add(image);

                            //Ajout des caractéristiques de l'unité à la grille interne
                            TextBlock texte = new TextBlock();
                            texte.TextAlignment = TextAlignment.Center;
                            vie = "Vie : " + lunite[cpt].getVie();
                            attaque = "Attaque : " + lunite[cpt].getAttaque();
                            defense = "Defense : " + lunite[cpt].getDefense();
                            mouvement = "Mouvement : " + lunite[cpt].getMouvement();
                            texte.Text = vie + "\n" + attaque + "\n" + defense + "\n" + mouvement;
                            //texte.Text = "\n" + vie + "\n" + mouvement;
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


       unsafe private void clicUnite(int ligne, int colonne) {
             effacerSelection();
             effacerSuggestion();
             this.mapInfo = true;
            //Si le joueur essaye de sélectionner des unités qui ne sont pas à lui
            if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).estCaseEnnemie(this.joueur))
            {
                MessageBox.Show("Impossible de sélectionner une case ennemie.");
            }
            else
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
                    this.ligneSelect = ligne;
                    this.colonneSelect = colonne;
            }
        }

        //Quand on clique sur une des unités de la barre d'information
        private void MouseLeftMapUnite(object sender, MouseButtonEventArgs e)
        {
            Grid grille = sender as Grid;
            int ligne = Grid.GetRow(grille);
            int colonne = Grid.GetColumn(grille);
            clicUnite(ligne, colonne);
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
                this.Menu.Text = "Manche " + this.nbTourInit + "/" + (this.jeu.getNbTours() + this.nbTourInit - 1);
                this.AuTourDeQui.Text = "C'est au tour de " + this.joueur.getPseudo() + " de jouer avec le peuple " + this.joueur.getNomPeuple() + ".";
               /* ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(@"Ressources/Joueurs/player" + numImgJoueur(this.joueur) + ".png", UriKind.Relative));
                this.ImgJoueurActuel.Fill = brush;
                this.ImgPeupleActuel.Fill = FabriqueImage.getInstance().getBrushUnite(this.joueur.getUnite(0)); */
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

        public void FenetreCarte_Closing(object sender, CancelEventArgs e)
        {

            MessageBoxResult res1 = MessageBox.Show("Voulez-vous sauvegarder la partie avant de quitter ?", "Sauver ?", MessageBoxButton.YesNoCancel);
            if (res1 == MessageBoxResult.Yes)
            {
                bool? res = sauvegarder();
                if (res == true)
                {
                    if (BoutonMenu == false)
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (res1 == MessageBoxResult.No)
            {
                if (BoutonMenu == false)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                BoutonAnnuler = true;
                BoutonMenu = false;
                e.Cancel = true;
            }
        }

        public bool? sauvegarderEnTantQue()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".sw";
            dialog.Filter = "SmallWorld (*.sw)|*.sw|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            Nullable<bool> res = dialog.ShowDialog();
            if (res == true)
            {
                this.nomDuFichier = dialog.FileName;

                Stream stream = File.Open(this.nomDuFichier, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.jeu);
                stream.Close();
            }
            return res;
        }
        private bool? sauvegarder()
        {
            bool? res;
            if (this.nomDuFichier == null)
            {
                res = this.sauvegarderEnTantQue();
            }
            else
            {
                Stream stream = File.Open(this.nomDuFichier, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.jeu);
                stream.Close();
                res = true;
            }
            return res;
        }


        private void changementImgJoueur1(object sender, MouseButtonEventArgs e)
        {
            if (this.numImgJ1 < numMaxImg)
                this.numImgJ1++;
            else
                this.numImgJ1 = 1;

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(@"Ressources/Joueurs/player" + this.numImgJ1 + ".png", UriKind.Relative));
            this.ImgJ1.Fill = brush;

        }
        private void changementImgJoueur2(object sender, MouseButtonEventArgs e)
        {
            if (this.numImgJ2 < numMaxImg)
                this.numImgJ2++;
            else
                this.numImgJ2 = 1;

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(@"Ressources/Joueurs/player" + this.numImgJ2 + ".png", UriKind.Relative));
            this.ImgJ2.Fill = brush;
        }
        private void RetourMenu_Click(object sender, RoutedEventArgs e)
        {
            BoutonMenu = true;
            this.Close();
            if (BoutonAnnuler == false)
            {
                new MainWindow().Show();
            }
            BoutonAnnuler = false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            sauvegarder();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.uiScaleSlider.Value += 0.1;
            }
            else if (e.Delta > 0)
            {
                this.uiScaleSlider.Value -= 0.1;
            }
        }
    }
}
