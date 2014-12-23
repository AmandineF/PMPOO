using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public interface Unite
    {
        bool estVivante();
        int getAttaque();

        int getDefense();

        double getMouvement();

        void incPtCase();

        int getPtCase();
        int getVie();

        void setAttaque(int a);

        void setDefense(int d);

        void setMouvement(double m);

        void setVie(int v);

        Joueur getJoueur();

        int defautPointsVie { get; }

        int defautPointsMvt { get; }
        
        int defautPointsAttaque { get; }

        int defautPointsDefense { get; }

        void setPtVictoire(int v);

        int getPtVictoire();
    }
}
