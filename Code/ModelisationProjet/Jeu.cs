using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class JeuImpl : Jeu
    {
        private int nbTours;
        private Joueur joueur2;
        private Joueur joueur1;
        private Carte carte;

        public JeuImpl(int n, Joueur j1, Joueur j2, Carte c) 
        {
            this.nbTours = n;
            this.joueur1 = j1;
            this.joueur2 = j2;
            this.carte = c;
        }
        public int getNbTours()
        {
            return this.nbTours;
        }

        public Carte getCarte()
        {
            return this.carte;
        }

        public void decNbTours()
        {
            this.nbTours--;
        }
        public Joueur getJoueur1()
        {
            return this.joueur1;
        }

        public Joueur getJoueur2()
        {
           return this.joueur2;
        }

        public Joueur getGagnant()
        {
            if (this.joueur1.calculerPoints() > this.joueur2.calculerPoints())
            {
                return this.joueur1;
            }
            else if (this.joueur2.calculerPoints() > this.joueur1.calculerPoints())
            {
                return this.joueur2;
            }
            return null;
        }

        public Joueur getPerdant()
        {
            if (this.joueur1.calculerPoints() < this.joueur2.calculerPoints())
            {
                return this.joueur1;
            }
            else
            {
                return this.joueur2;
            }
        }

        public bool finDuJeu()
        {
            return (this.joueur1.calculerPoints() == 0 || this.joueur2.calculerPoints() == 0 || this.nbTours == 0);
        }
    }

    public interface Jeu
    {
        Joueur getPerdant();

        bool finDuJeu();

        Joueur getGagnant();

        Joueur getJoueur1();

        Joueur getJoueur2();

        Carte getCarte();

        void decNbTours();

        int getNbTours();
    }
}
