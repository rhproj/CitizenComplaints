using Autofac;
using Complaints_WPF.Services;
using Complaints_WPF.Services.Interfaces;
using Complaints_WPF.ViewModels;
using Complaints_WPF.Views;

namespace Complaints_WPF.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LoginWindow>().AsSelf();
            builder.RegisterType<ComplaintsViewModel>().AsSelf();

            builder.RegisterType<CategoryReadService>().As<ICategoryReadService>();
            
            builder.RegisterType<CategoryWriteService>().As<ICategoryWriteService>();
            
            builder.RegisterType<ChiefReadService>().As<IChiefReadService>();
            
            builder.RegisterType<ChiefWriteService>().As<IChiefWriteService>();
            
            builder.RegisterType<ComplaintReadService>().As<IComplaintReadService>();
            
            builder.RegisterType<ComplaintWriteService>().As<IComplaintWriteService>();
            
            builder.RegisterType<FilterComplaintService>().As<IFilterComplaintService>();
            
            builder.RegisterType<OZhReadService>().As<IOZhReadService>();
           
            builder.RegisterType<OZhWriteService>().As<IOZhWriteService>();
            
            builder.RegisterType<ReadCitizenService>().As<IReadCitizenService>();
            
            builder.RegisterType<ReadProsecutorService>().As<IReadProsecutorService>();
            
            builder.RegisterType<ReadResultService>().As<IReadResultService>();

            return builder.Build();
        }
    }
}
