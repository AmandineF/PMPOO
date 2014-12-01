﻿using System;
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

        int getVie();

        void setAttaque(int a);

        void setDefense(int d);

        void setMouvement(double m);

        void setVie(int v);

        Joueur getJoueur();

        int defautPointsVie { get; }
    }
}