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
        
        /// <summary>
        /// Création d'une case de type montagne
        /// </summary>
        /// <returns>Une nouvelle case montagne</returns>
        public CaseMontagne creerMontagne() 
        {
            this.caseMontagne = new CaseMontagne();
            return caseMontagne;
        }

        /// <summary>
        /// Création d'une case plaine
        /// </summary>
        /// <returns>Une nouvelle case plaine</returns>
        public CasePlaine creerPlaine()
        {
            this.casePlaine = new CasePlaine();
            return casePlaine;
        }

        /// <summary>
        /// Création d'une case désert
        /// </summary>
        /// <returns>Une nouvelle case désert</returns>
        public CaseDesert creerDesert()
        {
            this.caseDesert = new CaseDesert();
            return caseDesert;
        }

        /// <summary>
        /// Création d'une case forêt
        /// </summary>
        /// <returns>Une nouvelle case forêt</returns>
        public CaseForet creerForet()
        {
            this.caseForet = new CaseForet();
            return caseForet;
        }
        public CaseImpl faireCase(int type)
        {
            switch (type)
            {
                case 1:
                    return this.creerDesert();
                case 2:
                    return this.creerForet();
                case 3:
                    return this.creerMontagne();
                case 4:
                    return this.creerPlaine();
                default:
                    return this.creerForet();
            }
        }
    }
    public interface FabriqueCase
    {
        
        CaseForet creerForet();

        CaseMontagne creerMontagne();

        CaseDesert creerDesert();

        CasePlaine creerPlaine();
        CaseImpl faireCase(int type);
    }
}
