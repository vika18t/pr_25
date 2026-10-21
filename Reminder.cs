using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pr_25
{
    public class Reminder
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string Text { get; set; }
        public bool IsShown { get; set; } // уже показано

        public override string ToString()
        {
            string s = "не показана";
            if (IsShown)
                s = "показана";
            return $"{Hour}:{Minute} {Text} ({s})";
        }
    }

}
