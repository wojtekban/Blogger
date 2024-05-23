﻿using Swashbuckle.AspNetCore.Filters;
using WebAPI.Wrappers;

namespace WebAPI.SwaggerExamples.Responses;

public class RegisterResponseStatus200Example : IExamplesProvider<RegisterResponseStatus200>
{
    public RegisterResponseStatus200 GetExamples()
    {
        return new RegisterResponseStatus200
        {
            Succeeded = true,
            Message = "User Created successfully!"
        };
    }
}

public class RegisterResponseStatus200 : Response
{ }