using PresentationTraker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker
{
    public class WorkSchedule
    {
        public Guid Id { get; set; }
        public Weekdays Day { get; set; }
        public TypesDays Type { get; set; }
        public string EntryTime { get; set; }
        public string ExitryTime { get; set; }
        public string Lunch_Time { get; set; }
        
        public List<Profile> Profiles { get; set; } = new List<Profile>();
    }
}
