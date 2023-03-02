using Bicep.Core;
using Bicep.Core.Analyzers.Interfaces;
using Bicep.Core.Analyzers.Linter;
using Bicep.Core.Analyzers.Linter.ApiVersions;
using Bicep.Core.Configuration;
using Bicep.Core.Features;
using Bicep.Core.FileSystem;
using Bicep.Core.Registry;
using Bicep.Core.Registry.Auth;
using Bicep.Core.Semantics.Namespaces;
using Bicep.Core.TypeSystem.Az;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using BicepConfigurationManager = Bicep.Core.Configuration.ConfigurationManager;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBicepCore(this IServiceCollection services) => services
        .AddScoped<INamespaceProvider, DefaultNamespaceProvider>()
        .AddScoped<IAzResourceTypeLoader, AzResourceTypeLoader>()
        .AddScoped<IContainerRegistryClientFactory, ContainerRegistryClientFactory>()
        .AddScoped<ITemplateSpecRepositoryFactory, TemplateSpecRepositoryFactory>()
        .AddScoped<IModuleDispatcher, ModuleDispatcher>()
        .AddScoped<IModuleRegistryProvider, DefaultModuleRegistryProvider>()
        .AddScoped<ITokenCredentialFactory, TokenCredentialFactory>()
        .AddScoped<IFileResolver, UriFileResolver>()
        .AddScoped<IFileSystem, MockFileSystem>()
        .AddScoped<IConfigurationManager, BicepConfigurationManager>()
        .AddScoped<IApiVersionProviderFactory, ApiVersionProviderFactory>()
        .AddScoped<IBicepAnalyzer, LinterAnalyzer>()
        .AddScoped<IFeatureProviderFactory, FeatureProviderFactory>()
        .AddScoped<ILinterRulesProvider, LinterRulesProvider>()
        .AddScoped<BicepCompiler>();
}