using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public abstract class MonteurPartieImpl : MonteurPartie
    {
        public StrategieCarte StrategieCarte
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    
        public void remplirCases()
        {
            throw new System.NotImplementedException();
        }

        public void setStrategie()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface MonteurPartie
    {
        void setStrategie();

        void remplirCases();
    }
}
