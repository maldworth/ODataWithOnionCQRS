using ODataWithOnionCQRS.Core.Query;
using ODataWithOnionCQRS.Services.Query;
using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System;
using System.Collections.Generic;

namespace ODataWithOnionCQRS.Bootstrapper
{
    public class MediatorModule : Module
    {
        private readonly System.Reflection.Assembly[] _assembliesToScan;

        public MediatorModule(params System.Reflection.Assembly[] assembliesToScan)
            : base()
        {
            _assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();

            // Request/Response for Query
            builder.RegisterAssemblyTypes(_assembliesToScan)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            // Request/void Response for Commands
            builder.RegisterAssemblyTypes(_assembliesToScan)
                .AsClosedTypesOf(typeof(RequestHandler<>))
                .AsImplementedInterfaces();

            // Special registration of our generic Automapper Handler
            builder.RegisterGeneric(typeof(AutoMapperQuery<,>)).AsSelf();
            builder.RegisterGeneric(typeof(AutoMapperQueryHandler<,>))
                .As(typeof(IRequestHandler<,>));

            // Special Registration of our generic Query Handler
            builder.RegisterGeneric(typeof(GenericQuery<>)).AsSelf();
            builder.RegisterGeneric(typeof(GenericQueryHandler<>))
                .As(typeof(IRequestHandler<,>));

            // Sets the delegate resolver factories for Mediatr.
            // These factories are used by Mediatr to find the appropriate Handlers
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
        }
    }
}
