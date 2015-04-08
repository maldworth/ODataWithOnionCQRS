using ODataWithOnionCQRS.Core.DomainModels;
using ODataWithOnionCQRS.Core.Dto;
using ODataWithOnionCQRS.Core.ViewModels;
using AutoMapper;
using System.Web.Http;

namespace ODataWithOnionCQRS.MyODataApi
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            var profiles = GlobalConfiguration.Configuration.DependencyResolver.GetServices(typeof(Profile));

            Mapper.Initialize(cfg =>
            {
                foreach (Profile profile in profiles)
                    cfg.AddProfile(profile);
            });
        }
    }
}