using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public class FabriqueCaseImpl : FabriqueCase
    {

        private CaseMontagne caseMontagne;
        private CaseDesert caseDesert;
        private CasePlaine casePlaine;
        private CaseForet caseForet;


        public FabriqueCaseImpl() {}

        public CaseMontagne creerMontagne() 
        {
            this.caseMontagne = new CaseMontagne();
            return caseMontagne;
        }
        public CasePlaine creerPlaine()
        {
            this.casePlaine = new CasePlaine();
            return casePlaine;
        }
        public CaseDesert creerDesert()
        {
            this.caseDesert = new CaseDesert();
            return caseDesert;
        }
        public CaseForet creerForet()
        {
            this.caseForet = new CaseForet();
            return caseForet;
        }
        public CasePlaine getPlaine()
        {
            return this.casePlaine;
        }

        public CaseDesert getDesert()
        {
            return this.caseDesert;
        }

        public CaseMontagne getMontagne()
        {
            return this.caseMontagne;
        }

        public CaseForet getForet()
        {
            return this.caseForet;
        }
    }

    public interface FabriqueCase
    {
        CaseDesert getDesert();

        CaseForet getForet();

        CaseMontagne getMontagne();

        CasePlaine getPlaine();
        
        CaseForet creerForet();

        CaseMontagne creerMontagne();

        CaseDesert creerDesert();

        CasePlaine creerPlaine();
    }
}
