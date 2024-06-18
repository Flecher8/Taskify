﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class NotificationFilterBuilder : BaseFilterBuilder<Notification>
    {
        public bool IncludeUser { get; private set; }

        public NotificationFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }
    }
}
