using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fixera
{
    public class DefaultSubjectFactory<T> : ISubjectFactory<T>
        where T : class
    {
        private readonly IFakeProvider _fakeProvider;

        public DefaultSubjectFactory(IFakeProvider fakeProvider)
        {
            _fakeProvider = fakeProvider;
        }

        public T Create()
        {
            var type = typeof(T);
            var ctor = GetConstructor(type);
            var arguments = CreateArguments(ctor).ToArray();
            var subject = ctor.Invoke(arguments);
            return (T) subject;
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            var constructor = type.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length)
                .FirstOrDefault();

            if (constructor == null)
                throw new InvalidOperationException($"Could not find any public contructor on subject type {type.FullName}");

            return constructor;
        }

        private IEnumerable<object> CreateArguments(ConstructorInfo constructor)
        {
            foreach (var parameter in constructor.GetParameters())
            {
                yield return CreateArgument(parameter);
            }
        }

        private object CreateArgument(ParameterInfo parameter)
        {
            var typeInfo = parameter.ParameterType.GetTypeInfo();

            if (parameter.ParameterType != typeof(string) && (typeInfo.IsInterface || typeInfo.IsClass || typeInfo.IsArray))
            {
                var argument = ProvideArgument(parameter.ParameterType);
                return argument;
            }

            if (parameter.HasDefaultValue)
            {
                var argument = parameter.DefaultValue;
                return argument;
            }

            throw new InvalidOperationException(
                $"Could not create constructor argument {parameter.Name} of type {parameter.ParameterType.FullName}." +
                " Constructor parameters must either be reference types or have a default value."
            );
        }

        private object ProvideArgument(Type type)
        {
            var openMethod = GetType().GetTypeInfo().GetDeclaredMethod(nameof(ProvideArgumentGeneric));
            var closedMethod = openMethod.MakeGenericMethod(type);
            var result = closedMethod.Invoke(this, new object[0]);
            return result;
        }

        private TFake ProvideArgumentGeneric<TFake>()
            where TFake : class
        {
            return _fakeProvider.GetSingleton<TFake>();
        }
    }
}
