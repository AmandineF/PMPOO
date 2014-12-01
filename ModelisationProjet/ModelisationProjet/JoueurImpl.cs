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

        public Unite getUnite(int i)
        {
            return this.listeUnite[i];
        }
        public Peuple getPeuple()
        {
            return this.peuple;
        }
        public string getPseudo()
        {
            return this.pseudo;
        }
        public int getNbUnite()
        {
            return this.nbUnite;
        }
        public int calculerPoints()
        {
            int total = 0;
            foreach (UniteImpl unite in listeUnite)
            {
                total += unite.getVie();
            }
            return total;
        }
    }
}
