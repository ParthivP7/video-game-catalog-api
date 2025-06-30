using System.Reflection;
using Application.Pipelines;
using Application.VideoGames.Commands;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>)); // Optional pre-processing
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>)); // Logging performance
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestValidationBehavior<>));   // Validation

        // Register MediatR handlers from Application assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AddVideoGameCommand.Handler).Assembly));

        // Register FluentValidation validators from same assembly
        AssemblyScanner
            .FindValidatorsInAssemblyContaining<AddVideoGameCommand.Validator>()
            .ForEach(pair => services.AddTransient(pair.InterfaceType, pair.ValidatorType));

        return services;
    }
}
