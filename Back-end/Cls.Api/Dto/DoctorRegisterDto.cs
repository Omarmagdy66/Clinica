﻿using Dto;

namespace Cls.Api.Dto
{
    public class DoctorRegisterDto : DoctorDto
    {
        public string UserName { get; set; }
        public int RoleId { get; set; }

    }
}
