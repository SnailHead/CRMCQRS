﻿using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Domain;

namespace CRMCQRS.Application.Users.Queries;

public class UserViewModel : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string? Middlename { get; set; }
    public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Password { get; set; }
    public int DepartmentId { get; set; }
    public long TelegramChatId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserViewModel>();
    }
}