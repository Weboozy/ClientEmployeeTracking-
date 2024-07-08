using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker
{
    public class BackgroundProgram
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset EntryTime { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<Profile> Profiles { get; set; } = new List<Profile>();
    }
}
