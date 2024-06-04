using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using model = TaskSched.Common.DataModel;
using db = TaskSched.DataStore.DataModel;
using System.Linq.Expressions;

namespace TaskSched.DataStore
{
    public interface IDataStoreMapper : IMapper
    {
    }
    public class DataStoreMapper : IDataStoreMapper
    {
        Mapper _mapper;

        //IMapper _mapper;
        public DataStoreMapper()
            :this(CreateConfiguration())
        {

        }

        public DataStoreMapper(IConfigurationProvider configuration)
        {
            _mapper = new Mapper(configuration);
        }

        public IConfigurationProvider ConfigurationProvider => throw new NotImplementedException();

        static IConfigurationProvider CreateConfiguration()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => {

                // entities
                cfg.CreateMap<model.Event, db.Event>();
                cfg.CreateMap<db.Event, model.Event>();

                cfg.CreateMap<model.Activity, db.Activity>();
                cfg.CreateMap<db.Activity, model.Activity>();

                cfg.CreateMap<model.ActivityField, db.ActivityField>();
                cfg.CreateMap<db.ActivityField, model.ActivityField>();

                cfg.CreateMap<model.EventActivity, db.EventActivity>();
                cfg.CreateMap<db.EventActivity, model.EventActivity>();

                cfg.CreateMap<model.EventActivityField, db.EventActivityField>();
                cfg.CreateMap<db.EventActivityField, model.EventActivityField>();

                cfg.CreateMap<model.EventSchedule, db.EventSchedule>();
                cfg.CreateMap<db.EventSchedule, model.EventSchedule>();

                cfg.CreateMap<model.Folder, db.Folder>();
                cfg.CreateMap<db.Folder, model.Folder>();

            });

            return mapperConfig;

        }

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
        {
            return _mapper.Map(source, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return _mapper.Map(source, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return _mapper.Map<TSource, TDestination>(source, destination, opts);
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            return _mapper.Map(source, sourceType, destinationType, opts);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            return _mapper.Map(source, destination, sourceType, destinationType, opts);
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map<TSource, TDestination>(source, destination);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, destination, sourceType, destinationType );
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return _mapper.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
        {
            return _mapper.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }

        public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters = null, params string[] membersToExpand)
        {
            return _mapper.ProjectTo(source, destinationType, parameters, membersToExpand);
        }
    }
}
