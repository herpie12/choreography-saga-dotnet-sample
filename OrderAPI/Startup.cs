using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OrderAPI.Consumers;
using OrderAPI.Db;
using OrderAPI.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;

namespace OrderAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase("orderDb"));

            services.AddScoped<OrderDbContext>();

            var connectionString = Configuration.GetConnectionString("OrderDb");

            services.AddDbContext<OrderDbContext>(opt =>
                 opt.UseSqlServer(connectionString));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                //c.SchemaFilter<EnumSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderAPI", Version = "v1" });
            });

            var bus = RabbitHutch.CreateBus(Configuration["RabbitMQ:ConnectionString"]);
            services.AddSingleton<IBus>(bus);
            services.AddSingleton<MessageDispatcher>();

            services.AddSingleton<AutoSubscriber>(_ =>
            {
                return new AutoSubscriber(_.GetRequiredService<IBus>(), Assembly.GetExecutingAssembly().GetName().Name)
                {
                    AutoSubscriberMessageDispatcher = _.GetRequiredService<MessageDispatcher>()
                };
            });

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<PaymentCompletedEventConsumer>();
            services.AddScoped<StocksReleasedEventConsumer>();

            services.AddHostedService<Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));
            }
        }
    }
}
