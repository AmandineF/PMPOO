using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class UnitePirate : UniteImpl
    {
        /// <summary>
        /// Construit une unité de type Pirate
        /// </summary>
        /// <param name="j">Joueur qui possède l'unité</param>
        public UnitePirate(Joueur j) : base(j) { }
        public UnitePirate(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
