using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetApp
{
    internal class Meetup
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Start {
            get { return _start; }
            set {
                if (value <= DateTime.Now)
                    throw new Exception($"Meetup can't start ({value}) in past {DateTime.Now}");
                if (value >= End && End != DateTime.MinValue)
                    throw new Exception($"Meetup can't end ({End}) before it starts ({value})");
                _start = value;
                ScheduleAlert();
            }
        }
        public DateTime End { 
            get { return _end; }
            set {
                if (value <= DateTime.Now)
                    throw new Exception($"Meetup can't end ({value}) in past {DateTime.Now}");
                if (value <= Start)
                    throw new Exception($"Meetup can't end ({value}) before it starts ({Start})");
                _end = value;
            }
        }
        public TimeSpan AlertBefore {
            get { return _alertBefore; }
            set {
                if (value < TimeSpan.Zero)
                    throw new ArgumentException("Alert time cannot be negative");
                _alertBefore = value;
                ScheduleAlert();
            }
        }

        private Timer? _timer;
        private TimeSpan _alertBefore;
        private DateTime _start;
        private DateTime _end;

        public event EventHandler? MeetupStartsSoon;


        public Meetup(string name, string description, DateTime start, DateTime end, TimeSpan alertBefore, EventHandler? notifyStart = null)
        {
            MeetupStartsSoon = notifyStart;
            Name = name;
            Description = description;
            End = end;
            Start = start;
            AlertBefore = alertBefore;
        }

        private void ScheduleAlert()
        {
            _timer?.Dispose();

            DateTime now = DateTime.Now;

            TimeSpan untilAlert = Start - DateTime.Now - AlertBefore;

            if (untilAlert <= TimeSpan.Zero)
            {
                MeetupStartsSoon?.Invoke(this, EventArgs.Empty);
                return;
            }

               _timer = new Timer(_ => MeetupStartsSoon?.Invoke(this, EventArgs.Empty), null, untilAlert, Timeout.InfiniteTimeSpan);
        }
        public override string ToString()
        {
            return $"{Name} {Description} {Start.ToShortDateString()}: {Start.ToShortTimeString()} - {End.ToShortTimeString()}";
        }
    }
}
