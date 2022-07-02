using BotFramework.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Telegram.Bot;
using WebApi.Handlers;

namespace WebApi;

public class Startup
{
    // Конфигурация бота из appsettings.json
    public BotConfiguration BotConfiguration { get; }
        
    public Startup(IConfiguration configuration)
    {
        BotConfiguration = configuration.GetSection("BotConfiguration")
            .Get<BotConfiguration>();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Внедрение WebHook'а
        services.AddHostedService<WebhookConfiguration>();
            
        // Внедрение Telegram.Bot
        services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>
                (
                    httpClient => new TelegramBotClient
                    (
                        BotConfiguration.Token, 
                        httpClient
                    )
                );
            
        // Внедрение BotFramework    
        services.AddBotFramework<ITelegramBotClient>()
                .AddHandler<EchoHandler>();
            
        services.AddControllers()
            .AddNewtonsoftJson();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc
            (
                "v1",
                new OpenApiInfo
                {
                    Title = "WebApi", 
                    Version = "v1"
                }
            );
        });
    }

    // Большая часть параметров для настройки Telegram Bot API взята из https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.WebHook
    public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            applicationBuilder.UseDeveloperExceptionPage()
                .UseSwagger()
                .UseSwaggerUI
                (
                    options => options.SwaggerEndpoint
                    (
                        "/swagger/v1/swagger.json", 
                        "WebApi v1"
                    )
                );
        }

        applicationBuilder.UseHttpsRedirection()
                          .UseRouting()
                          .UseCors();

        applicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute
            (
                name: "tgwebhook",
                pattern: $"Bot/GetUpdates",
                new
                {
                    сontroller = "Bot",
                    action = "Post"
                }
            );
                
            endpoints.MapControllers();
        });
    }
}