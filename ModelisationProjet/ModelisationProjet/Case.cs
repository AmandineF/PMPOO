using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public abstract class CaseImpl : Case
    {
        public List<Unite> listeUnite;

        public CaseImpl()
        {
            this.listeUnite = new List<Unite>();
        }

        public List<Unite> getUnite()
        {
            return this.listeUnite;
        }

        public void ajoutUnite(Unite unite) 
        {
            this.listeUnite.Add(unite);
        }

        public void supprimeUnite(Unite unite)
        {
            this.listeUnite.Remove(unite);
        }

        public bool estCaseEnnemie(Joueur j) 
        {
            bool res = false;
            if (this.listeUnite.Count() > 0)
            {
                if (this.listeUnite[1].getJoueur() != j)
                    res = true;
            }
            return res;
        }

        public bool estVide()
        {
            return (this.listeUnite.Count() == 0);
        }
    }

    public interface Case
    {
        List<Unite> getUnite();

        bool estCaseEnnemie(Joueur j);

        void ajoutUnite(Unite unite);

        bool estVide();

        void supprimeUnite(Unite unite);
    }
}
