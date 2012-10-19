using Autofac;
using Caliburn.Micro.Autofac;

namespace Flair
{
    public class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();

            EnforceNamespaceConvention = false;
            AutoSubscribeEventAggegatorHandlers = true;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();
        }
    }
}
