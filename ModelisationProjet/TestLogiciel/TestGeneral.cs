using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelisationProjet;

namespace TestLogiciel
{
    [TestClass]
    public class TestGeneral
    {
        [TestMethod]
        public void TestMain()
        {
            string ps1 = "";
            string ps2 = "";
            Peuple p1 = null;
            Peuple p2 = null;
            CreateurPartie dieu;

            Carte c = new CarteNormale();
            dieu = new CreateurPartie(c);
            ps1 = "Amandine";
            p1 = new PeupleOrc();
            ps2 = "Frank";
            p2 = new PeupleNain();
          
            Jeu j = dieu.creerPartie(ps1, p1, ps2, p2);
            Tour t = new TourImpl(j, j.getPremierJoueur());

        }
    }
}
