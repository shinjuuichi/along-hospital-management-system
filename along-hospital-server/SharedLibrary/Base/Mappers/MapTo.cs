using System.Reflection;
using AutoMapper;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Utils;

namespace SharedLibrary.Base.Mappers
{
    public interface IMapTo<T> where T : Entity
    {
        void Mapping(Profile profile);
    }

    public abstract class MapTo<T> : IMapTo<T> where T : Entity
    {
        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap(GetType(), typeof(T))
            .BeforeMap(BeforeMapping)
            .AfterMap(AfterMapping);
        }

        protected virtual void BeforeMapping(object source, object dest)
        {
            var stringProps = source.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var prop in stringProps)
            {
                var value = prop.GetValue(source) as string;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    prop.SetValue(source, value.Trim());
                }
            }

            source.TryValidate();
        }

        protected virtual void AfterMapping(object source, object dest)
        {
            dest.TryValidate();
        }
    }
}