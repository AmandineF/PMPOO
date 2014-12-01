using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelisationProjet
{
    public abstract class UniteImpl : Unite
    {
        private int ptAttaque;
        private int ptDefense;
        private int ptVie;
        private int ptMouvement;

        public int getAttaque()
        {
            throw new System.NotImplementedException();
        }

        public int getDefense()
        {
            throw new System.NotImplementedException();
        }

        public int getMouvement()
        {
            throw new System.NotImplementedException();
        }

        public int getVie()
        {
            throw new System.NotImplementedException();
        }

        public void setAttaque()
        {
            throw new System.NotImplementedException();
        }

        public void setDefense()
        {
            throw new System.NotImplementedException();
        }

        public void setMouvement()
        {
            throw new System.NotImplementedException();
        }

        public void setVie()
        {
            throw new System.NotImplementedException();
        }

        public void attaquer()
        {
            throw new System.NotImplementedException();
        }

        public void deplacer()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface Unite
    {
        void attaquer();

        void deplacer();

        int getAttaque();

        int getDefense();

        int getMouvement();

        int getVie();

        void setAttaque();

        void setDefense();

        void setMouvement();

        void setVie();
    }
}
