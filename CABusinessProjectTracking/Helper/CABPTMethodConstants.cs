using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessProjectTracking.Helper
{
	public static class CABPTMethodConstants
	{
		#region "Projects"
		public const string ALLPROJECTS = "AllProjects";
        public const string ALLDELIVERABLES = "AllDeliverables";
        public const string ALLUTILIZATIONS = "AllResourseUtilization";
        public const string REPORTFILTERS = "GetReportFilters";
        public const string CURRENTRELEASE = "CurrentRelease";
		public const string DELIVERABLEBYID = "DeliverableByID";
		public const string ADDNEWPROJECT = "AddNewProject";
		public const string ADDDELIVERABLE = "AddDeliverable";
        public const string ROLEMENUMAPPING = "GetRoleMenuMapping";
        public const string DELIVERABLEUTIL = "DeliverableUtilization";
        public const string StageList = "GetStageList";
        public const string StateList = "GetStateList";
        public const string UPDATEDELIVERALBEDETAILS = "UpdateDeliverableDetail";
        public const string UPDATERESOURCEUTIL = "UpdateResourceUtilization"; 
		public const string PROJECTSBYID = "ProjectDeliverableDetail";
		public const string UPDATEPROJECTDETAIL = "ProjectUpdate";
		//public const string STAGELIST = "GetStageList";
		public const string STATELIST = "StateList";
		public const string DELIVERABLETRACKINGBYID = "DeliverableDetail";
		public const string DELIVERABLEDETAILSUPDATE = "DeliverableDetailUpdate";
		public const string GETUSERROLE = "GetUserRoleLooup";
        public const string ADDUSERROLE = "AddUserRole";
        public const string ROLELIST = "UserPermissionList";
		public const string ProjectResources = "GetProjectResources";
		public const string ADDNEWCLIENT = "AddNewClient";
		public const string CheckIsAdmin = "CheckIsAdmin";
        public const string ADDNEWCHECKLIST = "AddNewCheckList";
        public const string CHECKLIST = "GetAllCheckList";
        public const string ADDNEWCHECKLISTACOUNTLINK = "AddNewCheckListAcountLink";
        public const string GETCHECKLISTACOUNTLIST = "CheckAccountListData";
        public const string GETStateList = "GetStateLists";
        public const string UPDATECHECKLISTACOUNTLINK = "UpdateCheckListAcountLink";
        public const string UPDATECHECKLIST = "UpdateCheckList";
        public const string ADMINROLEPERMISSION = "AdminRolePermission";
        public const string GETADMINROLEPERMISSION = "GetAdminroleusers";
        public const string GETROLELIST = "GetRoleList";
        public const string ADDCHECKLISTMAPPING = "AddCheckListMapping";
        public const string UPDATECHECKLISTMAPPING = "CheckListMapping";
        public const string GETCHECKLIST = "GetAllCheckLists";
        #endregion
    }
    public static class MenuIDConstant

    {
        public const int ADDNEWPROJECT = 2;

        public const int ROLESANDPERMISSION = 7;

        public const int CHECKLIST = 0;
    }

}