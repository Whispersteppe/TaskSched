using AutoMapper;
using internalModel = TaskSched.Common.DataModel;
using externalModel = TaskScheduler.WinForm.Models;
using TaskSched.Common.FieldValidator;

namespace TaskScheduler.WinForm
{

    public class ManagerMapper : Mapper
    {
        IMapper _mapper;
        public ManagerMapper()
            : base(CreateConfiguration())
        {

        }

        public ManagerMapper(IConfigurationProvider configuration)
            : base(configuration)
        {
        }

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



    }
}
