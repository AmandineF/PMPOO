using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable]
    public class CartePetite : CarteImpl
    {
        /// <summary>
        /// Construit une carte 10 x 10
        /// </summary>
        public CartePetite():base(10){}
        public CartePetite(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
