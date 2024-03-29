﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.DataModel;
using TaskSched.Common.Interfaces;

namespace TaskSched.Test.DummyComponents
{
    internal class InMemoryActivityStore : IActivityStore
    {
        Dictionary<Guid, Activity> _activities;

        public InMemoryActivityStore()
        {
            _activities = new Dictionary<Guid, Activity>();
        }
        public async Task<ExpandedResult<Guid>> Create(Activity activity)
        {
            activity.Id = Guid.NewGuid();
            _activities[activity.Id] = activity;

            return new ExpandedResult<Guid>()
            {
                Result = activity.Id
            };
        }

        public async Task<ExpandedResult> Delete(Guid activityId)
        {
            if (_activities.ContainsKey(activityId))
            {
                _activities.Remove(activityId);
            }

            return new ExpandedResult() { };
        }

        public async Task<ExpandedResult<Activity>> Get(Guid activityId)
        {
            if (_activities.ContainsKey(activityId))
            {
                return new ExpandedResult<Activity>()
                {
                    Result = _activities[activityId],
                };
            }
            else
            {
                return new ExpandedResult<Activity>() { };
            }
        }

        public async Task<ExpandedResult<List<Activity>>> GetAll()
        {
            var rslt = new ExpandedResult<List<Activity>>() { Result = new List<Activity>() };
            foreach (var activity in _activities.Values)
            {
                rslt.Result.Add(activity);
            }

            return rslt;
        }

        public async Task<ExpandedResult<Activity>> GetDefault()
        {
            var activity = new Activity()
            { Name = "Default", Id = Guid.NewGuid(), ActivityHandlerId = Guid.NewGuid(), DefaultFields = new List<ActivityField>() };

            var created = await this.Create(activity);

            return await Get(created.Result);

        }

        public async Task<ExpandedResult> Update(Activity activity)
        {
            _activities[activity.Id] = activity;

            return new ExpandedResult() { };
        }
    }
}
