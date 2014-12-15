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
        private BitmapImage nain = null;
        private BitmapImage elf = null;
        private BitmapImage orc = null;
        private BitmapImage nainInfos = null;
        private BitmapImage elfInfos = null;
        private BitmapImage orcInfos = null;
        private BitmapImage selectJ1 = null;
        private BitmapImage selectJ2 = null;
        private BitmapImage selectJ3 = null;
        private BitmapImage suggere = null;

        private static FabriqueImage INSTANCE = new FabriqueImage();

        private FabriqueImage()
        {
            this.caseDesert = new BitmapImage(new Uri(@"Ressources/Cases/Map3/caseDesert.png", UriKind.Relative));
            this.caseForet = new BitmapImage(new Uri(@"Ressources/Cases/Map3/caseForet.png", UriKind.Relative));
            this.caseMontagne = new BitmapImage(new Uri(@"Ressources/Cases/Map3/caseMontagne.png", UriKind.Relative));
            this.casePlaine = new BitmapImage(new Uri(@"Ressources/Cases/Map3/casePlaine.png", UriKind.Relative));

            
            this.nain = new BitmapImage(new Uri(@"Ressources/Unites/nain.png", UriKind.Relative));
            this.elf = new BitmapImage(new Uri(@"Ressources/Unites/elf.png", UriKind.Relative));
            this.orc = new BitmapImage(new Uri(@"Ressources/Unites/orc.png", UriKind.Relative));
            this.nainInfos = new BitmapImage(new Uri(@"Ressources/Unites/nainInfos.png", UriKind.Relative));
            this.elfInfos = new BitmapImage(new Uri(@"Ressources/Unites/elfInfos.png", UriKind.Relative));
            this.orcInfos = new BitmapImage(new Uri(@"Ressources/Unites/orcInfos.png", UriKind.Relative));

            this.selectJ1 = new BitmapImage(new Uri(@"Ressources/Cases/Map3/selectionJ1.png", UriKind.Relative));
            this.selectJ2 = new BitmapImage(new Uri(@"Ressources/Cases/Map3/selectionJ2.png", UriKind.Relative));
            this.selectJ3 = new BitmapImage(new Uri(@"Ressources/Cases/Map3/selectionJ3.png", UriKind.Relative));
            this.suggere = new BitmapImage(new Uri(@"Ressources/Cases/Map3/selection.png", UriKind.Relative));

        }

        public static FabriqueImage getInstance()
        {
            return INSTANCE;
        }

        public Brush getSuggere()
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = this.suggere;
            return brush;
        }
        public Brush getSelection(bool b)
        {
            ImageBrush brush = new ImageBrush();
            if (b)
            {
                brush.ImageSource = this.selectJ1;
            }
            else
            {
                brush.ImageSource = this.selectJ2;
            }

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

        public Brush getBrushUnite(Unite u)
        {
            ImageBrush brush = new ImageBrush();
            if (u is UniteNain)
            {
                    brush.ImageSource = this.nain;   
            }
            else if (u is UniteElf)
            {
                    brush.ImageSource = this.elf;

            }
            else if (u is UniteOrc)
            {
                    brush.ImageSource = this.orc;
            }
            return brush;
        }
        public Brush getBrushUniteInfos(Unite u)
        {
            ImageBrush brush = new ImageBrush();
            if (u is UniteNain)
            {
                brush.ImageSource = this.nainInfos;
            }
            else if (u is UniteElf)
            {
                brush.ImageSource = this.elfInfos;

            }
            else if (u is UniteOrc)
            {
                brush.ImageSource = this.orcInfos;
            }
            return brush;
        }

    }
}