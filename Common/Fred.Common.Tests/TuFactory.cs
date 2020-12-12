using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;

namespace Fred.Common.Tests
{
    public sealed class TuFactory
    {
        /// <summary>
        /// PErmet d'obtenir un delegate activateur
        /// </summary>
        /// <typeparam name="T">classe à activer</typeparam>
        /// <returns>delegete</returns>
        private Func<object[], object> GetActivator<T>() where T : class
        {
            return ExpressionActivator<Func<object[], object>>(typeof(T));
        }

        /// <summary>
        /// Contruit l'expression
        /// </summary>
        /// <typeparam name="TDelegate">activateur</typeparam>
        /// <param name="type">type de classe</param>
        /// <returns>delegate compilateur</returns>
        private Func<object[], object> ExpressionActivator<TDelegate>(Type type) where TDelegate : class
        {
            var ctorInfo = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();
            var ctorParams = ctorInfo.GetParameters();
            var paramExp = Expression.Parameter(typeof(object[]), "args");
            var expArr = new Expression[ctorParams.Length];
            for (var i = 0; i < ctorParams.Length; i++)
            {
                var ctorType = ctorParams[i].ParameterType;
                var argExp = Expression.ArrayIndex(paramExp, Expression.Constant(i));
                var argExpConverted = Expression.Convert(argExp, ctorType); expArr[i] = argExpConverted;
            }
            var newExp = Expression.New(ctorInfo, expArr);
            var lambda = Expression.Lambda<Func<object[], object>>(newExp, paramExp);
            return lambda.Compile();
        }

        /// <summary>
        /// Permet de créer un instance d'une classe de façon générique
        /// </summary>
        /// <typeparam name="T">classe</typeparam>
        /// <param name="args">arguments</param>
        /// <returns>Instance de classe</returns>
        public object CreateInstance<T>(params object[] args) where T : class
        {
            var activator = GetActivator<T>();
            return activator(args);
        }

        /// <summary>
        /// Permet de créer un mock à partir d'un Type
        /// </summary>
        /// <param name="interfaceType">type d'interface</param>
        /// <returns>Mock</returns>
        public Mock MakeMock(Type interfaceType)
        {
            Type generic = typeof(Mock<>);

            Type[] genericArgTypes = new[] { interfaceType };

            Type mockType = generic.MakeGenericType(genericArgTypes);

            Type[] argTypes = new[] { typeof(MockBehavior) };

            object[] argValues = new[] { (object)MockBehavior.Default };

            ConstructorInfo constructor = mockType.GetConstructor(argTypes);

            return constructor.Invoke(argValues) as Mock;
        }
    }
}
