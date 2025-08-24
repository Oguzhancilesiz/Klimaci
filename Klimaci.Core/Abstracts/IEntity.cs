﻿using Klimaci.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Core.Abstracts
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int AutoID { get; set; }

    }
}
