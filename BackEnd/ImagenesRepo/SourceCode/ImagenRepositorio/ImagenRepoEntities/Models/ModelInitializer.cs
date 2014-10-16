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
                Name = "auto 0",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto 0.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "http://www.hdwallpaperscool.com/wp-content/uploads/2013/11/gallardo-sports-cars-hd-images-widescreen-top-wallpapers.jpg",
                UserUploaded = false
            };
            var imagen2 = new Imagen()
            {
                Name = "auto 1",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto 1.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "auto 1",
                UserUploaded = true
            };
            var imagen3 = new Imagen()
            {
                Name = "auto 2",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto 2.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "auto 2",
                UserUploaded = true
            };
            var imagen4 = new Imagen()
            {
                Name = "auto_3",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto 3.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "auto 3",
                UserUploaded = true
            };
            var imagen5 = new Imagen()
            {
                Name = "auto 4",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto 4.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "auto 4",
                UserUploaded = true
            };
            var imagen6 = new Imagen()
            {
                Name = "auto 5",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto 5.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "auto 5",
                UserUploaded = true
            };
            var imagen7 = new Imagen()
            {
                Name = "autos",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/autos.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "autos",
                UserUploaded = true
            };
            var imagen8 = new Imagen()
            {
                Name = "casa 1",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/casa 1.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "casa 1",
                UserUploaded = true
            };
            var imagen9 = new Imagen()
            {
                Name = "casa 2",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/casa 2.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "casa 2",
                UserUploaded = true
            };
            var imagen10 = new Imagen()
            {
                Name = "casa 3",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/casa 3.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "casa 3",
                UserUploaded = true
            };
            var imagen11 = new Imagen()
            {
                Name = "casa 4",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/casa 4.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "casa 4",
                UserUploaded = true
            };
            var imagen12 = new Imagen()
            {
                Name = "casa 5",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/casa 5.jpg",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "casa 5",
                UserUploaded = true
            };
            var imagen13 = new Imagen()
            {
                Name = "viejo",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto gif 1.gif",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "gif 1",
                UserUploaded = true
            };
            var imagen14 = new Imagen()
            {
                Name = "auto gif 2",
                Path = "http://localhost:55069/Content/users/test_user@hotmail.com/images/auto gif 2.gif",
                IsDeleted = false,
                Created = DateTime.Today,
                OriginalUrl = "gif 2",
                UserUploaded = true
            };

            var tag1 = new Tag() 
            {
                Name = "Autos"
            };
            var tag2 = new Tag()
            {
                Name = "NSFW"
            };
            var tag3 = new Tag()
            {
                Name = "Gracioso"
            };
            var tag4 = new Tag()
            {
                Name = "Gifs"
            };
            var tag5 = new Tag()
            {
                Name = "animadas"
            };
            var tag6 = new Tag()
            {
                Name = "internet"
            };
            var tag7 = new Tag()
            {
                Name = "wallpaper"
            };
            var tag8 = new Tag()
            {
                Name = "fotos"
            };
            var tag9 = new Tag()
            {
                Name = "vintage"
            };
            var tag10 = new Tag()
            {
                Name = "casa"
            };

            imagen1.Tags.Add(tag1);
            imagen2.Tags.Add(tag1);
            imagen3.Tags.Add(tag1);
            imagen4.Tags.Add(tag1);
            imagen5.Tags.Add(tag1);
            imagen6.Tags.Add(tag1);
            imagen7.Tags.Add(tag1);

            imagen2.Tags.Add(tag9);
            imagen3.Tags.Add(tag9);
            imagen4.Tags.Add(tag9);
            imagen5.Tags.Add(tag9);
            imagen6.Tags.Add(tag9);

            imagen13.Tags.Add(tag4);
            imagen14.Tags.Add(tag4);

            imagen13.Tags.Add(tag5);
            imagen14.Tags.Add(tag5);

            imagen8.Tags.Add(tag10);
            imagen9.Tags.Add(tag10);
            imagen10.Tags.Add(tag10);
            imagen11.Tags.Add(tag10);
            imagen12.Tags.Add(tag10);

            context.Imagenes.Add(imagen1);
            context.Imagenes.Add(imagen2);
            context.Imagenes.Add(imagen3);
            context.Imagenes.Add(imagen4);
            context.Imagenes.Add(imagen5);
            context.Imagenes.Add(imagen6);
            context.Imagenes.Add(imagen7);
            context.Imagenes.Add(imagen8);
            context.Imagenes.Add(imagen9);
            context.Imagenes.Add(imagen10);
            context.Imagenes.Add(imagen11);
            context.Imagenes.Add(imagen12);
            context.Imagenes.Add(imagen13);
            context.Imagenes.Add(imagen14);

            context.Tags.Add(tag1);
            context.Tags.Add(tag2);
            context.Tags.Add(tag3);
            context.Tags.Add(tag4);
            context.Tags.Add(tag5);
            context.Tags.Add(tag6);
            context.Tags.Add(tag7);
            context.Tags.Add(tag8);
            context.Tags.Add(tag9);
            context.Tags.Add(tag9);

            context.SaveChanges();



        }
    }
}