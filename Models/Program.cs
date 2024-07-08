
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PresentationTraker.Models;
using System.Threading.Tasks;
using PresentationTraker.Enums;
using System.Drawing;
using System.Windows.Media;

namespace PresentationTraker
{
    public class Program
    {
        public int Number { get; set; }
        public string Id { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Duration { get; set; }
        public bool AllowedProgram { get; set; }
        public SolidColorBrush StatusProgram { get; set; } 
        public string ActivityId { get; set; }
    }
}
