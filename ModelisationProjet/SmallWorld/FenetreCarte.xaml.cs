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
        private const int tailleCase = 96;
        private const int numMaxImg = 11;
        private Jeu jeu;
        private Tour tour;
        private int numImgJ1;
        private int numImgJ2;
        private Border[,] plateauUnite;
        private Unite[,] uniteSelect;
        private int caseSelectX;
        private int caseSelectY;
        private Grid uniteGrid;
        private Border borderSelect;
        private Joueur joueur;
        private int nbTourInit = 1 ;
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
            this.numImgJ2 = 2;
            this.gridSelect = new Grid();
            this.jeu = j;
            this.plateauConseil = new Border[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.plateauUnite = new Border[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.uniteSelect = new Unite[this.jeu.getCarte().getTaille(), this.jeu.getCarte().getTaille()];
            this.joueur = this.jeu.getJoueurCourant();
            this.tour = new TourImpl(this.jeu, this.joueur);
            this.caseSelectX = -1;
            this.caseSelectY = -1;
            this.borderSelect = null;
            this.Menu.Text = "Manche " + (this.jeu.getNbToursTotal()-this.jeu.getNbTours()+1) + "/" + this.jeu.getNbToursTotal();
            this.BoutonMenu = false;
            this.BoutonAnnuler = false;
            this.mapInfo = false;
            this.infoFocusMap.Fill = FabriqueImage.getInstance().getFocusMap(true);
            this.infoFocusClavier.Fill = FabriqueImage.getInstance().getFocusClavier(false);
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
            this.AuTourDeQui.Text = "C'est au tour de " + this.joueur.getPseudo() + " de jouer avec le peuple " + this.joueur.getNomPeuple() + ".";
            if (this.jeu.getCarte() is CarteNormale)
            {
                this.uiScaleSlider.Value = 0.5;
            }
            else if (this.jeu.getCarte() is CartePetite)
            {
                this.uiScaleSlider.Value = 0.7;
            }
        }
      
        /// <summary>
        /// Mise en place de la carte
        /// </summary>
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
                    //Création de la case et placement dans le canvas
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

            //On détermine la taille du canvas pour pouvoir le centrer
            this.mapGrid.Width = this.jeu.getCarte().getTaille() * 100;
            this.mapGrid.Height = this.jeu.getCarte().getTaille() * 75;
            this.mapGrid.HorizontalAlignment = HorizontalAlignment.Center;
            this.mapGrid.VerticalAlignment = VerticalAlignment.Center;

            //On ajoute les unitézs sur la carte
            ajoutUnite();

            //On sélectionne la première case sélectionnée : celle qui contient toutes les unités du premier joueur qui va joueur
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
            selectionCase();
        }

        /// <summary>
        /// Mise en place des unités sur la carte
        /// </summary>
        private void ajoutUnite()
        {
            int t = 0;
            int l = 10;

            //Suppression des anciennes unités pour une mise à jour
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
                        //Mise en place de l'image de l'unité
                        Border hexagoneUnite = new Border();
                        hexagoneUnite.Background = FabriqueImage.getInstance().getBrushUnite(units[0]);
                        TextBlock unitText = new TextBlock();
                        hexagoneUnite.Width = tailleCase;
                        hexagoneUnite.Height = tailleCase;

                        //Mise en place de la zone de texte indiquant le nombre d'unités présentes sur la case
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


        /// <summary>
        /// Calcul de la coordonnée y d'une case en fonction de sa place sur le canvas
        /// </summary>
        /// <param name="coordY">La place sur le canvas de la case</param>
        /// <returns>La coordonnée y de la case dans le tableau des cases de la carte</returns>
        private int getY(double coordY)
        {
            if (coordY == 0)
                return 0;
            return (int)(coordY / 75);
        }

        /// <summary>
        /// Calcul de la coordonnée x d'une case en fonction de sa place sur le canvas
        /// </summary>
        /// <param name="coordX">La place sur le canvas de la case</param>
        /// <returns>La coordonnée x de la case dans le tableau des cases de la carte</returns>
        private int getX(double coordX)
        {
            if (coordX == 0)
                return 0;
            if ((coordX % 100) != 0)
                coordX -= 50;
            return (int)(coordX / 100);
        }


        /// <summary>
        /// Gestion du clic sur une case 
        /// </summary>

        private void clicGaucheCase(object sender, MouseButtonEventArgs e)
        {
            var selection = sender as Border;

            this.caseSelectX = this.getX(Canvas.GetLeft(selection));
            this.caseSelectY = this.getY(Canvas.GetTop(selection));
            selectionCase();

            if (this.tour.getUniteSelect() != null)
                deplacement();
        }

        /// <summary>
        /// Gestion de la sélection d'une case
        /// </summary>
        private void selectionCase()
        {
            //On supprime la bordure de l'ancienne case sélectionnée
            if (this.caseClic != null)
                this.mapGrid.Children.Remove(this.caseClic);

            effacerSuggestion();
            //Mise en place de la bordure sur la nouvelle case sélectionnée
            Border bordure = new Border();
            bordure.Background = FabriqueImage.getInstance().getSelection(true);
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

            //On affiche les unités de la case sélectionnée
            afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());

        }

        /// <summary>
        /// Gestion du déplacement des unités
        /// </summary>
        private void deplacement()
        {
            //On vérifie qu'une unité est bien sélectionnée
            if (this.tour.getUniteSelect() != null)
            {
                //On sélectionné la destination
                this.tour.selectionnerDestination(this.caseSelectY, this.caseSelectX);

                //Si le déplacement est possible sur cette destination
                if (this.tour.deplacementPossible(this.caseSelectY, this.caseSelectX))
                {
                    //On déplace l'unité et on met à jour les informations
                    this.deroulement.Text = this.tour.deplacementUnite();
                    effacerSelection();
                    effacerSuggestion();
                    ajoutUnite();
                    if (this.jeu.finDuJeu() == true)
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
                else
                {
                    //Si le déplacement est impossible on l'indique au joueur
                    this.deroulement.Text = "Impossible de se déplacer en (" + this.caseSelectY + " - " + this.caseSelectX + ")";
                    effacerSelection();
                }

                //On affiche les unités de la case sélectionnée
                if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                    afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
            }

        }

        /// <summary>
        /// Gestion des évenements clavier
        /// </summary>
        private void gestionClavier(object sender, KeyEventArgs e)
        {
            //On détecte la touche sélectionnée
            switch (e.Key)
            {

                //Si c'est la touche entrée
                case Key.Enter:

                    //Premier cas : le focus est sur la barre d'information
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
                    //Second cas : le focus est sur le canvas
                    else
                    {
                        selectionCase();
                        deplacement();
                    }
                    break;


                //Si c'est la touche L : gestion des déplacements gauche
                case Key.Q:
                    //Aller à gauche dans la barre d'info
                    if (this.mapInfo && this.caseClic != null && this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                    {
                        if (this.colonneSelect > 0)
                            this.colonneSelect--;
                        else if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() >= ((this.colonneSelect + 1) * 2))
                            this.colonneSelect = 2;

                        clicUnite(this.ligneSelect, this.colonneSelect);
                    }
                    else if (this.mapInfo == false)
                    {
                        if (this.caseSelectX > 0)
                            this.caseSelectX = this.caseSelectX - 1;
                        else
                            this.caseSelectX = this.jeu.getCarte().getTaille() - 1;

                        selectionCase();
                    }
                    break;

                //Si c'et la touche R : gestion des déplacements droite
                case Key.D:
                    //Aller à droite dans la barre d'info
                    if (this.mapInfo && this.caseClic != null && this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                    {
                        if (this.colonneSelect < 2 && this.colonneSelect < (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() - 1))
                            this.colonneSelect++;
                        else
                            this.colonneSelect = 0;

                        clicUnite(this.ligneSelect, this.colonneSelect);
                    }
                    else if (this.mapInfo == false)
                    {
                        if (this.caseSelectX < this.jeu.getCarte().getTaille() - 1)
                            this.caseSelectX = this.caseSelectX + 1;
                        else
                            this.caseSelectX = 0;

                        selectionCase();
                        if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                            afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());
                    }
                    break;


                //Si c'est la touche U : gestion des déplacements vers le bas
                case Key.Z:

                    if (this.mapInfo && this.caseClic != null && this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() > 0)
                    {
                        //Aller en haut dans la barre d'info
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
                    else if (this.mapInfo == false)
                    {
                        if (this.caseSelectY > 0)
                            this.caseSelectY = this.caseSelectY - 1;
                        else
                            this.caseSelectY = this.jeu.getCarte().getTaille() - 1;

                        selectionCase();
                    }
                    break;


                //Si c'est la touche D : gestion des déplacements vers le bas
                case Key.S:
                    if (this.mapInfo)
                    {
                        //Aller en bas dans la barre d'info
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
                    else if (this.mapInfo == false)
                    {
                        if (this.caseSelectY < this.jeu.getCarte().getTaille() - 1)
                            this.caseSelectY = this.caseSelectY + 1;
                        else
                            this.caseSelectY = 0;

                        selectionCase();
                    }
                    break;

                //Si c'est la touche Tab : Changement du focus clavier
                case Key.Tab:
                    //Si le focus était sur la barre d'information, le focus passe sur le canvas
                    if (this.mapInfo)
                    {
                        this.mapInfo = false;
                        this.infoFocusMap.Fill = FabriqueImage.getInstance().getFocusMap(true);
                        this.infoFocusClavier.Fill = FabriqueImage.getInstance().getFocusClavier(false);
                    }

                    //Si le focus était sur le canvas, le focus passe sur la barre d'information et on sélectionne la première unité
                    else
                    {
                        this.mapInfo = true;
                        this.infoFocusMap.Fill = FabriqueImage.getInstance().getFocusMap(false);
                        this.infoFocusClavier.Fill = FabriqueImage.getInstance().getFocusClavier(true);
                        this.ligneSelect = 0;
                        this.colonneSelect = 0;
                        clicUnite(this.ligneSelect, this.colonneSelect);
                    }
                    break;

                //Si c'est la touche espace : on gère la fin du tour
                case Key.P:
                    FinDuTour();
                    this.deroulement.Text = "Tour suivant !";
                    break;

                //Si c'est la touche - : On zoome vers l'arrière
                case Key.OemMinus:
                    this.uiScaleSlider.Value -= 0.1;
                    break;

                //Si c'est la touche Z : On zomme vers l'avant
                case Key.U:
                    this.uiScaleSlider.Value += 0.1;
                    break;

                default:
                    return;


            }
        }

        /// <summary>
        /// Gestion du survol souris sur une case du canvas
        /// </summary>
        private void survolCase(object sender, MouseEventArgs e)
        {
            //On commence par supprimer l'ancienne case survolée
            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);

            //On ajoute une bordure autour de la case survolée
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

            //Si la case survolée contient des unités on les affiche, sinon on affiche les unités de la case sélecionnée
            if (this.jeu.getCarte().getCase(this.getY(Canvas.GetTop(selection)), this.getX(Canvas.GetLeft(selection))).getUnite().Count() > 0)
            {
                afficherUniteCase(this.jeu.getCarte().getCase(this.getY(Canvas.GetTop(selection)), this.getX(Canvas.GetLeft(selection))).getUnite());
            }
            else if (this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite().Count() > 0)
            {
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectX, this.caseSelectY).getUnite());
            }

        }

        /// <summary>
        /// Gestion de la souris du canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void echapCase(object sender, MouseEventArgs e)
        {
            //On enlève le repère de survol
            if (this.caseSelect != null)
                this.mapGrid.Children.Remove(this.caseSelect);

            //On supprime la grille des unités si aucune case n'est sélectionnée
            if (this.caseClic == null && (this.caseClic != null && this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite().Count() == 0))
                effacementGrilleUnite();

            //Si une case est sélectionnée on laisse la grille des unités
            if (this.caseClic != null)
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());

        }

        /// <summary>
        /// Méthode pour effacer la grille des unités de la barre d'information 
        /// </summary>
        private void effacementGrilleUnite()
        {
            this.barreInfo.Children.Remove(this.uniteGrid);
        }

        /// <summary>
        /// Méthode pour effacer les suggestions de cases
        /// </summary>
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

        /// <summary>
        /// Méthode pour effacer la sélection d'une unité dans la barre d'information
        /// </summary>
        private void effacerSelection()
        {
            this.tour.deselectionnerUnite();
            if (this.borderSelect != null)
                this.borderSelect.BorderBrush = new SolidColorBrush(Color.FromArgb(150, 244, 234, 229));

        }


        /// <summary>
        /// Affichage des unités d'une case 
        /// </summary>
        /// <param name="lunite">La liste des unités de la case</param>
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

            //On vérifie que la case contient bien des unités
            if (lunite.Count() > 0)
            {
                //Création de la grille qui va contenir les unités dans la barre d'information
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
                            Grid.SetRow(texte, 1);
                            Grid.SetColumn(texte, 0);
                            grille.Children.Add(texte);

                            //On enregistre les unités pour pouvoir détecter par la suite leur sélection 
                            this.uniteSelect[i, j] = lunite[cpt];
                            cpt++;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gestion de la sélection d'une unité
        /// </summary>
        /// <param name="ligne">La ligne sur laquelle se trouve l'unité sélectionnée dans la grille</param>
        /// <param name="colonne">La colonne sur laquelle se trouve l'unité sélectionnée dans la grille</param>
        unsafe private void clicUnite(int ligne, int colonne)
        {

            effacerSelection(); //On efface l'ancienne sélection
            effacerSuggestion();  //On efface les anciennes suggestions de cases
            this.mapInfo = true; //Mise à jour de l'indicateur de focus pour indiquer que l'utilisateur intéragit avec la barre d'information

            //Si le joueur essaye de sélectionner des unités qui ne sont pas à lui
            if (this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).estCaseEnnemie(this.joueur))
            {
                MessageBox.Show("Impossible de sélectionner une case ennemie.");
            }
            else
            {
                //Mise en place de la bordure autour de l'unité sélectionnée
                Border bordure = new Border();
                bordure.BorderBrush = Brushes.Black;
                bordure.BorderThickness = new Thickness(1);
                Grid.SetRow(bordure, ligne);
                Grid.SetColumn(bordure, colonne);
                uniteGrid.Children.Add(bordure);

                //On enregistre l'unité sélectionnée
                this.tour.selectionnerUnite(this.uniteSelect[ligne, colonne], this.caseSelectY, this.caseSelectX);

                //On met en place une bordure sur les cases où l'unité peut se déplacer
                int i, j;
                int tailleMap = this.jeu.getCarte().getTaille();
                bool** carteBool = this.tour.recupererCarteSuggestion();
                for (i = 0; i < tailleMap; i++)
                {
                    for (j = 0; j < tailleMap; j++)
                    {
                        if (carteBool[i][j] == true)
                        {
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



        /// <summary>
        /// Gestion du clic sur une des unités de la barre d'infomartion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftMapUnite(object sender, MouseButtonEventArgs e)
        {
            //On récupère le numéro de ligne et de colonne de l'unité sur laquelle l'utilisateur a cliqué
            Grid grille = sender as Grid;
            int ligne = Grid.GetRow(grille);
            int colonne = Grid.GetColumn(grille);
            clicUnite(ligne, colonne);
        }

        /// <summary>
        /// Gestion de l'affichage des informations joueurs
        /// </summary>
        private void informationsJoueur()
        {
            string uniteRestantes = "";
            string pointVictoire = "";
            string peuple = "";

            //Informations joueur 1
            this.pseudoJ1.Text = this.jeu.getJoueur1().getPseudo();
            peuple = "Peuple " + this.jeu.getJoueur1().getNomPeuple() + "\n"; //On affiche le peuple du joueur
            uniteRestantes = this.jeu.getJoueur1().getNbUnite() + " unités restantes\n"; //On affiche le nombre d'unités restantes au joueur
            pointVictoire = this.jeu.getJoueur1().getPtVictoire() + " points.\n"; //On affiche le nombre de points de victoire du joueur
            this.infoJ1.Text = peuple + uniteRestantes + pointVictoire;

            //Informations joueur 2
            this.pseudoJ2.Text = this.jeu.getJoueur2().getPseudo();
            peuple = "Peuple " + this.jeu.getJoueur2().getNomPeuple() + "\n"; //On affiche le peuple du joueur
            uniteRestantes = this.jeu.getJoueur2().getNbUnite() + " unités restantes\n"; //On affiche le nombre d'unités restantes au joueur
            pointVictoire = this.jeu.getJoueur2().getPtVictoire() + " points.\n"; //On affiche le nombre de points de victoire du joueur
            this.infoJ2.Text = peuple + uniteRestantes + pointVictoire;
        }


        private void FinTour_Click(object sender, RoutedEventArgs e)
        {
            FinDuTour();
            this.deroulement.Text = "Tour suivant !";
        }
        /// <summary>
        /// Gestion du clic sur le bouton fin de tour
        /// </summary>
        private void FinDuTour()
        {
            //Si le nombre de tour maximal n'a pas été atteint ou si aucun des deux joueurs n'est mort
            if (this.jeu.getNbTours() > 1 && (this.jeu.finDuJeu() == false))
            {
                //Si les deux joueurs ont joué le tour actuel
                if (this.deuxJoueursTour)
                {
                    //On met à jour les points de victoire
                    this.jeu.getJoueur1().setPtVictoire(this.jeu.getJoueur1().calculerPoints() + this.jeu.getCarte().nbCasesColonisees(this.jeu.getJoueur1()));
                    this.jeu.getJoueur2().setPtVictoire(this.jeu.getJoueur2().calculerPoints() + this.jeu.getCarte().nbCasesColonisees(this.jeu.getJoueur2()));

                    //On décrémente le nombre de tour
                    this.jeu.decNbTours();
                    this.jeu.reinitialisation();
                    this.nbTourInit++;
                    this.deuxJoueursTour = false;
                }
                else
                {
                    //Sinon on fait jouer le deuxième joueur
                    this.deuxJoueursTour = true;
                }

                //On inverse le joueur actuel
                if (this.joueur == this.jeu.getJoueur1())
                {
                    this.joueur = this.jeu.getJoueur2();
                    this.jeu.setJoueurCourant(this.joueur);
                }
                else
                {
                    this.joueur = this.jeu.getJoueur1();
                    this.jeu.setJoueurCourant(this.joueur);
                }

                //On crée un nouveau tour
                this.tour = new TourImpl(this.jeu, this.joueur);

                //On met à jour les informations affichées
                this.Menu.Text = "Manche " + (this.jeu.getNbToursTotal() - this.jeu.getNbTours() + 1) + "/" + this.jeu.getNbToursTotal();
                this.AuTourDeQui.Text = "C'est au tour de " + this.joueur.getPseudo() + " de jouer avec le peuple " + this.joueur.getNomPeuple() + ".";
                informationsJoueur();

                //On affiche les unités sur la case actuellement sélectionnée
                afficherUniteCase(this.jeu.getCarte().getCase(this.caseSelectY, this.caseSelectX).getUnite());

            }
            else
            {
                //Sinon on détermine le gagnant et on affiche le résultat
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


        /// <summary>
        /// Gestion de la fermeture de la fenêtre
        /// </summary>
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

        /// <summary>
        /// Gestion de la sauvegarde
        /// </summary>
        /// <returns>Vrai si la sauvegarde s'est bien passée</returns>
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
            else
            {
                BoutonMenu = false;
            }
            return res;
        }

        /// <summary>
        /// Gestion de la sauvegarde
        /// </summary>
        /// <returns>Vrai si la sauvegarde s'est bien passée</returns>
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



        /// <summary>
        /// Gestion du changement d'image du joueur 1 (au clic sur l'image)
        /// </summary>
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

        /// <summary>
        /// Gestion du changement d'image du joueur 2 (au clic sur l'image)
        /// </summary>
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

        /// <summary>
        /// Gestion du clic sur le bouton de retour au menu
        /// </summary>
        private void RetourMenu_Click(object sender, RoutedEventArgs e)
        {
            BoutonMenu = true;

            this.Close();
            if (BoutonAnnuler == false )
            {
                if(BoutonMenu != false)
                new MainWindow().Show();
            }
            BoutonAnnuler = false;
            
        }

        /// <summary>
        /// Gestion du clic sur le bouton de sauvegarde
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool? res = sauvegarder();
            if (res == true)
            {
                this.deroulement.Text = "Partie sauvegardée !";
            }
        }

        /// <summary>
        /// Gestion de la roulette de la souris pour zoomer la carte
        /// </summary>
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.uiScaleSlider.Value -= 0.1;
            }
            else if (e.Delta > 0)
            {
                this.uiScaleSlider.Value += 0.1;
            }
        }

        /// <summary>
        /// Donne le focus clavier à la barre d'information
        /// </summary>
        private void donnerFocusClavier(object sender, MouseButtonEventArgs e)
        {
            this.mapInfo = true;
            this.infoFocusMap.Fill = FabriqueImage.getInstance().getFocusMap(false);
            this.infoFocusClavier.Fill = FabriqueImage.getInstance().getFocusClavier(true);
        }

        /// <summary>
        /// Donne le focus clavier à la carte 
        /// </summary>
        private void donnerFocusMap(object sender, MouseButtonEventArgs e)
        {
            this.mapInfo = false;
            this.infoFocusMap.Fill = FabriqueImage.getInstance().getFocusMap(true);
            this.infoFocusClavier.Fill = FabriqueImage.getInstance().getFocusClavier(false);
        }
    }
}
