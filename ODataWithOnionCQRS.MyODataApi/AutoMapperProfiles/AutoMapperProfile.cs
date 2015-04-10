using AutoMapper;
using ODataWithOnionCQRS.Core.DomainModels;
using ODataWithOnionCQRS.MyODataApi.ViewModels;
using System.Collections.Generic;

namespace ODataWithOnionCQRS.MyODataApi.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Student, StudentCourseListViewModel>();
            Mapper.CreateMap<Enrollment, CourseDetailsViewModel>();
        }
    }
}