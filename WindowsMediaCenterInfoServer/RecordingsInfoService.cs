using Microsoft.MediaCenter.TV.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsMediaCenterInfoCommon;

namespace WindowsMediaCenterInfoServer
{
    public class RecordingsInfoService : IRecordingsInfoService
    {
        private EventSchedule eventSchedule;
        private ISet<Recording> onGoingRecordings = new SortedSet<Recording>();

        public RecordingsInfoService()
        {
            this.eventSchedule = new EventSchedule();
            this.eventSchedule.ScheduleEventStateChanged += eventSchedule_ScheduleEventStateChanged;
            FindOnGoingRecordings();
        }

        public ICollection<Recording> getOnGoingRecordings()
        {
            return onGoingRecordings;
        }

        #region ### HELPER METHODS ###

        private void FindOnGoingRecordings()
        {
            DateTime now = DateTime.Now;
            ICollection<ScheduleEvent> scheduledEventsLast24h = eventSchedule.GetScheduleEvents(now.AddDays(-1.0), now, ScheduleEventStates.IsOccurring);

            foreach (ScheduleEvent scheduleEvent in scheduledEventsLast24h)
            {
                onGoingRecordings.Add(RecordingFactory.CreateFromScheduleEvent(scheduleEvent));
            }
        }

        private void eventSchedule_ScheduleEventStateChanged(object sender, ScheduleEventChangedEventArgs e)
        {
            ScheduleEventChange[] scheduleEventChanges = e.Changes;

            foreach (ScheduleEventChange scheduleEventChange in scheduleEventChanges)
            {

                if (scheduleEventChange.NewState == ScheduleEventStates.IsOccurring)
                {
                    ScheduleEvent scheduleEvent = eventSchedule.GetScheduleEventWithId(scheduleEventChange.ScheduleEventId);
                    onGoingRecordings.Add(RecordingFactory.CreateFromScheduleEvent(scheduleEvent));
                }
                else if (scheduleEventChange.NewState == ScheduleEventStates.HasOccurred)
                {
                    ScheduleEvent scheduleEvent = eventSchedule.GetScheduleEventWithId(scheduleEventChange.ScheduleEventId);
                    onGoingRecordings.Remove(RecordingFactory.CreateFromScheduleEvent(scheduleEvent));
                }
            }
        }

        #endregion

    }

}
