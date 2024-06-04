using AutoMapper;
using internalModel = TaskSched.Common.DataModel;
using externalModel = TaskScheduler.WinForm.Models;
using TaskSched.Common.FieldValidator;
using System.Linq.Expressions;

namespace TaskScheduler.WinForm
{

    public class ManagerMapper : IMapper
    {
        Mapper _mapper;
        public ManagerMapper()
            : this(CreateConfiguration())
        {

        }

        public ManagerMapper(IConfigurationProvider configuration)
        {
            _mapper = new Mapper(configuration);
        }

        public IConfigurationProvider ConfigurationProvider => CreateConfiguration();

        static IConfigurationProvider CreateConfiguration()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => {

                // entities
                cfg.CreateMap<internalModel.Event, externalModel.EventModel>();
                cfg.CreateMap<externalModel.EventModel, internalModel.Event>();

                cfg.CreateMap<internalModel.Event, externalModel.EventModel>();
                cfg.CreateMap<externalModel.EventModel, internalModel.Event>();
                cfg.CreateMap<internalModel.EventSchedule, externalModel.EventScheduleModel>();
                cfg.CreateMap<externalModel.EventScheduleModel, internalModel.EventSchedule>();
                cfg.CreateMap<internalModel.EventActivity, externalModel.EventActivityModel>();
                cfg.CreateMap<externalModel.EventActivityModel, internalModel.EventActivity>();
                cfg.CreateMap<internalModel.EventActivityField, externalModel.EventActivityFieldModel>();
                cfg.CreateMap<externalModel.EventActivityFieldModel, internalModel.EventActivityField>();


                cfg.CreateMap<internalModel.Activity, externalModel.ActivityModel>();
                cfg.CreateMap<externalModel.ActivityModel, internalModel.Activity>();

                cfg.CreateMap<internalModel.ActivityField, externalModel.ActivityFieldModel>();
                cfg.CreateMap<externalModel.ActivityFieldModel, internalModel.ActivityField>();


                cfg.CreateMap<internalModel.Folder, externalModel.FolderModel>();
                cfg.CreateMap<externalModel.FolderModel, internalModel.Folder>();


                cfg.CreateMap<internalModel.ActivityField, internalModel.ActivityField>();
                cfg.CreateMap<internalModel.ActivityField, internalModel.ActivityField>();

                cfg.CreateMap<internalModel.EventActivity, internalModel.EventActivity>();
                cfg.CreateMap<internalModel.EventActivity, internalModel.EventActivity>();

                cfg.CreateMap<internalModel.EventActivityField, internalModel.EventActivityField>();
                cfg.CreateMap<internalModel.EventActivityField, internalModel.EventActivityField>();

                cfg.CreateMap<internalModel.EventSchedule, internalModel.EventSchedule>();
                cfg.CreateMap<internalModel.EventSchedule, internalModel.EventSchedule>();

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
            return _mapper.Map(source, destination, sourceType, destinationType);
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
