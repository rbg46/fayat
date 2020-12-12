using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fred.Web.Shared.App_LocalResources;
using Unity.Lifetime;

namespace Fred.Web.Bootstrapper.DependencyInjection
{
    public class DependencyInjectionMvcLifetimeManager : LifetimeManager, IInstanceLifetimeManager, IFactoryLifetimeManager, ITypeLifetimeManager
    {
        [ThreadStatic]
        private static Dictionary<Guid, object> values;
        private readonly Guid key = Guid.NewGuid();

        public override object GetValue(ILifetimeContainer container = null)
        {
            return IsInHttpContext() ? GetValueWithHttpContext() : GetValueWithoutHttpContext();

            object GetValueWithHttpContext() => UnityPerRequestHttpModule.GetValue(key);
            object GetValueWithoutHttpContext()
            {
                if (values == null)
                {
                    return NoValue;
                }

                return !values.TryGetValue(key, out object result) ? NoValue : result;
            }
        }

        public override void SetValue(object newValue, ILifetimeContainer container = null)
        {
            if (IsInHttpContext())
            {
                SetValueWithHttpContext();
            }
            else
            {
                SetValueWithoutHttpContext();
            }

            void SetValueWithHttpContext() => UnityPerRequestHttpModule.SetValue(key, newValue);
            void SetValueWithoutHttpContext()
            {
                if (values == null)
                {
                    values = new Dictionary<Guid, object>();
                }

                values[key] = newValue;
            }
        }

        private static bool IsInHttpContext()
        {
            return HttpContext.Current != null;
        }

        protected override LifetimeManager OnCreateLifetimeManager()
        {
            return new DependencyInjectionMvcLifetimeManager();
        }

        private class UnityPerRequestHttpModule : IHttpModule
        {
            private static readonly object ModuleKey = new object();

            public static object GetValue(object key)
            {
                Dictionary<object, object> registrations = GetRegistrations(HttpContext.Current);
                if (registrations == null || !registrations.TryGetValue(key, out object value))
                {
                    return NoValue;
                }

                return value;
            }

            public static void SetValue(object key, object value)
            {
                Dictionary<object, object> registrations = GetRegistrations(HttpContext.Current);
                if (registrations == null)
                {
                    InitializeRegistrations();
                }

                registrations[key] = value;

                void InitializeRegistrations()
                {
                    registrations = new Dictionary<object, object>();

                    HttpContext.Current.Items[ModuleKey] = registrations;
                }
            }

            public void Init(HttpApplication context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                context.EndRequest += OnEndRequest;
            }

            private void OnEndRequest(object sender, EventArgs e)
            {
                var application = (HttpApplication)sender;

                Dictionary<object, object> registrations = GetRegistrations(application.Context);
                if (registrations != null)
                {
                    DisposeRegistrations();
                }

                void DisposeRegistrations()
                {
                    foreach (IDisposable disposableType in registrations.Values.OfType<IDisposable>())
                    {
                        disposableType.Dispose();
                    }
                }
            }

            public void Dispose()
            {
                // No instance resource to dispose
            }

            private static Dictionary<object, object> GetRegistrations(HttpContext context)
            {
                if (context == null)
                {
                    throw new InvalidOperationException(FredResource.Exception_PerRequestLifetimeManagerWithoutHttpContext);
                }

                return context.Items[ModuleKey] as Dictionary<object, object>;
            }
        }
    }
}
