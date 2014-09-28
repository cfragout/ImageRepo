using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImagenRepoEntities.Models
{
    public class ModelInitializer : DropCreateDatabaseAlways<ModelContainer>
    {
        protected override void Seed(ModelContainer context)
        {

            var imagen1 = new Imagen() 
            {
                Name = "lambourghini",
                Path = " http://localhost:55069/Content/Images/Lambourghini.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "http://www.hdwallpaperscool.com/wp-content/uploads/2013/11/gallardo-sports-cars-hd-images-widescreen-top-wallpapers.jpg",
                UserUploaded = false
            };

            var imagen2 = new Imagen
            {
                UserUploaded = true,
                Name = "audi",
                Path = " http://localhost:55069/Content/Images/audi.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "sldkjjautoloco.jpg",
            };

            var tag = new Tag() 
            {
                Nombre = "Autos"
            };

            imagen1.Tags.Add(tag);

            context.Imagenes.Add(imagen1);
            context.Imagenes.Add(imagen2);

            context.SaveChanges();



        }
    }
}