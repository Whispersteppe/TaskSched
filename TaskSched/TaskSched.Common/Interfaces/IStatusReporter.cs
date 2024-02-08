﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSched.Common.Interfaces
{
    /// <summary>
    /// allows something to push status to a listener
    /// </summary>
    public interface IStatusListener
    {
        //TODO
    }

    /// <summary>
    /// allows something to pull status from the scheduling engine
    /// </summary>
    public interface ISchedulerEngineStatus
    {
        //TODO

    }


    /// <summary>
    /// allows something to pull status from the execution engine
    /// </summary>
    public interface IExecutionEngineStatus
    {
        //TODO
    }

}
