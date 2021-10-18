using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMBLNetNbanking
{
    public class AuditLogFetch
    {
        public int AuditID {get; set;}

        public string CRN { get; set; }

        public string CreatedOn { get; set; }

        public int SessionID { get; set; }

        public int EventID { get; set; }

        public string EventName { get; set; }

        public int StatusID { get; set; }

        public string StatusName { get; set; }

        public string ResultReason { get; set; }

        public string ChannelName { get; set; }

        public string GateFailedAt { get; set; }

        //public string DeviceDetails { get; set; }

        public int ParentTransactionID { get; set; }

        public int Score { get; set; }

        public int IsCompleted { get; set; }

        public string Version { get; set; }

        public string json_response { get; set; }

        public List<AuditLogExpandFetch> AuditLogExpandFetch { get; set; }

    }

    public class AuditLogRegisterCount
    {
        public int reg_complete_single_attempt { get; set; }

        public int reg_complete_multiple_attempt { get; set; }

        public int reg_average_completed_users { get; set; }

        public int reg_drop_single_attempt { get; set; }

        public int reg_drop_multiple_attempt { get; set; }
        
        public int reg_average_dropped_users { get; set; }

        public int reg_successful_attempts { get; set; }

        public int reg_failed_attempts { get; set; }
    }

    public class AuditLogVerifyCount
    {
        public int verify_complete_single_attempt { get; set; }

        public int verify_complete_multiple_attempt { get; set; }

        public int verify_average_completed_users { get; set; }

        public int verify_drop_single_attempt { get; set; }

        public int verify_drop_multiple_attempt { get; set; }

        public int verify_average_dropped_users { get; set; }

        public int verify_successful_attempts { get; set; }

        public int verify_failed_attempts { get; set; }
    }

    public class AuditLogExcelDownload
    {
    
        public string CreatedOn { get; set; }

        public string CRN { get; set; }

        public string EventName { get; set; }

        public string Version { get; set; }

        public string StatusName { get; set; }

        public string GateFailedAt { get; set; }

        public string ResultReason { get; set; }

        public string ChannelName { get; set; }

        public int GATE_NAME { get; set; }

        public string json_response { get; set; }

        public string threshold_value { get; set; }

    }

    public class UserImageZipDownload
    {

        public string CreatedOn { get; set; }

        public string CRN { get; set; }

        public string customer_images { get; set; }
    }

}