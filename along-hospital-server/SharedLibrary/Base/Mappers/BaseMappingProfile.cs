using AutoMapper;
using System.Reflection;

namespace SharedLibrary.Base.Mappers
{
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public BaseMappingProfile(params Assembly[] additionalAssemblies)
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var assembly in additionalAssemblies)
            {
                ApplyMappingsFromAssembly(assembly);
            }
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t != typeof(MapFrom<>)
                        && t != typeof(MapTo<>)
                        && t.GetInterfaces().Any(i => i.IsGenericType
                            && (i.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                            || i.GetGenericTypeDefinition() == typeof(IMapTo<>))
                ))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, [this]);
            }
        }
    }
}