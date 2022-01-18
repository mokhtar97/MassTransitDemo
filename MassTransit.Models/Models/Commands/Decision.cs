using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Messages.Models.Commands
{
    public class Decision
    {
        public string From { get; set ; }
        public string To { get; set; }
        public string SchoolName { get; set; }
        public string Description { get; set; }
    }
}
