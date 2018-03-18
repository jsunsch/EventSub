using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using EventSub.DependencyInjection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EventSub
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureWindsor(GlobalConfiguration.Configuration);
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();

            // Use Castle Windsor Installers to register interfaces and their implementations.
            _container.Install(FromAssembly.This());

            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));

            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;

            // Instead of using Castle Windsor Installers, it is also to register interfaces and their implementations
            // directly here.
            _container.Register(
                Component.For<Repositories.IEventSubscriptionRepository>().ImplementedBy<Repositories.EventSubscriptionRepository>().LifestylePerWebRequest(),
                Component.For<Repositories.ILiveEventRepository>().ImplementedBy<Repositories.LiveEventRepository>().LifestylePerWebRequest()
            );

            // Test repositories.
            //_container.Register(
            //    Component.For<Repositories.IEventSubscriptionRepository>().ImplementedBy<Repositories.EventSubscriptionTestRepository>().LifestylePerWebRequest(),
            //    Component.For<Repositories.ILiveEventRepository>().ImplementedBy<Repositories.LiveEventTestRepository>().LifestylePerWebRequest()
            //);

            // Register Web API controllers.
            _container.Register(
                Classes.FromAssembly(typeof(Controllers.EventsController).Assembly).BasedOn<ApiController>().LifestylePerWebRequest(),
                Classes.FromAssembly(typeof(Controllers.EventSubscriptionController).Assembly).BasedOn<ApiController>().LifestylePerWebRequest()
            );
        }
    }
}
