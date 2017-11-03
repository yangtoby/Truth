using Autofac;

namespace MM.Web.CommonHelper
{
    public class ApplicationContainer
    {
        public static ILifetimeScope Container
        {
            get
            {
                return Autofac.Integration.Mvc.AutofacDependencyResolver.Current.ApplicationContainer;
            }
        }
    }
}
