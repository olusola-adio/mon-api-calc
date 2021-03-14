using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DFC.Functions.DI.Standard
{
    public static class DependencyInjectionWebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddDependencyInjection(this IWebJobsBuilder builder)
        {
            if(builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton<IInjectBindingProvider, InjectBindingProvider>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IFunctionFilter, ScopeCleanupFilter>());
            builder.AddExtension<InjectConfiguration>();

            return builder;
        }
    }
}