using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public abstract class UniteImpl : Unite, ISerializable
    {
        private const int POINTS_VIE_INITIAL = 5;
        private const int POINTS_ATTAQUE_INITIAL = 2;
        private const int POINTS_DEFENSE_INITIAL = 1;
        private const int POINTS_MVT_INITIAL = 1;
        private int ptVie;
        private int ptVictoire;
        private int ptAttaque;
        private int ptDefense;
        private double ptMouvement;
        private Joueur proprietaire;

        /// <summary>
        /// Construit une unité 
        /// </summary>
        /// <param name="j">Joueur qui possède l'unité</param>
        public UniteImpl(Joueur j)
        {
            this.ptAttaque = POINTS_ATTAQUE_INITIAL;
            this.ptDefense = POINTS_DEFENSE_INITIAL;
            this.ptVie = POINTS_VIE_INITIAL;
            this.ptMouvement = POINTS_MVT_INITIAL;
            this.ptVictoire = 0;
            this.proprietaire = j;
        }

         public UniteImpl(SerializationInfo info, StreamingContext context) {
            this.ptVie = (int)info.GetValue("ptVie", typeof(int));
            this.ptVictoire = (int)info.GetValue("ptVictoire", typeof(int));
            this.ptAttaque = (int)info.GetValue("ptAttaque", typeof(int));
            this.ptDefense = (int)info.GetValue("ptDefense", typeof(int));
            this.ptMouvement = (double)info.GetValue("ptDefense", typeof(double));
            this.proprietaire = (Joueur)info.GetValue("proprio", typeof(Joueur));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("ptVie", this.ptVie);
            info.AddValue("ptVictoire", this.ptVictoire);
            info.AddValue("ptAttaque", this.ptAttaque);
            info.AddValue("ptDefense", this.ptDefense);
            info.AddValue("ptMouvement", this.ptMouvement);
            info.AddValue("proprio", this.proprietaire);
        }

        /// <summary>
        /// Retourne le nombre de points de vie initial
        /// </summary>
        public int defautPointsVie
        {
            get { return POINTS_VIE_INITIAL; }
        }

        /// <summary>
        /// Retourne le nombre de points de mouvement initial
        /// </summary>
        public int defautPointsMvt
        {
            get { return POINTS_MVT_INITIAL; }
        }

        /// <summary>
        /// Retourne le nombre de points d'attaque initial
        /// </summary>
        public int defautPointsAttaque
        {
            get { return POINTS_ATTAQUE_INITIAL; }
        }

        /// <summary>
        /// Retourne le nombre de points de défense initial
        /// </summary>
        public int defautPointsDefense
        {
            get { return POINTS_DEFENSE_INITIAL; }
        }

        /// <summary>
        /// Retourne le nombre de points de victoire
        /// </summary>
        /// <returns>Le nombre de points de victoire</returns>
        public int getPtVictoire()
        {
            return this.ptVictoire;
        }

        /// <summary>
        /// Donne le nombre de point de victoire
        /// </summary>
        /// <param name="v">Le nouveau nombre de points de victoire</param>
        public void setPtVictoire(int v)
        {
            this.ptVictoire = v;
        }

        /// <summary>
        /// Méthode pour vérifier si une unité est vivante ou non
        /// </summary>
        /// <returns>Vrai si elle est vivante, faux sinon</returns>
        public bool estVivante()
        {
            return (this.ptVie > 0);
        }

        /// <summary>
        /// Donne le nombre de points d'attaque de l'unité
        /// </summary>
        /// <returns>Les points d'attaque</returns>
        public int getAttaque()
        {
           return this.ptAttaque;
        }

        /// <summary>
        /// Donne le nombre de points de défense de l'unité
        /// </summary>
        /// <returns>Les points de défense</returns>

        public int getDefense()
        {
            return this.ptDefense;
        }

        /// <summary>
        /// Donne le nombre de points de mouvement de l'unité
        /// </summary>
        /// <returns>Les points de mouvement</returns>
        public double getMouvement()
        {
            return this.ptMouvement;
        }
        /// <summary>
        /// Donne le nombre de points de vie de l'unité
        /// </summary>
        /// <returns>Les points de vie</returns>
        public int getVie()
        {
            return this.ptVie;
        }

        /// <summary>
        /// Enregistre le nouveau nombre de points d'attaque de l'unité
        /// </summary>
        /// <param name="a">Les points d'attaque</param>
        public void setAttaque(int a)
        {
           this.ptAttaque = a;
           if (this.ptAttaque < 0)
               this.ptAttaque = 0;
        }

        /// <summary>
        /// Enregistre le nouveau nombre de points de défense de l'unité
        /// </summary>
        /// <param name="d">Les points de défense</param>
        public void setDefense(int d)
        {
            this.ptDefense = d;
            if (this.ptDefense < 0)
                this.ptDefense = 0;
        }


        /// <summary>
        /// Enregistre le nouveau nombre de points de mouvement de l'unité
        /// </summary>
        /// <param name="m">Les points de mouvement</param>
        public void setMouvement(double m)
        {
            this.ptMouvement = m;
            if (this.ptMouvement < 0)
                this.ptMouvement = 0;
        }

        /// <summary>
        /// Enregistre le nouveau nombre de points de vie de l'unité
        /// </summary>
        /// <param name="v">Les points de vie</param>
        public void setVie(int v)
        {
            this.ptVie = v;
        }
    
        /// <summary>
        /// Retourne le joueur à qui l'unité appartient
        /// </summary>
        /// <returns>Joueur propriétaire de l'unité</returns>
        public Joueur getJoueur()
        {
            return this.proprietaire;
        }

    }
}
