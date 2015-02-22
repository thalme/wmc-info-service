using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsMediaCenterInfoCommon
{

    [Serializable]
    public class Recording : IComparable
    {
        private string id;
        private string channelId;
        private string title;
        private DateTime startTime;
        private DateTime endTime;

        public Recording(string id, string channelId, string title, DateTime startTime, DateTime endTime)
        {
            this.id = id;
            this.channelId = channelId;
            this.title = title;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public string ChannelId
        {
            get { return channelId; }
        }

        public string Title
        {
            get { return title; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            Recording otherRecording = obj as Recording;
            if (otherRecording != null)
            {
                return this.id.CompareTo(otherRecording.id);
            }
            else
            {
                throw new ArgumentException("Object is not a Recording");
            }
        }
    }

}
