using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable]
    public class CarteNormale : CarteImpl
    {
        /// <summary>
        /// Construit une carte 15 x 15
        /// </summary>
        public CarteNormale():base(15){}
        public CarteNormale(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
