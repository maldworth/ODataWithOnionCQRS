using ODataWithOnionCQRS.Bootstrapper;
using ODataWithOnionCQRS.Core.Data;
using ODataWithOnionCQRS.Core.Logging;
using ODataWithOnionCQRS.Data;
using ODataWithOnionCQRS.Infrastructure.Logging;
using ODataWithOnionCQRS.Services;
using Autofac;
using Autofac.Integration.WebApi;
using Mehdime.Entity;
using System.Web.Compilation;
using System.Linq;
using ODataWithOnionCQRS.MyODataApi;
using System.Web.Http;
using System.Data.Entity;
using FluentValidation;
using System.Reflection;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IocConfig), "RegisterDependencies")]

namespace ODataWithOnionCQRS.Bootstrapper
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            DbContextScopeExtensionConfig.Setup();

            var builder = new ContainerBuilder();

            // Get HttpConfiguration
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            builder.RegisterType<SchoolDbContext>().As<ISchoolDbContext>().InstancePerRequest();
            
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().SingleInstance();
            
            builder.Register(b => NLogLogger.Instance).SingleInstance();
            
            // Registers our IMediator (abstraction for observer pattern, which lets us use CQRS)
            builder.RegisterModule(new MediatorModule(Assembly.Load("ODataWithOnionCQRS.Services")));

            // Registers our Fluent Validations that we use on our Models
            builder.RegisterModule(new FluentValidationModule(Assembly.Load("ODataWithOnionCQRS.MyODataApi")));

            // Registers our AutoMapper Profiles
            builder.RegisterModule(new AutoMapperModule(Assembly.Load("ODataWithOnionCQRS.MyODataApi"), Assembly.Load("ODataWithOnionCQRS.Services")));

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
