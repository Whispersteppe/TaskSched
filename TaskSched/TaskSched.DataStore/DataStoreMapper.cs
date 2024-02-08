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

namespace TaskSched.DataStore
{
    public interface IDataStoreMapper : IMapper
    {
    }
    public class DataStoreMapper : Mapper, IDataStoreMapper
    {
        IMapper _mapper;
        public DataStoreMapper()
            : base(CreateConfiguration())
        {

        }

        public DataStoreMapper(IConfigurationProvider configuration)
            : base(configuration)
        {
        }

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

                cfg.CreateMap<model.Calendar, db.Calendar>();
                cfg.CreateMap<db.Calendar, model.Calendar>();

            });

            return mapperConfig;

        }
    }
}
