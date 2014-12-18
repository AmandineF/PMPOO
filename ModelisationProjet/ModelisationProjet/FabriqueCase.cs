using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class FabriqueCaseImpl : FabriqueCase, ISerializable
    {

        private CaseMontagne caseMontagne;
        private CaseDesert caseDesert;
        private CasePlaine casePlaine;
        private CaseForet caseForet;
        private CaseMer caseMer;

        public FabriqueCaseImpl() { }

        public FabriqueCaseImpl(SerializationInfo info, StreamingContext context) {
            this.caseDesert = (CaseDesert)info.GetValue("caseDesert", typeof(CaseDesert));
            this.caseMontagne = (CaseMontagne)info.GetValue("caseMontagne", typeof(CaseMontagne));
            this.casePlaine = (CasePlaine)info.GetValue("casePlaine", typeof(CasePlaine));            
            this.caseForet = (CaseForet)info.GetValue("caseForet", typeof(CaseForet));
            this.caseMer = (CaseMer)info.GetValue("caseMer", typeof(CaseMer));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("caseDesert", this.caseDesert);
            info.AddValue("caseMontagne", this.caseMontagne);
            info.AddValue("casePlaine", this.casePlaine);
            info.AddValue("caseForet", this.caseForet);
            info.AddValue("caseMer", this.caseMer);
        }
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
        public CaseMer creerMer()
        {
            this.caseMer = new CaseMer();
            return caseMer;
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
                case 5:
                    return this.creerMer();
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

        CaseMer creerMer();

        CaseImpl faireCase(int type);
    }
}
