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
        private BitmapImage nain4 = null;
        private BitmapImage elf = null;
        private BitmapImage orc = null;
        private BitmapImage unit1 = null;
        private BitmapImage unit2 = null;
        private BitmapImage unit3 = null;
        private BitmapImage unit4 = null;

        private static FabriqueImage INSTANCE = new FabriqueImage();

        private FabriqueImage()
        {
            this.caseDesert = new BitmapImage(new Uri(@"Ressources/Cases/Map2/caseDesert.png", UriKind.Relative));
            this.caseForet = new BitmapImage(new Uri(@"Ressources/Cases/Map2/caseForet.png", UriKind.Relative));
            this.caseMontagne = new BitmapImage(new Uri(@"Ressources/Cases/Map2/caseMontagne.png", UriKind.Relative));
            this.casePlaine = new BitmapImage(new Uri(@"Ressources/Cases/Map2/casePlaine.png", UriKind.Relative));

            this.unit1 = new BitmapImage(new Uri(@"Ressources/un.png", UriKind.Relative));
            this.unit2 = new BitmapImage(new Uri(@"Ressources/deux.png", UriKind.Relative));
            this.unit3 = new BitmapImage(new Uri(@"Ressources/trois.png", UriKind.Relative));
            this.unit4 = new BitmapImage(new Uri(@"Ressources/quatre.png", UriKind.Relative));
            
            this.nain = new BitmapImage(new Uri(@"Ressources/Unites/nainN1.png", UriKind.Relative));
            this.nain4 = new BitmapImage(new Uri(@"Ressources/Unites/nain4.png", UriKind.Relative));
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
           
                if (nbUnite == 1)
                {
                    brush.ImageSource = this.unit1;
                }
                else if (nbUnite == 2)
                {
                    brush.ImageSource = this.unit2;
                }
                else if (nbUnite == 3)
                {
                    brush.ImageSource = this.unit3;
                }
                else if (nbUnite == 4)
                {
                    brush.ImageSource = this.unit4;
                }

            return brush;
        }
        public Brush getBrushUnite2(Unite u, int nbUnite)
        {
            ImageBrush brush = new ImageBrush();
            if (u is UniteNain)
            {
                if (nbUnite > 2)
                {
                    brush.ImageSource = this.nain4;
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
                    brush.ImageSource = this.nain4;
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