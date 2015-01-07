using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class CasePlaine : CaseImpl
    {
        /// <summary>
        /// Construit une case de type plaine
        /// </summary>
       public CasePlaine():base(){}
         public CasePlaine(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
