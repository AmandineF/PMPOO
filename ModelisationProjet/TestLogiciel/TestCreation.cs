using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelisationProjet;

namespace TestLogiciel
{
    [TestClass]
    public class TestCreateur
    {
        [TestMethod]
        public void TestCreerPartie()
        {
            Carte c = new CarteDemo();
            CreateurPartie dieu = new CreateurPartie(c);
            Jeu j = dieu.creerPartie("Amandine", new PeupleElf(),"Frank", new PeupleOrc());
           
        }
    }
}
