using System;
using EventSourcing;
using EventSourcing.EventBus;
using Kanban.Projections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plk.Blazor.DragDrop;
using SimpleUI.Data;

namespace SimpleUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            
            services.AddSingleton<IAppendOnlyStore>(_ => new PoorMansAppendOnlyStore("db.txt"));
            services.AddSingleton<IEventStore, EventStore>();
            services.AddSingleton<IEntityRepository, EventSourcedRepository>();
            services.AddSingleton<KanbanBoardLiveProjection>();
            services.AddSingleton<IEventBus, EventBus>();
            services.AddSingleton<IDomainEventHandler>(provider => provider.GetService<KanbanBoardLiveProjection>() ?? throw new Exception("Can't get instance of live projection"));
            services.AddSingleton<KanbanBoardService>();
            services.AddHostedService<HydrateLiveProjection>();
            
            services.AddBlazorDragDrop();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}