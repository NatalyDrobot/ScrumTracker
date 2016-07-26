using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DataLayer;
using DataLayer.Entities;
using DTO.Entities;
using WebSite.Models.Issue;

namespace WebSite.Util
{
    public static class AutoMapperConfig
    {
        private static IMapper Mapper { get; set; }
        private static MapperConfiguration MapperConfiguration { get; set; }

        static AutoMapperConfig()
        {
            MapperConfiguration = new MapperConfiguration(cfg => {
                cfg.CreateMap<IssueDto, IssueInfoModel>();
                cfg.CreateMap<IssueInfoModel, IssueDto>();
            });
            Mapper = MapperConfiguration.CreateMapper();
        }
        public static IssueInfoModel IssueDtoToIssueModelInfo(IssueDto from)
        {
            
            return Mapper.Map<IssueDto, IssueInfoModel>(from);
        }

        public static IssueDto IssuModelInfoToIssueDto(IssueInfoModel from)
        {
            return Mapper.Map<IssueInfoModel, IssueDto>(from);
        }
    }
}