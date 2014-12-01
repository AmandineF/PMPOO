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
            CreateurPartie dieu = new CreateurPartie(4);
            Jeu j = dieu.creerPartie("Amandine", new PeupleElf(),"Frank", new PeupleOrc());
            j.getCarte().Dessin();
       
            Console.WriteLine(" \n Joueur 1 : " + j.getJoueur1().getPseudo() + " - Joueur 2 : " + j.getJoueur2().getPseudo());
        }
    }
}
