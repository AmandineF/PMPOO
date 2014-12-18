using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable]
    public class CarteDemo : CarteImpl
    {
        /// <summary>
        /// Construit une carte 5 x 5
        /// </summary>
        public CarteDemo():base(4){}
        public CarteDemo(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
