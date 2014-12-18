using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace ModelisationProjet
{
    [Serializable()]
    public abstract class CaseImpl : Case, ISerializable
    {
        public List<Unite> listeUnite;

        /// <summary>
        /// Constructeur de la classe CaseImpl, initialise la liste d'unité de la case
        /// </summary>
        public CaseImpl()
        {
            this.listeUnite = new List<Unite>();
        }

        public CaseImpl(SerializationInfo info, StreamingContext context)
        {
            this.listeUnite = (List<Unite>)info.GetValue("listeUnite", typeof(List<Unite>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("listeUnite", this.listeUnite);
        }
        /// <summary>
        /// Permet de retourner les unités présentes sur la case
        /// </summary>
        /// <returns>La liste d'unité de la case</returns>
        public List<Unite> getUnite()
        {
            return this.listeUnite;
        }

        /// <summary>
        /// Permet d'ajouter une unité à la liste des unités de la case
        /// </summary>
        /// <param name="unite">L'unité à ajouter</param>
        public void ajoutUnite(Unite unite) 
        {
            this.listeUnite.Add(unite);
        }

        /// <summary>
        /// Permet de supprimer une unité à la liste des unités de la case
        /// </summary>
        /// <param name="unite">L'unité à supprimer</param>
        public void supprimeUnite(Unite unite)
        {
            this.listeUnite.Remove(unite);
        }

        /// <summary>
        /// Permet de savoir si la case appartient à l'ennemi du joueur j ou non
        /// </summary>
        /// <param name="j">Le joueur qui veut savoir si la case appartient à son ennemi </param>
        /// <returns>Faux si la case n'appartient pas à l'ennemi, vrai sinon</returns>
        public bool estCaseEnnemie(Joueur j) 
        {
            bool res = false;
            if (this.listeUnite.Count() > 0)
            {
                if (this.listeUnite[0].getJoueur() != j)
                    res = true;
            }
            return res;
        }

        /// <summary>
        /// Permet de savoir si la case appartient au joueur j ou non
        /// </summary>
        /// <param name="j">Le joueur qui veut savoir si la case lui appartient ou non</param>
        /// <returns>Faux si la case appartient au joueur, vrai sinon</returns>
        public bool estCase(Joueur j)
        {
            bool res = false;
            if (this.listeUnite.Count() > 0)
            {
                if (this.listeUnite[0].getJoueur() == j)
                    res = true;
            }
            return res;
        }


    }

    public interface Case
    {
        List<Unite> getUnite();

        bool estCaseEnnemie(Joueur j);

        bool estCase(Joueur j);

        void ajoutUnite(Unite unite);

        void supprimeUnite(Unite unite);
    }
}
