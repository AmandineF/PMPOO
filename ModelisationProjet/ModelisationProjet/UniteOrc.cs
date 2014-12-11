using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class UniteOrc : UniteImpl
    {
        /// <summary>
        /// Construit une unité de type Orc
        /// </summary>
        /// <param name="j">Joueur qui possède l'unité</param>
         public UniteOrc(Joueur j):base(j){}
    }
}
