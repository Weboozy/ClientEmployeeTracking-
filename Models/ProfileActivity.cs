using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker
{
    public class ProfileActivity
    {

        public string id { get; set; }
        public string startWorking { get; set; }
        public string endWorking { get; set; }
        public DateTime date { get; set; }
        public DateTime createdAt { get; set; }
        public string downtime { get; set; }
        public string profileId { get; set; }

    }
}
