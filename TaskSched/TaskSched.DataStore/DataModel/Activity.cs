﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.DataStore.DataModel
{
    public class Activity
    {
        public Guid Id { get; set; }
        public Guid ActivityHandlerId { get; set; }
        public string Name { get; set; }
        public List<ActivityField> DefaultFields { get; set; }
        public List<EventActivity> TaskActions { get; set; }
    }

}
