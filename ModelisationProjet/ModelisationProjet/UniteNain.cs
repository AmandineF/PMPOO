using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class UniteNain : UniteImpl
    {
        /// <summary>
        /// Construit une unité de type nain
        /// </summary>
        /// <param name="j">Joueur qui possède l'unité</param>
           public UniteNain(Joueur j):base(j){}
        public UniteNain(SerializationInfo info, StreamingContext context): base(info, context) {

        }

    }
}
