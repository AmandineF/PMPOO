using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class UniteElf : UniteImpl
    {
        /// <summary>
        /// Construit une unité de type elf
        /// </summary>
        /// <param name="j">Joueur qui possède l'unité</param>
        public UniteElf(Joueur j):base(j){}
        public UniteElf(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
