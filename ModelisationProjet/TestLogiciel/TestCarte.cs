using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelisationProjet;

namespace TestLogiciel
{
    [TestClass]
    public class TestCarte
    {
        [TestMethod]
        public void TestGenerationCarte()
        {
           
            CarteImpl carte = new CarteDemo();

            PeupleElf p1 = new PeupleElf();
            JoueurImpl j1 = new JoueurImpl(p1,carte.getTaille(), "Amandine");

            PeupleNain p2 = new PeupleNain();
            JoueurImpl j2 = new JoueurImpl(p2, carte.getTaille(), "Frank");

            for(int i = 0; i < carte.getTaille(); i++) {
                for(int j = 0; j < carte.getTaille(); j++) {
                    Case tuile = carte.getCase(i, j);
                    tuile.getUnite();
                    bool estJ2 = tuile.estCaseEnnemie(j1);
                    bool estJ1 = tuile.estCase(j1);
                    if (estJ1 && estJ2)
                    {
                        Console.WriteLine("Problème");
                    }
                    Unite u = new UniteElf(j1);
                    tuile.ajoutUnite(u);
                    tuile.supprimeUnite(u);
                }
            }

            
           carte.nbCasesColonisees(j1);
            
               
        }
    }
}
