using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class CarteImpl : Carte
    {

        private int taille;
        private Case[,] carte;
        private FabriqueCaseImpl fabrique;

        /// <summary>
        /// Créer une carte de manière aléatoire
        /// </summary>
        /// <param name="t">La taille de la matrice qui représentera la carte</param>
        public CarteImpl(int t)
        {
            this.taille = t;
            this.carte = new CaseImpl[taille, taille];
            int i, j, rand, cptDesert, cptMontagne, cptForet, cptPlaine;
            this.fabrique = new FabriqueCaseImpl();
            cptDesert = 0;
            cptMontagne = 0;
            cptForet = 0;
            cptPlaine = 0;
            Random r = new Random();
            bool b = true;
            for (i = 0; i < this.taille; i++)
            {
                for (j = 0; j < this.taille; j++)
                {
                    while (b)
                    {
                        rand = r.Next(1, 5);
                        if (rand == 1 && (cptDesert < ((this.taille * this.taille) / 4)))
                        {
                            carte[i, j] = fabrique.creerDesert();
                            cptDesert++;
                            b = false;
                        }
                        if (rand == 2 && (cptForet < ((this.taille * this.taille) / 4)))
                        {
                            carte[i, j] = fabrique.creerForet();
                            cptForet++;
                            b = false;
                        }
                        if (rand == 3 && (cptMontagne < ((this.taille * this.taille) / 4)))
                        {
                            carte[i, j] = fabrique.creerMontagne();
                            cptMontagne++;
                            b = false;
                        }
                        if (rand == 4 && (cptPlaine < ((this.taille * this.taille) / 4)))
                        {
                            carte[i, j] = fabrique.creerPlaine();
                            cptPlaine++;
                            b = false;
                        }           
                    }
                    b = true;
                }
            }
           
        }

        /// <summary>
        /// Permet de retourner une case de la matrice carte en fonction de ses coordonnées
        /// </summary>
        /// <param name="x">Coordonnée x, abscisse</param>
        /// <param name="y">Coordonnée y, ordonnée</param>
        /// <returns></returns>
        public Case getCase(int x, int y)
        {
            return this.carte[x, y];
        }

        /// <summary>
        /// Donne la taille de la carte
        /// </summary>
        /// <returns>Retourne la taille de la matrice carte</returns>
        public int getTaille()
        {
            return this.taille;
        }


        public int nbCasesColonisees(Joueur jo)
        {
            int total = 0;
            for (int i = 0; i < this.carte.GetLength(0); i++)
            {
                for (int j = 0; j < this.carte.GetLength(1); j++)
                {
                    if (this.carte[i,j].estCase(jo))
                    {
                        total++;
                    }
                }
            }
            return total;
        }

    }

    public interface Carte
    {
        Case getCase(int x, int y);

        int nbCasesColonisees(Joueur j);

        int getTaille();
    }
}
