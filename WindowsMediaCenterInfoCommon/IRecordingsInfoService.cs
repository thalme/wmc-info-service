using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WindowsMediaCenterInfoCommon
{
    [ServiceContract(Namespace = "http://RecordingsInfoService")]
    public interface IRecordingsInfoService
    {

        [OperationContract]
        ICollection<Recording> getOnGoingRecordings();

    }
}
