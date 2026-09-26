using AutoMapper;
using SharedLibrary.Commons.EntityAbstractions;

namespace SharedLibrary.Base.Mappers
{
    public interface IMapFrom<T> where T : Entity
    {
        void Mapping(Profile profile);
    }

    public abstract class MapFrom<T> : IMapFrom<T> where T : Entity
    {
        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
