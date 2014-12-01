using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SmallWorld;
using ModelisationProjet;

namespace SmallWorld
{
    class FabriqueImage
    {
        private BitmapImage caseDesert = null;
        private BitmapImage caseForet = null;
        private BitmapImage caseMontagne = null;
        private BitmapImage casePlaine = null;
        private BitmapImage vide = null;
        private BitmapImage nain = null;
        private BitmapImage nain2 = null;
        private BitmapImage elf = null;
        private BitmapImage orc = null;
        private static FabriqueImage INSTANCE = new FabriqueImage();

        private FabriqueImage()
        {
            this.caseDesert = new BitmapImage(new Uri(@"Ressources/Cases/Map2/caseDesert.png", UriKind.Relative));
            this.caseForet = new BitmapImage(new Uri(@"Ressources/Cases/Map2/caseForet.png", UriKind.Relative));
            this.caseMontagne = new BitmapImage(new Uri(@"Ressources/Cases/Map2/caseMontagne.png", UriKind.Relative));
            this.casePlaine = new BitmapImage(new Uri(@"Ressources/Cases/Map2/casePlaine.png", UriKind.Relative));
            
            this.nain = new BitmapImage(new Uri(@"Ressources/Unites/nainN1.png", UriKind.Relative));
            this.nain2 = new BitmapImage(new Uri(@"Ressources/Unites/nain2.png", UriKind.Relative));
            this.elf = new BitmapImage(new Uri(@"Ressources/Unites/elf.png", UriKind.Relative));
            this.orc = new BitmapImage(new Uri(@"Ressources/Unites/orcN1.png", UriKind.Relative));

            this.vide = new BitmapImage(new Uri(@"Ressources/Cases/vide.png", UriKind.Relative));
        }

        public static FabriqueImage getInstance()
        {
            return INSTANCE;
        }

        public Brush getVide()
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = this.vide;
            return brush;
        }
        public Brush getBrushCase(Case c)
        {
            ImageBrush brush = new ImageBrush();
            if (c is CaseDesert)
            {
                brush.ImageSource = this.caseDesert;
            }
            else if (c is CaseForet)
            {
                brush.ImageSource = this.caseForet;
            }
            else if (c is CaseMontagne)
            {
                brush.ImageSource = this.caseMontagne;
            }
            else if (c is CasePlaine)
            {
                brush.ImageSource = this.casePlaine;
            }
            return brush;
        }

        public Brush getBrushUnite(Unite u, int nbUnite)
        {
            ImageBrush brush = new ImageBrush();
            if (u is UniteNain)
            {
                if (nbUnite > 2)
                {
                    brush.ImageSource = this.nain2;
                }
                else 
                {
                    brush.ImageSource = this.nain;
                } 
                
            }
            else if (u is UniteElf)
            {
                if (nbUnite > 2)
                {
                    brush.ImageSource = this.nain2;
                }
                else
                {
                    brush.ImageSource = this.nain;
                } 
            }
            else if (u is UniteOrc)
            {
                if (nbUnite > 2)
                {
                    brush.ImageSource = this.orc;
                }
                else
                {
                    brush.ImageSource = this.orc;
                } 
            }
            return brush;
        }

    }
}