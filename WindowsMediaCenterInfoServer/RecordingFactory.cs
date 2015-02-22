using Microsoft.MediaCenter.TV.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsMediaCenterInfoCommon;

namespace WindowsMediaCenterInfoServer
{
    
    class RecordingFactory
    {
        private const string CHANNEL_ID = "ChannelID";
        private const string TITLE = "Title";

        private RecordingFactory()
        {
            //
        }

        public static Recording CreateFromScheduleEvent(ScheduleEvent scheduleEvent)
        {
            return new Recording(
                scheduleEvent.Id,
                (string)scheduleEvent.GetExtendedProperty(CHANNEL_ID),
                (string)scheduleEvent.GetExtendedProperty(TITLE),
                scheduleEvent.StartTime.ToLocalTime(),
                scheduleEvent.EndTime.ToLocalTime()
            );
        }

    }
}
