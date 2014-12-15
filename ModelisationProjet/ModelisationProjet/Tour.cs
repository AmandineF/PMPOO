using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper;

namespace ModelisationProjet
{
    public class TourImpl : Tour
    {
        private Jeu jeu;
        private Joueur joueur;
        private int positionXselect;
        private int positionYselect;
        private int positionXdest;
        private int positionYdest;
        private Unite uniteSelect;

        public TourImpl(Jeu j, Joueur p)
        {
            this.jeu = j;
            this.joueur = p;
            uniteSelect = null;

        }

        /// <summary>
        /// On rend l'unité sélectionnée
        /// </summary>
        /// <returns>Unité sélectionnée</returns>
        public Unite getUniteSelect()
        {
            return this.uniteSelect;
        }

        /// <summary>
        /// On sélectionne une unité
        /// </summary>
        /// <param name="u">L'unité sélectionnée</param>
        /// <param name="x">Abscisse de la case sélectionnée</param>
        /// <param name="y">Ordonnée de la case sélectionnée </param>
        public void selectionnerUnite(Unite u, int x, int y)
        {
            this.uniteSelect = u;
            this.positionXselect = x;
            this.positionYselect = y;

        }

        /// <summary>
        /// On sélectionne une destination
        /// </summary>
        /// <param name="x">Abscisse de la case de destination</param>
        /// <param name="y">Ordonnée de la case de destination</param>
        public void selectionnerDestination(int x, int y)
        {
            this.positionXdest = x;
            this.positionYdest = y;
        }

        /// <summary>
        /// On vérifie si le déplacement de la case sélectionnée vers la case de destination est possible
        /// </summary>
        /// <returns>Vrai si le déplacement est possible, faux sinon</returns>
        public bool deplacementPossible()
        {
            bool deplacement = true;
            if (this.uniteSelect == null)
            {
                // Console.WriteLine("Aucune unité sélectionnée");
                deplacement = false;
            }
            if (this.uniteSelect.getMouvement() < 1)
            {
                //Console.WriteLine("Pas assez de point de mouvement");
                deplacement = false;
            }
            if (this.positionXdest > this.positionXselect + 1 || this.positionYdest > this.positionYselect + 1 || this.positionXdest < this.positionXselect - 1 || this.positionYdest < this.positionYselect - 1)
            {
                //Console.WriteLine("La case de destination est trop loin par rapport à la case actuelle");
                deplacement = false;
            }
            if ((this.uniteSelect is UniteNain) && (this.jeu.getCarte().getCase(positionXselect, positionYselect) is CaseMontagne) && (this.jeu.getCarte().getCase(positionXdest, positionYdest) is CaseMontagne))
            {
                deplacement = true;
            }
            if (this.uniteSelect is UniteElf && this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest) is CaseDesert)
            {
                //Console.WriteLine("Un Elf doit avoir plus de 2 points de mouvement pour se déplacer sur une case Desert");
                deplacement = false;
            }
            return deplacement;
        }

        /// <summary>
        /// On enlève des points de mouvement à l'unité déplacée
        /// </summary>
        /// <param name="x">L'abscisse de la case sur laquelle l'unité s'est déplacée</param>
        /// <param name="y">L'ordonnée de la case sur laquelle l'unité s'est déplacée</param>
        public void consequenceDeplacement(int x, int y)
        {
            if (this.uniteSelect is UniteElf)
            {
                //Le coût de déplacement sur une case Forêt est divisée par deux pour les Elfes
                if (this.jeu.getCarte().getCase(x, y) is CaseForet)
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 0.5);
                }
                //Le coût de déplacement sur une case Désert est multipliée par deux pour les Elfes
                else if (this.jeu.getCarte().getCase(x, y) is CaseDesert)
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 2);
                }
                else
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 1);
                }
            }
            else if (this.uniteSelect is UniteOrc)
            {
                //Le coût de déplacement sur une case Plaine est divisée par deux pour les Orcs
                if (this.jeu.getCarte().getCase(x, y) is CasePlaine)
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 0.5);
                }
                else
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 1);
                }
            }
            else if (this.uniteSelect is UniteNain)
            {
                //Le coût de déplacement sur une case Plaine est divisée par deux pour les Nains
                if (this.jeu.getCarte().getCase(x, y) is CasePlaine)
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 0.5);
                }
                else
                {
                    this.uniteSelect.setMouvement(this.uniteSelect.getMouvement() - 1);
                }
            }
        }

        /// <summary>
        /// On déselectionne l'unité sélectionnée
        /// </summary>
        public void deselectionnerUnite()
        {
            this.uniteSelect = null;
        }

        /// <summary>
        /// On déplace l'unité sélectionnée sur la case de destination sélectionnée
        /// </summary>
        /// <returns>L'affichage du déplacement pour informer les joueurs</returns>
        public string deplacementUnite()
        {
            string affichage = "";
            bool personne = true;
            if (this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).estCaseEnnemie(this.joueur))
            {
                personne = false;
                affichage += combattre(this.uniteSelect);
                if (this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).estCaseEnnemie(this.joueur))
                {
                    personne = false;
                }
                else
                {
                    personne = true;
                }
            }

            if (personne && deplacementPossible())
            {
                this.jeu.getCarte().getCase(this.positionXselect, this.positionYselect).supprimeUnite(this.uniteSelect);
                this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).ajoutUnite(this.uniteSelect);
                consequenceDeplacement(this.positionXdest, this.positionYdest);
                affichage += "Déplacement de l'unité sélectionnée sur la case (" + this.positionXdest + " - " + this.positionYdest + ").\n";
            }

            deselectionnerUnite();
            return affichage;
        }

        /// <summary>
        /// Méthode pour choisir la meilleure unité présente sur une case
        /// </summary>
        /// <returns>La meilleure unité</returns>
        public Unite meilleureUnite()
        {
            Unite res = null;
            List<Unite> u = this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).getUnite();
            res = u[0];
            for (int i = 1; i < u.Count; i++)
            {
                if (res.getVie() < u[i].getVie())
                {
                    res = u[i];
                }
            }
            return res;
        }

        /// <summary>
        /// Méthode de combat entre deux unités
        /// </summary>
        /// <param name="attaque">L'unité attaquante</param>
        /// <returns>L'affichage du déroulement du combat pour informer les joueurs</returns>
        public string combattre(Unite attaque)
        {
            string affichage = "";
            //selectionner meilleure unité ennemie
            Unite defense = meilleureUnite();

            //calcul du nombre de combat
            Random nbCombatRand = new Random();
            Random randCombat = new Random();
            int nbCombat = nbCombatRand.Next(3, (Math.Max(attaque.getVie(), defense.getVie()) + 2));
            //tant que le nombre de combat n'a pas été atteint et qu'aucune des unités n'est morte
            affichage = "Le combat dure " + nbCombat + " tours.\n";
            int nbRound = 0;
            while (nbRound < nbCombat && attaque.estVivante() && defense.estVivante())
            {
                //algorithme
                double ratioVieAttaque = (double)attaque.getVie() / (double)attaque.defautPointsVie;
                double ratioVieDefense = (double)defense.getVie() / (double)defense.defautPointsVie;
                double ptAttaque = (double)ratioVieAttaque * (double)attaque.getAttaque();
                double ptDefense = (double)ratioVieDefense * (double)defense.getDefense();
                double ratioAttDef = (double)(ptAttaque / ptDefense);
                double ratioChanceDef = 0;
                if (ratioAttDef > 1)
                {
                    ratioChanceDef = (1 / ratioAttDef) / 2;
                    ratioChanceDef = (0.5 - ratioChanceDef) + 0.5;
                }
                else if (ratioAttDef == 1)
                {
                    ratioChanceDef = 0.5;
                }
                else
                {
                    ratioChanceDef = ratioAttDef / 2;
                }

                double res = (double)((double)randCombat.Next(100) / 100);
                if (ratioChanceDef >= res)
                {
                    //Attaque gagne
                    defense.setVie(defense.getVie() - 1);
                    affichage += "Round " + nbRound + " : défaite de la défense (" + defense.getJoueur().getNomPeuple() + ").\n";
                }
                else
                {
                    //Défense gagne
                    attaque.setVie(attaque.getVie() - 1);
                    affichage += "Round " + nbRound + " : défaite de l'attaque (" + attaque.getJoueur().getNomPeuple() + ").\n";
                }
                nbRound++;
            }

            defense.setDefense(defense.getDefense() - 1);
            attaque.setAttaque(attaque.getAttaque() - 1);

            if (!defense.estVivante())
            {
                this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).supprimeUnite(defense);
                defense.getJoueur().decNbUnite();
                attaque.setPtVictoire(attaque.getPtVictoire() + 1);
                affichage += "Dèces de la défense (" + defense.getJoueur().getNomPeuple() + "). \nL'attaque gagne 1 point de victoire.\n";
                defense.getJoueur().removeUnite(defense);
            }
            else if (!attaque.estVivante())
            {
                this.jeu.getCarte().getCase(this.positionXselect, this.positionYselect).supprimeUnite(attaque);
                attaque.getJoueur().decNbUnite();
                defense.setPtVictoire(defense.getPtVictoire() + 1);
                affichage += "Dèces de l'attaque (" + attaque.getJoueur().getNomPeuple() + "). \nLa défense gagne un point de victoire\n";
            }
            else
            {
                consequenceDeplacement(this.positionXdest, this.positionYdest);
                affichage += "Aucun décès. \n";
            }
            return affichage;
        }

        unsafe public int[][] creerCarteElement(int taille)
        {
            taille = this.jeu.getCarte().getTaille();
            int[][] carteElement = new int[taille][];
            for (int k = 0; k < taille; k++)
            {
                carteElement[k] = new int[taille];
            }
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    if (this.jeu.getCarte().getCase(i, j) is CaseMontagne)
                    {
                        carteElement[i][j] = 1; // Montagne
                    }
                    else if (this.jeu.getCarte().getCase(i, j) is CasePlaine)
                    {
                        carteElement[i][j] = 2; // Plaine
                    }
                    else if (this.jeu.getCarte().getCase(i, j) is CaseForet)
                    {
                        carteElement[i][j] = 3; // Foret
                    }
                    else if (this.jeu.getCarte().getCase(i, j) is CaseDesert)
                    {
                        carteElement[i][j] = 4; // Desert
                    }
                }
            }
            return carteElement;
        }

        unsafe public bool** recupererCarteSuggestion()
        {
            int type = 0;
            if(uniteSelect is UniteNain){
                type = 1;
            }else if(uniteSelect is UniteElf) {
                type = 2;
            }else if(uniteSelect is UniteOrc) {
                type = 3;
            }

            int[][] carteElement = creerCarteElement(this.jeu.getCarte().getTaille());
            WrapperAlgo wp = new WrapperAlgo();
            bool** carteBool = wp.suggestion(this.jeu.getCarte().getTaille(), this.positionXselect, this.positionYselect, type, carteElement, uniteSelect.getMouvement());
            return carteBool;
        }

    }
        unsafe public interface Tour
        {
            void selectionnerUnite(Unite u, int x, int y);

            void selectionnerDestination(int x, int y);

            string combattre(Unite attaque);

            Unite meilleureUnite();

            string deplacementUnite();

            bool deplacementPossible();

            void deselectionnerUnite();

            Unite getUniteSelect();
            int[][] creerCarteElement(int taille);
            bool** recupererCarteSuggestion();
        }
    }
