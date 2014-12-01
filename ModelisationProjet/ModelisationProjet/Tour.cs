using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //On redonne un point de mouvement à chaque unité au début du tour
            for (int i = 0; i < this.joueur.getNbUnite();i++)
            {
                this.joueur.getUnite(i).setMouvement(this.joueur.getUnite(i).getMouvement() + 1);
            }
        }

        public void selectionnerUnite(Unite u)
        {
            this.uniteSelect = u;
        }
        public void selectionnerUnite(Unite u, int x, int y)
        {
            this.uniteSelect = u;
            this.positionXselect = x;
            this.positionYselect = y;
        }

        public void selectionnerDestination(int x, int y)
        {
            this.positionXdest = x;
            this.positionYdest = y;
        }
        public bool deplacementPossible()
        {
            bool deplacement = true;
            if (this.uniteSelect == null)
            {
                Console.WriteLine("Aucune unité sélectionnée");
                deplacement = false;
            }
            else if (this.uniteSelect.getMouvement() < 1)
            {
                Console.WriteLine("Pas assez de point de mouvement");
                deplacement = false;
            }
            else if (this.positionXdest > this.positionXselect + 1 || this.positionYdest > this.positionYselect + 1)
            {
                Console.WriteLine("La case de destination est trop loin par rapport à la case actuelle");
                deplacement = false;
            }
            else if (this.uniteSelect is UniteNain && this.jeu.getCarte().getCase(positionXselect, positionYselect) is CaseMontagne)
            {
                deplacement = true;
            }
            else if (this.uniteSelect.getMouvement() < 1)
            {
                Console.WriteLine("L'unité sélectionnée n'a pas assez de points de mouvement pour se déplacer");
                deplacement = false;
            }
            else if (this.uniteSelect is UniteElf && this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest) is CaseDesert && this.uniteSelect.getMouvement() < 2)
            {
                Console.WriteLine("Un Elf doit avoir plus de 2 points de mouvement pour se déplacer sur une case Desert");
                deplacement = false;
            }
            return deplacement;
        }
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
                else if (this.jeu.getCarte().getCase(x,y) is CaseDesert)
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

        public void deselectionnerUnite()
        {
            this.uniteSelect = null;
        }

        public void deplacementUnite()
        {
            bool personne = true;
            if (this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).estCaseEnnemie(this.joueur))
            {
                personne = false;
                combattre(this.uniteSelect);
                if (this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).estCaseEnnemie(this.joueur))
                    personne = false;
            }

            if (personne && deplacementPossible())
            {
                this.jeu.getCarte().getCase(this.positionXselect, this.positionYselect).supprimeUnite(this.uniteSelect);
                this.jeu.getCarte().getCase(this.positionXdest, this.positionYdest).ajoutUnite(this.uniteSelect);
                consequenceDeplacement(this.positionXdest, this.positionYdest);
            }

            deselectionnerUnite();
        }

        public Unite meilleureUnite()
        {
            Unite res = null;
            List<Unite> u = this.jeu.getCarte().getCase(this.positionXselect, this.positionYselect).getUnite();
            res = u[0];
            for (int i = 1; i < u.Count ; i++)
            {
                if (res.getVie() < u[i].getVie()) 
                {
                        res = u[i];
                }
            }
            return res;
        }

        public void combattre(Unite attaque)
        {
            //selectionner meilleure unité ennemie
            Unite defense = meilleureUnite();

            //calcul du nombre de combat
            Random nbCombatRand = new Random();
            Random randCombat = new Random();
            int nbCombat = nbCombatRand.Next(3, (Math.Max(attaque.getVie(), defense.getVie()) + 2));
            //tant que le nombre de combat n'a pas été atteint et qu'aucune des unités n'est morte
            while(nbCombat > 0 && attaque.estVivante() && defense.estVivante()) 
            {
                //algorithme
                double ratioVieAttaque =(double) attaque.getVie() / (double)attaque.defautPointsVie;     
                double ratioVieDefense = (double)defense.getVie() / (double)defense.defautPointsVie;
                double ptAttaque = (double)ratioVieAttaque * (double)attaque.getAttaque();
                double ptDefense = (double)ratioVieDefense * (double)defense.getDefense();
                double ratioAttDef = (double)ptAttaque / (double)ptDefense;
                double ratioChancePerdre = 0.5 + ((1 - ratioAttDef) * 0.5);
                if (ratioChancePerdre < 0)
                {
                    ratioChancePerdre = 0;
                }
                
                double res = (double)((double)randCombat.Next(100) / 100);
                if (ratioChancePerdre < res)
                {
                    //Attaque gagne
                    defense.setVie(defense.getVie() - 1);
                }
                else
                {
                    //Défense gagne
                    attaque.setVie(attaque.getVie() - 1);
                }
                nbCombat--;
            }
        }

    }

    public interface Tour
    {
        void selectionnerUnite(Unite u);

        void selectionnerUnite(Unite u, int x, int y);

        void selectionnerDestination(int x, int y);

        void combattre(Unite attaque);

        Unite meilleureUnite();

        void deplacementUnite();

        void deselectionnerUnite();
    }
}
