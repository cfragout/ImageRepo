using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ImagenRepoEntities.Entities;
using ImagenRepoEntities.Models;
using ImagenRepoRepository.IRepository;
using ImagenRepoRepository.Repository;
using ImagenRepoServices.IServices;
using ImagenRepoServices.Services;
using ImagenRepositorio.Automapper;
using ImagenRepositorio.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ImagenRepositorio
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly WindsorContainer container;

        public WebApiApplication()
        {
            this.container = new WindsorContainer();
        }

        protected void Application_Start()
        {
          
            AreaRegistration.RegisterAllAreas();

            this.RegisterDependencyResolver();
            this.InstallDependencies();
            this.RegisterServices();
            this.RegisterRepository();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutomapperDomainRegistration.Register();

            System.Data.Entity.Database.SetInitializer(new ModelInitializer());
        }

        private void RegisterRepository()
        {
            container.Register(Component.For<IGenericRepository<Imagen>>().ImplementedBy<GenericRepository<Imagen>>().LifeStyle.PerWebRequest);
            container.Register(Component.For<IGenericRepository<Tag>>().ImplementedBy<GenericRepository<Tag>>().LifeStyle.PerWebRequest);

        }

        private void RegisterServices()
        {
            container.Register(Component.For<ModelContainer>().LifestylePerWebRequest());
            container.Register(Component.For<IImagenService>().ImplementedBy<ImagenService>().LifeStyle.PerWebRequest);
            container.Register(Component.For<ITagService>().ImplementedBy<TagService>().LifeStyle.PerWebRequest);

        }

        private void InstallDependencies()
        {
            this.container.Install(FromAssembly.This());
        }

        private void RegisterDependencyResolver()
        {
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(this.container.Kernel);
        }


    }
}
