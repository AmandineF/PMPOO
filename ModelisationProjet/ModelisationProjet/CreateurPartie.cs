using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class CreateurPartie
    {
        private int nbTours;
        private int nbUnites;
        private Carte carte;
        //public MonteurPartieImpl monteurPartie;

        /// <summary>
        /// Constructeur du créateur de partie
        /// </summary>
        /// <param name="t">Taille de la carte voulue</param>
        public CreateurPartie(Carte c)
        {
            this.carte = c;

            if (this.carte.getTaille() == 6)
            {
                this.nbUnites = 4;
                this.nbTours = 10;
            }
            else if (this.carte.getTaille() == 10)
            {
                this.nbUnites = 6;
                this.nbTours = 20;
            }
            else 
            {
                this.nbUnites = 8;
                this.nbTours = 30;
            }
        }

        /// <summary>
        /// Crée une partie
        /// </summary>
        /// <param name="pseudo1">Pseudo du joueur 1</param>
        /// <param name="peuple1">Peuple du joueur 1</param>
        /// <param name="pseudo2">Pseudo du joueur 2</param>
        /// <param name="peuple2">Peuple du joueur 2</param>
        /// <returns>Le jeu créé</returns>
        public Jeu creerPartie(string pseudo1, Peuple peuple1, string pseudo2, Peuple peuple2)
        {
            int i;
            //Création des joueurs
            Joueur joueur1 = new JoueurImpl(peuple1, this.nbUnites, pseudo1);
            Joueur joueur2 = new JoueurImpl(peuple2, this.nbUnites, pseudo2);

            //calcul des coordonnées pour placer les unités au départ
            Random r = new Random();
            int rand = 0;
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;

            rand = r.Next(1, 3);
            if (rand == 1)
            {
                x1 = 0;
                y1 = 0;
                x2 = this.carte.getTaille() - 1;
                y2 = this.carte.getTaille() - 1;
               
            }
            else
            {
                x1 = this.carte.getTaille() - 1;
                y1 = this.carte.getTaille() - 1;
               x2 = 0;
               y2 = 0;
            }

            //Placement des unités
            for (i = 0; i < this.nbUnites; i++)
            {
                Unite u = joueur1.getUnite(i);
                this.carte.getCase(x1, y1).ajoutUnite(u);
                Unite v = joueur2.getUnite(i);
                this.carte.getCase(x2, y2).ajoutUnite(v);
            }

            return new JeuImpl(this.nbTours, joueur1, joueur2, this.carte);
        }

        public void chargerPartie()
        {
            throw new System.NotImplementedException();
        }
    }

}
