using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class CaseDesert : CaseImpl
    {
        /// <summary>
        /// Construit une case de type désert
        /// </summary>
         public CaseDesert():base(){}
        public CaseDesert(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
