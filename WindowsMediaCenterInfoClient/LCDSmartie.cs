using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WindowsMediaCenterInfoCommon;

namespace WindowsMediaCenterInfoClient
{
    /// <summary>
    /// Defines the necessary methods to implement the LCDSmartie plugin specification.
    /// Uses WCF to decouple the client (plugin) from the server to allow the communication between the plugin (32-bit) and the server 
    /// </summary>
    public class LCDSmartie
    {
        private IRecordingsInfoService recordingsInfoServiceProxy = null;

        private const string CHANNEL_ID_DISPLAY_FORMAT_PLACEHOLDER = "$channelId";
        private const string TITLE_DISPLAY_FORMAT_PLACEHOLDER = "$title";
        private const string START_TIME_DISPLAY_FORMAT_PLACEHOLDER = "$startTime";
        private const string END_TIME_DISPLAY_FORMAT_PLACEHOLDER = "$endTime";

        private const string DEFAULT_RECORDING_DISPLAY_FORMAT = "$channelId: $title [$startTime-$endTime]";
        private const string DEFAULT_RECORDING_DISPLAY_SEPARATOR = " - ";


        public LCDSmartie()
        {
            CreateProxy();
        }

        /// <summary>
        /// Returns the number of currently on-going recordings.
        /// </summary>
        /// <param name="param1">Not used</param>
        /// <param name="param2">Not used</param>
        public string function1(string param1, string param2)
        {
            CheckChannel();
            return this.recordingsInfoServiceProxy.getOnGoingRecordings().Count.ToString();
        }

        /// <summary>
        /// Returns a formatted string of the currently on-going recordings.
        /// </summary>
        /// <param name="param1">Defines the format for each recording (i.e. "$title [$startTime - $endTime]).</param>
        /// <param name="param2">Defines the separator between each listed recording. (i.e. "-").</param>
        public string function2(string param1, string param2)
        {
            CheckChannel();

            string recordingDisplayFormat = (String.IsNullOrEmpty(param1) ? DEFAULT_RECORDING_DISPLAY_FORMAT : param1);
            string recordingDisplaySeparator = (String.IsNullOrEmpty(param2) ? DEFAULT_RECORDING_DISPLAY_SEPARATOR : param2);

            return RenderRecordingsToSingleLine(this.recordingsInfoServiceProxy.getOnGoingRecordings(), recordingDisplayFormat, recordingDisplaySeparator);
        }

        /// <summary>
        /// Checks if a process is running and returns 1 if this is the case otherwise 0. 
        /// </summary>
        /// <param name="param1">Name of the process (i.e. "ehshell.exe").</param>
        /// <param name="param2">Not used</param>
        public string function20(string param1, string param2)
        {
            string processName = (param1 != null ? param1.Replace(".exe", "") : "");
            return (IsProcessRunning(processName) ? "1" : "0");
        }


        #region ### HELPER METHODS ###

        private void CheckChannel()
        {
            if (((IChannel)recordingsInfoServiceProxy).State == CommunicationState.Faulted)
            {
                CreateProxy();
            }
        }

        private void CreateProxy()
        {
            ChannelFactory<IRecordingsInfoService> recordingsInfoServiceChannelFactory = new ChannelFactory<IRecordingsInfoService>(
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/WindowsMediaCenterInfoService")
            );

            this.recordingsInfoServiceProxy = recordingsInfoServiceChannelFactory.CreateChannel();
        }

        private string RenderRecordingsToSingleLine(ICollection<Recording> recordings, string recordingDisplayFormat, string recordingDisplaySeparator)
        {
            string renderedSingleRecordingLineStr = null;

            if (recordings.Count == 1)
            {
                renderedSingleRecordingLineStr = RenderSingleRecordingDisplayString(recordings.First(), recordingDisplayFormat) + " ";
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                foreach (Recording recording in recordings)
                {
                    sb.Append(RenderSingleRecordingDisplayString(recording, recordingDisplayFormat));
                    sb.Append(recordingDisplaySeparator);
                }

                renderedSingleRecordingLineStr = sb.ToString();
            }

            return renderedSingleRecordingLineStr;
        }

        private string RenderSingleRecordingDisplayString(Recording recording, string recordingDisplayFormat)
        {
            string renderedRecordingDisplayStr = recordingDisplayFormat;
            renderedRecordingDisplayStr = renderedRecordingDisplayStr.Replace(CHANNEL_ID_DISPLAY_FORMAT_PLACEHOLDER, recording.ChannelId);
            renderedRecordingDisplayStr = renderedRecordingDisplayStr.Replace(TITLE_DISPLAY_FORMAT_PLACEHOLDER, recording.Title);
            renderedRecordingDisplayStr = renderedRecordingDisplayStr.Replace(START_TIME_DISPLAY_FORMAT_PLACEHOLDER, recording.StartTime.ToShortTimeString());
            renderedRecordingDisplayStr = renderedRecordingDisplayStr.Replace(END_TIME_DISPLAY_FORMAT_PLACEHOLDER, recording.EndTime.ToShortTimeString());

            return renderedRecordingDisplayStr;
        }

        private bool IsProcessRunning(string processName)
        {
            return (Process.GetProcessesByName(processName).Length > 0);
        }

        #endregion

    }

}
