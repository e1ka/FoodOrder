﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Website
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public virtual List<Category> Categories { get; set; }
    }
}
