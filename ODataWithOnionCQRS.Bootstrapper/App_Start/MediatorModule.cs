using ODataWithOnionCQRS.Core.Query;
using ODataWithOnionCQRS.Services.Query;
using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System;
using System.Collections.Generic;
using ODataWithOnionCQRS.Bootstrapper.Extensions;
using ODataWithOnionCQRS.Services;
using ODataWithOnionCQRS.Core.Services;

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
                .AsClosedTypesOf(typeof(IRequestHandler<,>), "service-handlers")
                .SingleInstance();

            // Request/void Response for Commands
            builder.RegisterAssemblyTypes(_assembliesToScan)
                .AsClosedTypesOf(typeof(RequestHandler<>))
                .SingleInstance();

            // Register our PreRequestHandler
            builder.RegisterAssemblyTypes(_assembliesToScan)
                .AsClosedTypesOf(typeof(IPreRequestHandler<>))
                .SingleInstance();

            // Decorate All Services with our Pipeline
            builder.RegisterGenericDecorator(typeof(MediatorPipeline<,>), typeof(IRequestHandler<,>), fromKey: "service-handlers", toKey: "pipeline-handlers");

            // Decorate All Pipelines with our Validator
            builder.RegisterGenericDecorator(typeof(ValidatorHandler<,>), typeof(IRequestHandler<,>), fromKey: "pipeline-handlers");    // The outermost decorator should not have a toKey

            // Special registration of our Automapper Handler
            builder.RegisterGeneric(typeof(AutoMapperQuery<,>)).AsSelf();
            builder.RegisterGeneric(typeof(AutoMapperQueryHandler<,>))
                .Named("service-handlers", typeof(IRequestHandler<,>)) // Because these are missed in the scan above, we have to manually name them for decoration
                .SingleInstance();

            // Special Registration of our Generic Query Handler
            builder.RegisterGeneric(typeof(GenericQuery<>)).AsSelf();
            builder.RegisterGeneric(typeof(GenericQueryHandler<>))
                .Named("service-handlers", typeof(IRequestHandler<,>)) // Because these are missed in the scan above, we have to manually name them for decoration
                .SingleInstance();

            // Special Registration of our Pagination Query Handler
            builder.RegisterGeneric(typeof(PaginateQuery<>)).AsSelf();
            builder.RegisterGeneric(typeof(PaginateQueryHandler<>))
                .Named("service-handlers", typeof(IRequestHandler<,>)) // Because these are missed in the scan above, we have to manually name them for decoration
                .SingleInstance();

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
