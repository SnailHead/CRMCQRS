﻿using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Tags.Commands.CreateTag;
using CRMCQRS.Application.Tags.Queries.GetPageTag;

namespace CRMCQRS.API.Models;

public class GetPageTagDto : IMapWith<GetPageTagQuery>
{
    public string Title { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageTagDto, GetPageTagQuery>();
    }
}