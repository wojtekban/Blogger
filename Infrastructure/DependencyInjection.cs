﻿using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICosmosPostRepository, CosmosPostRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            return services;
   
        }   
    }
}
