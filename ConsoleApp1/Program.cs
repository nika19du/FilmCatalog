using FilmCatalog;
using FilmCatalog.View;
using System;
using System.Windows.Forms;

namespace FilmCatalog
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Run(new Form2());
            
            //Display display = new Display(new Data.FilmCatalogContext());
        }
    }
}
