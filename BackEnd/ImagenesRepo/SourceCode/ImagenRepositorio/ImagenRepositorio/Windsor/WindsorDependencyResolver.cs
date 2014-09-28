namespace ImagenRepositorio.Windsor
{
    using Castle.MicroKernel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http.Dependencies;
    using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

    public class WindsorDependencyResolver: IDependencyResolver
    {
        private readonly IKernel container;

        public WindsorDependencyResolver(IKernel container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(this.container);
        }

        public object GetService(Type serviceType)
        {
            return this.container.HasComponent(serviceType) ? this.container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}