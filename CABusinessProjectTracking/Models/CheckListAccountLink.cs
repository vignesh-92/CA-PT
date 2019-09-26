using PT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessProjectTracking.Models
{
    public class CheckListAccountLink
    {
        public string ResourceName { get; set; }
        public string ProjectID { get; set; }
        public string DeliverableID { get; set; }
        public string DeliverableDetailID { get; set; }
        public string[] CheckListIDS { get; set; }
        public string Stage { get; set; }
        public string State { get; set; }
    }

    public class CheckListAccountResponse
    {
        public List<CheckListAccount> CheckListAccountList { get; set; }

        public List<State> StateList;

    }
    public class CheckListMappingList
    {
        public string UserID { get; set; }
        public int CheckListMapId { get; set; }
        public int CheckListId { get; set; }
        public int RoleId { get; set; }
        public int ProjectId { get; set; }
        public int StageId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string[] CheckListIDS { get; set; }
    }
}