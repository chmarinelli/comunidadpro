using AutoMapper;
using MiComunidadPro.Common;
using MiComunidadPro.Business.Entities;
using MiComunidadPro.Web.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace MiComunidadPro.Web.Infrastructure.AutoMapper
{
    public class AutoMapperCommonProfileConfiguration : Profile
    {
        public AutoMapperCommonProfileConfiguration()
        : this("GlobalProfile")
        {
        }
        protected AutoMapperCommonProfileConfiguration(string profileName)
        : base(profileName)
        {
            this.RecognizePrefixes("Config");

            //NOTE: Only map the types that are common in all your apis
        }
    }
}
