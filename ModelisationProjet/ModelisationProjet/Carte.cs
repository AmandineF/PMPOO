using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Wrapper;

using System.Runtime.Serialization;


namespace ModelisationProjet
{
    [Serializable()]
    public class CarteImpl : Carte, ISerializable
    {

        private int taille;
        private Case[,] carte;
        private FabriqueCaseImpl fabrique;

        /// <summary>
        /// Créer une carte de manière aléatoire
        /// </summary>
        /// <param name="t">La taille de la matrice qui représentera la carte</param>
        unsafe public CarteImpl(int t)
        {
            this.taille = t;
            this.carte = new CaseImpl[taille, taille];
            int i, j;
            this.fabrique = new FabriqueCaseImpl();
            WrapperAlgo wp = new WrapperAlgo();
            int** typeCase = wp.generationMap(taille);
            carte = new Case[taille, taille];

            for ( i = 0; i < taille; i++)
            {
                for ( j = 0; j < taille; j++)
                {
                    carte[i, j] = fabrique.faireCase(typeCase[i][j]);
                }
            }
         
        }

        public CarteImpl(SerializationInfo info, StreamingContext context) {
            this.taille = (int)info.GetValue("Taille", typeof(int));
            this.carte = (Case[,])info.GetValue("Carte", typeof(Case[,]));
            this.fabrique = (FabriqueCaseImpl)info.GetValue("Fabrique", typeof(FabriqueCaseImpl));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("Taille", this.taille);
            info.AddValue("Carte", this.carte);
            info.AddValue("Fabrique", this.fabrique);
        }

        /// <summary>
        /// Permet de retourner une case de la matrice carte en fonction de ses coordonnées
        /// </summary>
        /// <param name="x">Coordonnée x, abscisse</param>
        /// <param name="y">Coordonnée y, ordonnée</param>
        /// <returns></returns>
        public Case getCase(int x, int y)
        {
           // Console.Write("x :" + y + " et y : " + y);
            return this.carte[x, y];
        }

        /// <summary>
        /// Donne la taille de la carte
        /// </summary>
        /// <returns>Retourne la taille de la matrice carte</returns>
        public int getTaille()
        {
            return this.taille;
        }

        /// <summary>
        /// Donne le nombre de cases appartenant au joueur jo sur la carte
        /// </summary>
        /// <param name="jo">Le joueur dont on veut connaitre le nombre de cases</param>
        /// <returns>le nombre de cases</returns>
        public int nbCasesColonisees(Joueur jo)
        {
            int total = 0;
            for (int i = 0; i < this.carte.GetLength(0); i++)
            {
                for (int j = 0; j < this.carte.GetLength(1); j++)
                {
                    if (this.carte[i,j].estCase(jo))
                    {
                        total++;
                    }
                }
            }
            return total;
        }

    }

    public interface Carte
    {
        Case getCase(int x, int y);

        int nbCasesColonisees(Joueur j);

        int getTaille();
    }
}
