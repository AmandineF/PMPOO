using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class JoueurImpl : Joueur
    {
        private string pseudo;
        private Peuple peuple;
        private List<Unite> listeUnite;
        private int nbUnite;
        private int ptVictoire;

        /// <summary>
        /// Construit un nouveau joueur
        /// </summary>
        /// <param name="p">Le peuple du joueur</param>
        /// <param name="n">le nombre d'unités du joueur</param>
        /// <param name="s">Le pseudo du joeuur</param>
        public JoueurImpl(Peuple p, int n, string s)
        {
            int i = 0;
            this.nbUnite = n;
            this.peuple = p;
            this.pseudo = s;
            this.listeUnite = new List<Unite>();
            if (this.peuple is PeupleElf)
            {
                for (i = 0; i < nbUnite; i++)
                {
                    this.listeUnite.Add(new UniteElf(this));
                }
            }
            else if (this.peuple is PeupleOrc)
            {
                for (i = 0; i < nbUnite; i++)
                {
                    this.listeUnite.Add(new UniteOrc(this));
                }
            }
            else if (this.peuple is PeupleNain)
            {
                for (i = 0; i < nbUnite; i++)
                {
                    this.listeUnite.Add(new UniteNain(this));
                }
            }
        }

        /// <summary>
        /// Donne une des unités que possède le joueur
        /// </summary>
        /// <param name="i">La position de l'unité voulue dans la liste</param>
        /// <returns>L'unité demandée</returns>
        public Unite getUnite(int i)
        {
            return this.listeUnite[i];
        }

        public void removeUnite(Unite u)
        {
            this.listeUnite.Remove(u);
        }

        /// <summary>
        /// Donne le peuple du joueur
        /// </summary>
        /// <returns>Le peuple du joueur</returns>
        public Peuple getPeuple()
        {
            return this.peuple;
        }

        /// <summary>
        /// Donne le nom du peuple en string
        /// </summary>
        /// <returns>Le nom du peuple</returns>
        public String getNomPeuple()
        {
            if (this.peuple is PeupleElf)
            {
                return "Elfe";
            }
            else if (this.peuple is PeupleNain)
            {
                return "Nain";
            } else if(this.peuple is PeupleOrc) {
                return "Orc";
            }
            return "";
        }

        /// <summary>
        /// Donne le pseudo du joueur
        /// </summary>
        /// <returns>Le pseudo du joueur</returns>
        public string getPseudo()
        {
            return this.pseudo;
        }

        /// <summary>
        /// Donne le nombre d'unités que possède le joueur
        /// </summary>
        /// <returns>Le nombre d'unités du joueur</returns>
        public int getNbUnite()
        {
            return this.nbUnite;
        }

        /// <summary>
        /// Décrémente le nombre d'unités que possède le joueur
        /// </summary>
        public void decNbUnite()
        {
           this.nbUnite--;
        }

        public void setPtVictoire(int v) {
            this.ptVictoire += v;
        }

        public int getPtVictoire() 
        {
            return this.ptVictoire;
        }
        /// <summary>
        /// calcule le nombre de points du joueur
        /// </summary>
        /// <returns>Le nombre de points du joueur</returns>
        public int calculerPoints()
        {
            int total = 0;
            foreach (Unite unite in listeUnite)
            {
                total += unite.getPtVictoire();
            }
            return total;
        }

    }
}
