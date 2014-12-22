using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelisationProjet;

namespace TestLogiciel
{
    [TestClass]
    public class TestTour
    {
        [TestMethod]
        public void TestTourJ1()
        {
            Joueur j = new JoueurImpl(new PeupleOrc(), 4, "Amandne");
            Unite u = new UniteOrc(j);
            u.setAttaque(1);
            u.setVie(3);

           // TourImpl t = new TourImpl(j);

    

        }
    }
}
