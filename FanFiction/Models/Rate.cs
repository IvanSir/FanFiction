﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanFiction.Models
{
    public class Rate
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FictionId { get; set; }

        public string UserId { get; set; }

        public int CurrentRate { get; set; }

    }
}
