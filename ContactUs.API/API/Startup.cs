using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using System.IO;

namespace API
{
  public class Startup
  {
    private const string _corsPolicy = "CorsPolicy";
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Register AutoMapper
      services.AddAutoMapper(typeof(Startup));
      // Register IRepository Service
      services.AddScoped<IRepository, Repository>();
      // Register ContactUsContext
      services.AddDbContext<ContactUsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
      // Register API Controllers
      services.AddControllers(config => config.Filters.Add(typeof(ApiExceptionFilter)));
      // Register Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactUs_API", Version = "v1" }));
      // Allow CORS
      services.AddCors(o =>
        o.AddPolicy(_corsPolicy, builder =>
         {
           builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
         }
      ));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      // Enable serving static files placed in [wwwroot] folder
      app.UseStaticFiles();
      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();
      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS_API_v1"));
      // Enable App Routing
      app.UseRouting();
      // global exception handler
      app.UseExceptionHandler(appError =>
      {
        appError.Run(async context =>
        {
          context.Response.ContentType = "application/json";
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
          if (contextFeature != null)
            await context.Response.WriteAsync(JsonSerializer.Serialize(contextFeature.Error));
        });
      });
      // UseCors with CorsPolicyBuilder.
      app.UseCors(_corsPolicy);
      // Use Authorization
      app.UseAuthorization();
      // handle API routes
      app.UseEndpoints(endpoints => endpoints.MapControllers());
      // handle client side routes [catch all routes for SPA]
      app.Run(async (context) =>
      {
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
      });
    }
  }
}
