using Autofac;
using ComplaintsAdmin.Services;
using ComplaintsAdmin.ViewModels;
using ComplaintsAdmin.Views;

namespace ComplaintsAdmin.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LoginWindow>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();

            builder.RegisterType<AccessService>().As<IAccessService>();

            builder.RegisterType<LoginService>().As<ILoginService>();

            return builder.Build();
        }
    }
}
