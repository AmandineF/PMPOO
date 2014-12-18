using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class CaseMer : CaseImpl
    {
        /// <summary>
        /// Construit une case de type mer
        /// </summary>
        public CaseMer() : base() { }
        public CaseMer(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
