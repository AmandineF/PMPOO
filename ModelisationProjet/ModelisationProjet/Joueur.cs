using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public interface Joueur
    {
        Peuple getPeuple();

        int calculerPoints();

        string getPseudo();

        void removeUnite(Unite u);

        int getNbUnite();

        Unite getUnite(int i);

        void setPtVictoire(int v);

        int getPtVictoire();

        String getNomPeuple();

        void decNbUnite();
    }
}
