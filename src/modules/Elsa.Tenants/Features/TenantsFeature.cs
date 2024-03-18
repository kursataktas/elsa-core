using Elsa.Common.Contracts;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Elsa.Tenants.Contracts;
using Elsa.Tenants.Options;
using Elsa.Tenants.Providers;
using Elsa.Tenants.Resolvers;
using Elsa.Tenants.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Tenants.Features;

/// <summary>
/// Configures multi-tenancy features.
/// </summary>
public class TenantsFeature : FeatureBase
{
    /// <inheritdoc />
    public TenantsFeature(IModule serviceConfiguration) : base(serviceConfiguration)
    {
    }

    /// <summary>
    /// Configures the Tenants options.
    /// </summary>
    public Action<MultiTenancyOptions> TenantsOptions { get; set; } = _ => { };

    /// <summary>
    /// A delegate that creates an instance of an implementation of <see cref="ITenantsProvider"/>.
    /// </summary>
    public Func<IServiceProvider, ITenantsProvider> TenantsProvider { get; set; } = sp => sp.GetRequiredService<ConfigurationTenantsProvider>();

    /// <inheritdoc />
    public override void Apply()
    {
        Services.Configure(TenantsOptions);

        Services
            .AddScoped<ITenantResolver, TenantResolver>()
            .AddSingleton<ConfigurationTenantsProvider>()
            .AddSingleton<IAmbientTenantAccessor, AmbientTenantAccessor>()
            .AddScoped(TenantsProvider)
            .AddHttpContextAccessor();

        Services
            .AddScoped<ITenantResolutionStrategy, AmbientTenantResolver>();
    }

    /// <summary>
    /// Configures the feature to use <see cref="ConfigurationTenantsProvider"/>.
    /// </summary>
    public TenantsFeature UseConfigurationBasedTenantsProvider()
    {
        return UseTenantsProvider(sp => sp.GetRequiredService<ConfigurationTenantsProvider>());
    }
    
    /// <summary>
    /// Configures the feature to use the specified <see cref="ITenantsProvider"/>.
    /// </summary>
    public TenantsFeature UseTenantsProvider(Func<IServiceProvider, ITenantsProvider> provider)
    {
        TenantsProvider = provider;  
        return this;
    }
}