using FluentValidation;
using Mapster;
using MapsterMapper;
using OnlineBookStore.Core.Constants;
using OnlineBookStore.Core.Entities;
using OnlineBookStore.Services.Books;
using OnlineBookStore.WebApi.Models;
using System.Net;

namespace OnlineBookStore.WebApi.Endpoints
{
    public static class AdminEndpoints
    {
        public static WebApplication MapAdminEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/admin");

            routeGroupBuilder.MapGet("/{acc}/{pass}", GetAdminDetails)
                .WithName("GetAdminDetail")
                .Produces<ApiResponse<bool>>();

            return app;
        }

        private static async Task<IResult> GetAdminDetails(string acc, string pass, IAdminRepository adminRepository)
        {
            var admin = await adminRepository.Check(acc, pass);
            return admin == false ? Results.Ok(ApiResponse.Success(false)) : Results.Ok(ApiResponse.Success(true));
        }
    }
}
