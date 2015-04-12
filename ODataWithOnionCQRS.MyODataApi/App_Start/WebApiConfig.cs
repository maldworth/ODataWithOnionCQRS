using ODataWithOnionCQRS.Core.DomainModels;
using ODataWithOnionCQRS.Core.Dto;
using FluentValidation.WebApi;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using ODataWithOnionCQRS.MyODataApi.ViewModels;

namespace ODataWithOnionCQRS.MyODataApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var validatorFactory = new ODataWithOnionCQRS.MyODataApi.App_Start.FluentValidatorFactory();
            FluentValidationModelValidatorProvider.Configure(config, provider => provider.ValidatorFactory = validatorFactory);
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: GetEdmModel());
        }

        // Examples https://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/OData/v4/
        private static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.Namespace = "ContosoUniversity";

            var studentsEntitySet = modelBuilder.EntitySet<Student>("Students");

            // OData Actions
            modelBuilder.EntityType<Student>()
                .Action("DropAllCourses");

            modelBuilder.EntityType<Student>()
                .Function("BestMark")
                .ReturnsFromEntitySet<Enrollment>("Enrollments");

            modelBuilder.EntityType<Student>()
                .Function("BestMarkInCourse")
                .Returns<BestMarkInCourseDto>();

            modelBuilder.EntityType<Student>()
                .Collection
                .Function("CourseEnrollments")
                .ReturnsCollection<StudentCourseListViewModel>();

            return modelBuilder.GetEdmModel();
        }
    }
}
