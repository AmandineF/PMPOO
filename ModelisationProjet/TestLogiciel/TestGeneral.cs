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
            int taille = 0;
            string ps1 = "";
            string ps2 = "";
            Peuple p1 = null;
            Peuple p2 = null;
            CreateurPartie dieu;
            Console.WriteLine("Bienvenue sur SmallWorld\n");
            Console.WriteLine("Quel type de partie voulez-vous jouer ? 1- Demo, 2- Petite, 3- Normale\n");
            /*string choix = Console.ReadLine();
            if (choix == "1")
                taille = 4;
            else if (choix == "2")
                taille = 10;
            else if (choix == "3")
                taille = 16;*/
            taille = 4;
            dieu = new CreateurPartie(taille);
            Console.WriteLine("Joueur 1 -\n");
            Console.WriteLine("Quel est votre pseudo ? \n");
           // ps1 = Console.ReadLine();
            ps1 = "Amandine";
            Console.WriteLine("Quel peuple voulez vous ? 1- Nain, 2- Elf, 3- Orc\n");
           /* choix = Console.ReadLine();
            if (choix == "1")
                p1 = new PeupleNain();
            else if (choix == "2")
                p1 = new PeupleElf();
            else if (choix == "3")
                p1 = new PeupleOrc();
            */
            p1 = new PeupleOrc();

            Console.WriteLine("Joueur 2 -\n");
            Console.WriteLine("Quel est votre pseudo ? \n");
            //ps2 = Console.ReadLine();
            ps2 = "Frank";
            Console.WriteLine("Quel peuple voulez vous ? 1- Nain, 2- Elf, 3- Orc\n");
            /*choix = Console.ReadLine();
            if (choix == "1")
                p2 = new PeupleNain();
            else if (choix == "2")
                p2 = new PeupleElf();
            else if (choix == "3")
                p2 = new PeupleOrc();*/
            p2 = new PeupleNain();
            //En vrai il faudra enlever le choix de peuple que le joueur 1 a fait
            Jeu j = dieu.creerPartie(ps1, p1, ps2, p2);
            Tour t = null;
            Random premierJ = new Random();
            int pj = premierJ.Next(1, 3);

            while (j.getNbTours() > 0 && (j.finDuJeu() == false))
            {
                j.getCarte().Dessin();
                Console.WriteLine("\n");
                if (pj == 1)
                {
                   //t = new TourImpl(j, j.getJoueur1());
                    //t = new TourImpl(j, j.getJoueur2());
                }
                else
                {
                    //t = new TourImpl(j, j.getJoueur2());
                    //t = new TourImpl(j, j.getJoueur1());
                }

                j.decNbTours();
            }

            if (j.getGagnant() == p1)
            {
                Console.WriteLine("Félicitation au Joueur 1\n");
            }
            else if (j.getGagnant() == p2)
            {
                Console.WriteLine("Félicitation au Joueur 2\n");
            } 
            else
            {
                Console.WriteLine("Egalité\n");
            }
        }
    }
}
