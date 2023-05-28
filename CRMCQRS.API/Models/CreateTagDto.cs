﻿using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Tags.Commands.CreateTag;
using CRMCQRS.Domain;

namespace CRMCQRS.API.Models;

public class CreateTagDto : IMapWith<CreateTagCommand>
{
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateTagDto, CreateTagCommand>();
    }
}