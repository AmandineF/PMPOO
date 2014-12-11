using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class UniteNain : UniteImpl
    {
        /// <summary>
        /// Construit une unité de type nain
        /// </summary>
        /// <param name="j">Joueur qui possède l'unité</param>
           public UniteNain(Joueur j):base(j){}
    }
}
