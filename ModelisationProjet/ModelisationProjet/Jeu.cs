using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public class JeuImpl : Jeu
    {

        private int nbTours;
        private Joueur joueur2;
        private Joueur joueur1;
        private Carte carte;

        /// <summary>
        /// Constuit un nouveau jeu
        /// </summary>
        /// <param name="n">Le nombre de tours du jeu</param>
        /// <param name="j1">Le joueur 1 du jeu</param>
        /// <param name="j2">Le joueur 2 du jeu</param>
        /// <param name="c">La carte du jeu</param>
        public JeuImpl(int n, Joueur j1, Joueur j2, Carte c) 
        {
            this.nbTours = n;
            this.joueur1 = j1;
            this.joueur2 = j2;
            this.carte = c;
        }
        public JeuImpl(SerializationInfo info, StreamingContext context) {
            this.joueur1 = (Joueur)info.GetValue("Joueur1", typeof(Joueur));
            this.joueur2 = (Joueur)info.GetValue("Joueur2", typeof(Joueur));
            this.carte = (Carte)info.GetValue("Carte", typeof(Carte));
            this.nbTours = (int)info.GetValue("NbTours", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Player1", this.joueur1);
            info.AddValue("Player2", this.joueur2);
            info.AddValue("Carte", this.carte);
            info.AddValue("NbTours", this.nbTours);
        }
        public JeuImpl() {}

        /// <summary>
        /// Donne le nombre de tour restant du jeu
        /// </summary>
        /// <returns>Nombre de tours du jeu</returns>
        public int getNbTours()
        {
            return this.nbTours;
        }

        /// <summary>
        /// Donne la carte du jeu
        /// </summary>
        /// <returns>Carte du jeu</returns>
        public Carte getCarte()
        {
            return this.carte;
        }
      
        /// <summary>
        /// Décrémente le nombre de tours du jeu
        /// </summary>
        public void decNbTours()
        {
            this.nbTours--;
        }
        /// <summary>
        /// Donne le premier joueur
        /// </summary>
        /// <returns>Joueur 1</returns>
        public Joueur getJoueur1()
        {
            return this.joueur1;
        }

        /// <summary>
        /// Donne le second joueur
        /// </summary>
        /// <returns>Joueur 2</returns>
        public Joueur getJoueur2()
        {
           return this.joueur2;
        }

        public void reinitialisation()
        {
            for (int i = 0; i < this.joueur1.getNbUnite(); i++)
            {
                this.joueur1.getUnite(i).setMouvement(this.joueur1.getUnite(i).defautPointsMvt);
                this.joueur1.getUnite(i).setAttaque(this.joueur1.getUnite(i).defautPointsAttaque);
                this.joueur1.getUnite(i).setDefense(this.joueur1.getUnite(i).defautPointsDefense);
                this.joueur1.getUnite(i).setPtVictoire(0);
            }
            for (int i = 0; i < this.joueur2.getNbUnite(); i++)
            {
                this.joueur2.getUnite(i).setMouvement(this.joueur2.getUnite(i).defautPointsMvt);
                this.joueur2.getUnite(i).setAttaque(this.joueur2.getUnite(i).defautPointsAttaque);
                this.joueur2.getUnite(i).setDefense(this.joueur2.getUnite(i).defautPointsDefense);
                this.joueur2.getUnite(i).setPtVictoire(0);
            }
        }

        /// <summary>
        /// Choisit aléatoirement le premier joueur qui va commencer la partie
        /// </summary>
        /// <returns>Le joueur qui va jouer le premier</returns>
        public Joueur getPremierJoueur()
        {
            Random r = new Random();
            int rand = r.Next(1, 3);
            if (rand == 1)
                return this.joueur1;
            else
                return this.joueur2;
        }


        /// <summary>
        /// Donne le gagnant du jeu
        /// </summary>
        /// <returns>Le joueur gagnant</returns>
        public Joueur getGagnant()
        {
            if (this.joueur1.getNbUnite() == 0 && this.joueur2.getNbUnite() == 0)
            {
                if (this.joueur1.getPtVictoire() > this.joueur2.getPtVictoire())
                {
                    return this.joueur1;
                }
                else if (this.joueur2.getPtVictoire() > this.joueur1.getPtVictoire())
                {
                    return this.joueur2;
                }
            }
            else
            {
                if (this.joueur1.getNbUnite() == 0)
                {
                    return this.joueur2;
                }
                else if (this.joueur2.getNbUnite() == 0)
                {
                    return this.joueur1;
                }
            }
            return null;
        }

        /// <summary>
        /// Le jeu est terminé si un des joueurs n'a plus d'unité ou si le nombre de tours restant est égal à 0
        /// </summary>
        /// <returns>Vrai si le jeu est terminé, faux sinon</returns>
        public bool finDuJeu()
        {
            return (this.joueur1.getNbUnite() == 0 || this.joueur2.getNbUnite() == 0 || this.nbTours == 0);
        }
    }

    public interface Jeu
    {
        bool finDuJeu();

        Joueur getGagnant();

        Joueur getJoueur1();

        Joueur getJoueur2();

        Joueur getPremierJoueur();

        void reinitialisation();

        Carte getCarte();

        void decNbTours();

        int getNbTours();
    }
}
