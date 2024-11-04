using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetApp
{
    internal class MeetupManager
    {
        public event EventHandler? MeetupStartsSoon;
        public List<Meetup> MeetupList = new List<Meetup>();

        public MeetupManager(EventHandler? alertStart = null)
        {
            MeetupStartsSoon = alertStart;
        }
        public void AddMeetup(string name, string desc, DateTime start, DateTime end, TimeSpan alert)
        {
            if (MeetupList.Any(m => start < m.End && end > m.Start))
                throw new InvalidOperationException("New meetup overlaps with the other one");

            MeetupList.Add(new Meetup(name, desc, start, end, alert, MeetupStartsSoon));
        }
        public void RemoveMeetup(int id)
        {
            if (id > MeetupList.Count - 1 || id < 0)
                throw new ArgumentException("id cannot be less than 0 and bigger than number of meetups");
            MeetupList.RemoveAt(id);
        }
        public void ChangeMeetup(int id, string? name = null, string? desc = null, DateTime? start = null, DateTime? end = null, TimeSpan? alert = null)
        {
            if (id > MeetupList.Count - 1 || id < 0)
                throw new ArgumentException("id cannot be less than 0 and bigger than number of meetups");

            if (name != null)
                MeetupList[id].Name = name;

            if (desc != null)
                MeetupList[id].Description = desc;

            if (start != null && end != null)
            { 
                if (alert != null)
                    MeetupList[id] = new Meetup(MeetupList[id].Name, MeetupList[id].Description, start.Value, end.Value, alert.Value, MeetupStartsSoon);
                else
                    MeetupList[id] = new Meetup(MeetupList[id].Name, MeetupList[id].Description, start.Value, end.Value, MeetupList[id].AlertBefore.Value, MeetupStartsSoon);
                return;
            }

            if (start != null)
                MeetupList[id].Start = start.Value;
            if (end != null)
                MeetupList[id].End = end.Value;
            if (alert != null)
                MeetupList[id].AlertBefore = alert.Value;


        }
        public List<Meetup> GetMeetupsForDate(DateTime date) => MeetupList.Where(m => m.Start.Value.Date == date.Date).ToList();
        public string GetStringMeetupsForDate(DateTime date) => date.ToShortDateString() + "\n" + string.Join("\n", GetMeetupsForDate(date).Select(m => m.ToString()));
    }
}
