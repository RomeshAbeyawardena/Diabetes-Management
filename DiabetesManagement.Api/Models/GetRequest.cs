﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.Models
{
    public class GetRequest
    {
        public Guid? InventoryId { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public int? Version { get; set; }
    }
}