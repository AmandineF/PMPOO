using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class CreateurPartie
    {
        private int taille;
        private int nbTours;
        private int nbUnites;
        public MonteurPartieImpl monteurPartie;

        public CreateurPartie(int t)
        {
            //temporaire pour tests
            this.taille = t;
            this.nbUnites = 4;
            this.nbTours = 5;
        }
        public Jeu creerPartie(string pseudo1, Peuple peuple1, string pseudo2, Peuple peuple2)
        {
            int i;
            Carte c = new CarteImpl(this.taille);
            Joueur joueur1 = new JoueurImpl(peuple1, this.nbUnites, pseudo1);
            Joueur joueur2 = new JoueurImpl(peuple2, this.nbUnites, pseudo2);

            //calcul des coordonnées (à ranger ailleurs ?)
            Random r = new Random();
            int rand = 0;
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;

            rand = r.Next(1, 3);
            if (rand == 1)
            {
                x1 = r.Next(0, this.taille);
                x2 = x1;
                y1 = r.Next(1, 3);
                if (y1 == 1) {
                    y1 = 0;
                    y2 = this.taille-1;
                } else {
                    y1 = this.taille - 1;
                    y2 = 0;
                }
            }
            else
            {
                y1 = r.Next(0, this.taille);
                y2 = x1;
                x1 = r.Next(1, 3);
                if (x1 == 1)
                {
                    x1 = 0;
                    x2 = this.taille - 1;
                }
                else
                {
                    x1 = this.taille - 1;
                    x2 = 0;
                }

            }

            for (i = 0; i < this.nbUnites; i++)
            {
                Unite u = joueur1.getUnite(i);
                c.getCase(x1, y1).ajoutUnite(u);
                Unite v = joueur2.getUnite(i);
                c.getCase(x2, y2).ajoutUnite(v);
            }

            return new JeuImpl(this.nbTours, joueur1, joueur2, c);
        }

        public void chargerPartie()
        {
            throw new System.NotImplementedException();
        }
    }

}
