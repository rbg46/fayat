using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fred.Common.Tests
{
    /// <summary>
    /// Classe de base test des managers
    /// </summary>
    /// <typeparam name="T">classe à tester</typeparam>
    public abstract class BaseTu<T>
        where T : class
    {
        /// <summary>
        /// Instancie la classe de test
        /// </summary>
        /// <remarks>Auto Mockage des injections</remarks>
        private T InitializeActual()
        {
            return MockAllInjectionsProperties(CreateInstanceOfActual());
        }

        /// <summary>
        /// Instancie Actual
        /// </summary>
        private T CreateInstanceOfActual()
        {
            var argsObjects = new List<object>();
            foreach (var param in ParameterInfos)
            {
                //si on veut substitue le mock d'un argument
                object arg = null;
                if (ConstructorArgs.TryGetValue(param.ParameterType, out arg))
                {
                    argsObjects.Add(arg);
                }
                else
                {
                    //sinon automock
                    Mock argMocked;
                    if (!InterfaceMocks.TryGetValue(param.ParameterType, out argMocked))
                    {
                        argMocked = Factory.MakeMock(param.ParameterType);
                        InterfaceMocks.Add(param.ParameterType, argMocked);
                    }
                    argsObjects.Add(argMocked.Object);
                }
            }
            return Factory.CreateInstance<T>(argsObjects.ToArray()) as T;
        }

        /// <summary>
        /// Obtient ou définit la classe à tester
        /// </summary>
        protected T Actual => actual ?? (actual = InitializeActual());
        private T actual;

        /// <summary>
        /// obtient ou définit la méthode anonyme
        /// </summary>
        protected Delegate AnonymousMethod { get; set; }

        /// <summary>
        /// Liste des propriétés mockés
        /// </summary>
        private readonly Dictionary<Type, Mock> InterfaceMocks = new Dictionary<Type, Mock>();

        /// <summary>
        /// Liste des paramètres du contructeur non mocké
        /// </summary>
        private readonly Dictionary<Type, object> ConstructorArgs = new Dictionary<Type, object>();

        private IEnumerable<PropertyInfo> propertiesInfos;
        /// <summary>
        /// Obtient la liste des propriétés à l'exécution de la classe
        /// </summary>
        protected IEnumerable<PropertyInfo> PropertiesInfos => propertiesInfos ?? (propertiesInfos = typeof(T).GetRuntimeProperties());

        /// <summary>
        /// Obtient la liste des arguments du premier constructor de la classe
        /// </summary>
        protected IEnumerable<ParameterInfo> ParameterInfos => parameterInfos ?? (parameterInfos = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault().GetParameters());
        private IEnumerable<ParameterInfo> parameterInfos;

        /// <summary>
        /// Obtient la fabrique générique
        /// </summary>
        private TuFactory Factory => factory ?? (factory = new TuFactory());
        private TuFactory factory;

        /// <summary>
        /// Permet de remplacer la valeur d'une propriété de la classe
        /// </summary>
        /// /// <typeparam name="TType">type de propriété</typeparam>
        /// <param name="substitution">valeur</param>
        protected void SubstituteProperty<TType>(object substitution) where TType : class
        {
            //get also non-public properties
            var prop = PropertiesInfos.FirstOrDefault(c => c.PropertyType == typeof(TType));

            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(Actual, substitution, null);
            }

        }

        /// <summary>
        /// Permet de remplacer la valeur d'un argument du constructor de la classe
        /// </summary>
        /// <typeparam name="TType">type d'argument</typeparam>
        /// <param name="substitution">valeur</param>
        protected void SubstituteConstructorArgument<TType>(TType substitution) where TType : class
        {
            if (!ConstructorArgs.ContainsKey(typeof(TType)))
            {
                ConstructorArgs.Add(typeof(TType), substitution);
            }
            //Refresh de l'actual
            actual = null;
        }

        /// <summary>
        /// Permet d'instancer toutes les interfaces par des mocks
        /// </summary>
        private T MockAllInjectionsProperties(T actual)
        {
            foreach (var prop in PropertiesInfos)
            {
                if (prop.PropertyType.IsInterface)
                {
                    Mock mockProperty = null;
                    if (!InterfaceMocks.TryGetValue(prop.PropertyType, out mockProperty))
                    {
                        mockProperty = Factory.MakeMock(prop.PropertyType);
                        InterfaceMocks.Add(prop.PropertyType, mockProperty);
                    }

                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(actual, mockProperty.Object, null);
                    }
                }
            };
            return actual;
        }

        /// <summary>
        /// Permet de récuperer la propriété fictive
        /// </summary>
        /// <returns>object fictif</returns>
        public Mock<I> GetMocked<I>(params object[] args) where I : class
        {
            Mock mock = null;

            if (!InterfaceMocks.TryGetValue(typeof(I), out mock))
            {
                if (typeof(I) == typeof(FredDbContext))
                {
                    var options = new DbContextOptions<FredDbContext>();

                    args = new object[] { options };
                }

                mock = new Mock<I>(args);

                InterfaceMocks.Add(typeof(I), mock);
            }

            return (Mock<I>)mock;
        }

        /// <summary>
        ///  Permet d'obtenir l'action
        /// </summary>
        /// <param name="action">délégate</param>
        /// <returns>L'action</returns>
        public Action Invoking(Action action)
        {
            return action;
        }

        /// <summary>
        /// Permet d'obtenir l'action
        /// </summary>
        /// <param name="action">délégate</param>
        /// <returns>L'action</returns>
        public Func<TResult> Invoking<TResult>(Func<TResult> action)
        {
            return action;
        }

    }
}
