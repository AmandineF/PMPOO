using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public abstract class UniteImpl : Unite
    {
        private const int POINTS_VIE_INITIAL = 5;
        private const int POINTS_ATTAQUE_INITIAL = 2;
        private const int POINTS_DEFENSE_INITIAL = 1;
        private const int POINTS_MVT_INITIAL = 2;
        private int ptVie;
        private int ptAttaque;
        private int ptDefense;
        private double ptMouvement;
        private Joueur proprietaire;

        public UniteImpl(Joueur j)
        {
            this.ptAttaque = POINTS_ATTAQUE_INITIAL;
            this.ptDefense = POINTS_DEFENSE_INITIAL;
            this.ptVie = POINTS_VIE_INITIAL;
            this.ptMouvement = POINTS_MVT_INITIAL;
            this.proprietaire = j;
        }

        public int defautPointsVie
        {
            get { return POINTS_VIE_INITIAL; }
        }
        public bool estVivante()
        {
            return (this.ptVie > 0);
        }

        public int getAttaque()
        {
           return this.ptAttaque;
        }

        public int getDefense()
        {
            return this.ptDefense;
        }

        public double getMouvement()
        {
            return this.ptMouvement;
        }

        public int getVie()
        {
            return this.ptVie;
        }

        public void setAttaque(int a)
        {
           this.ptAttaque = a;
        }

        public void setDefense(int d)
        {
            this.ptDefense = d;
        }

        public void setMouvement(double m)
        {
            this.ptMouvement = m;
        }

        public void setVie(int v)
        {
            this.ptVie = v;
        }

        public Joueur getJoueur()
        {
            return this.proprietaire;
        }

    }
}
