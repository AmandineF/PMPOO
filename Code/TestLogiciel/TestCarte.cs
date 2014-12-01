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
            int taille = 4;
            CarteImpl c = new CarteImpl(taille);

            PeupleElf p1 = new PeupleElf();
            JoueurImpl j1 = new JoueurImpl(p1, taille, "Amandine");

            PeupleNain p2 = new PeupleNain();
            JoueurImpl j2 = new JoueurImpl(p2, taille, "Frank");


            Random r = new Random();
            int rand = 0;
            int x1 = 0;
            int x2 = 0;
            int y1 = 0;
            int y2 = 0;

            rand = r.Next(1, 3);
            if (rand == 1)
            {
                x1 = r.Next(0, taille);
                x2 = x1;
                y1 = r.Next(1, 3);
                if (y1 == 1)
                {
                    y1 = 0;
                    y2 = taille - 1;
                }
                else
                {
                    y1 = taille - 1;
                    y2 = 0;
                }
            }
            else
            {
                y1 = r.Next(0, taille);
                y2 = y1;
                x1 = r.Next(1, 3);
                if (x1 == 1)
                {
                    x1 = 0;
                    x2 = taille - 1;
                }
                else
                {
                    x1 = taille - 1;
                    x2 = 0;
                }

            }
            int i;
            for (i = 0; i < j1.getNbUnite(); i++)
            {
                Unite u = j1.getUnite(i);
                c.getCase(x1, y1).ajoutUnite(u);
                Unite v = j2.getUnite(i);
                c.getCase(x2, y2).ajoutUnite(v);
            }
               
            c.Dessin();
        }
    }
}
