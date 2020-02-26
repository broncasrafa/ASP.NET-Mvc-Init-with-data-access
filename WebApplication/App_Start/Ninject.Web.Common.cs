[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApplication.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebApplication.App_Start.NinjectWebCommon), "Stop")]

namespace WebApplication.App_Start
{
    using System;
    using System.Web;
    using Application.AppServices;
    using Application.IAppServices;
    using DataAccess.Providers.ADO.Repository;
    using DataAccess.Repositories;
    using Domain.Interfaces.IRepositories;
    using Domain.Interfaces.IServices;
    using Domain.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Service Application
            kernel.Bind<IAppServiceUsuario>().To<AppServiceUsuario>();


            // Services do Domain
            kernel.Bind<IServiceUsuario>().To<ServiceUsuario>();



            // Repositories
            kernel.Bind<IBaseRepository>().To<BaseRepository>();
            kernel.Bind<IRepositoryUsuario>().To<RepositoryUsuario>();
        }        
    }
}