using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class CarteImpl : Carte
    {

        private int taille;
        private Case[,] listeCase;
        private FabriqueCaseImpl fabrique;

        public CarteImpl(int t)
        {
            this.taille = t;
            this.listeCase = new CaseImpl[taille, taille];
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
                            listeCase[i, j] = fabrique.creerDesert();
                            cptDesert++;
                            b = false;
                        }
                        if (rand == 2 && (cptForet < ((this.taille * this.taille) / 4)))
                        {
                            listeCase[i, j] = fabrique.creerForet();
                            cptForet++;
                            b = false;
                        }
                        if (rand == 3 && (cptMontagne < ((this.taille * this.taille) / 4)))
                        {
                            listeCase[i, j] = fabrique.creerMontagne();
                            cptMontagne++;
                            b = false;
                        }
                        if (rand == 4 && (cptPlaine < ((this.taille * this.taille) / 4)))
                        {
                            listeCase[i, j] = fabrique.creerPlaine();
                            cptPlaine++;
                            b = false;
                        }           
                    }
                    b = true;
                }
            }
           
        }
        public Case getCase(int x, int y)
        {
            return this.listeCase[x, y];
        }

        public int getTaille()
        {
            return this.taille;
        }
        public void Dessin()
        {
            int i, j;
            Console.Write("Dessin de la carte\n");
            for (i = 0; i < this.taille; i++)
            {
                Console.Write("\n");
                for (j = 0; j < this.taille; j++)
                {
                    if (listeCase[i, j] is CaseDesert)
                    {
                        Console.Write("Desert (" + listeCase[i,j].getUnite().Count() + ")");
                        
                    }
                    else if (listeCase[i, j] is CaseForet)
                    {
                        Console.Write("Forêt (" + listeCase[i,j].getUnite().Count() + ") ");
                    }
                    else if (listeCase[i, j] is CaseMontagne)
                    {
                        Console.Write("Montagne (" + listeCase[i, j].getUnite().Count() + ") ");
                    }
                    else if (listeCase[i, j] is CasePlaine)
                    {
                        Console.Write("Plaine (" + listeCase[i, j].getUnite().Count() + ") ");
                    }
                } 
            }

        }
    }

    public interface Carte
    {
        void Dessin();

        Case getCase(int x, int y);

        int getTaille();
    }
}
