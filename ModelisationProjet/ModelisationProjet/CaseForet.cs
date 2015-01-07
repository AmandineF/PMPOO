using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.Serialization;
using System.Text;

namespace ModelisationProjet
{
    [Serializable()]
    public class CaseForet : CaseImpl
    {
        /// <summary>
        /// Construit une case du type forêt
        /// </summary>
        public CaseForet():base(){}
  public CaseForet(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
