using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTraker
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid ProgramId { get; set; }
        public Program? Program { get; set; }
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
