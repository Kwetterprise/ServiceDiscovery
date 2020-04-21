namespace Kwetterprise.ServiceDiscovery.IntegrationTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public static class Extensions
    {
        #region Asserting

        public static T AssertSingle<T>(this IEnumerable<T> collection)
        {
            return Assert.Single(collection);
        }

        public static T AssertSingle<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            return Assert.Single(collection, predicate);
        }

        public static TOut AssertIsType<TOut>(this object @in)
        {
            return Assert.IsType<TOut>(@in);
        }

        public static void AssertEqual<T>(this T actual, T expected)
        {
            Assert.Equal(expected, actual);
        }

        public static T? NullableSingleOrDefault<T>(this IEnumerable<T> source, Predicate<T> predicate)
            where T : struct
        {
            T? found = null;
            foreach (var variable in source)
            {
                if (predicate(variable))
                {
                    if (found != null)
                    {
                        throw new InvalidOperationException($"More than one element satisfies the condition in {nameof(predicate)}.");
                    }

                    found = variable;
                }
            }

            if (found == null)
            {
                throw new InvalidOperationException($"No element satisfies the condition in {nameof(predicate)}.");
            }

            return found;
        }

        public static void AssertEmpty(this IEnumerable source)
        {
            Assert.Empty(source);
        }

        #endregion

        #region Json

        public static string ToJson(this Dictionary<string, string> dict)
        {
            return System.Text.Json.JsonSerializer.Serialize(dict);
        }

        public static HttpContent ToContent(this Service service)
        {
            return new StringContent(service.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        #endregion

        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);

            return services;
        }

        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> factory,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var objectFactory = new Func<IServiceProvider, object>(p => factory(p));

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), objectFactory, lifetime);

            services.Add(descriptorToAdd);

            return services;
        }
    }
}
