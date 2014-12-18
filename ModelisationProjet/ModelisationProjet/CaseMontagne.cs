using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class CaseMontagne : CaseImpl
    {
        /// <summary>
        /// Construit une case de type montagne
        /// </summary>
        public CaseMontagne():base(){}
         public CaseMontagne(SerializationInfo info, StreamingContext context): base(info, context) {

        }
    }
}
