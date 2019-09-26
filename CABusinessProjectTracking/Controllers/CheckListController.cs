using BusinessProjectTracking.Helper;
using BusinessProjectTracking.Models;
using Newtonsoft.Json;
using PT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessProjectTracking.Controllers
{
    public class CheckListController : Controller
    {
      
        // GET: CheckList
        public ActionResult Index()
        {
            RouteAuthenticaion routeAuthenticaion = new RouteAuthenticaion();
            
            if (!routeAuthenticaion.IsAuthorizedPage(MenuIDConstant.CHECKLIST))
            {
                return RedirectToAction("UnAuthorized", "Home");
            }
            else
            {
                string queryString = "?UserID=" + HttpContext.User.Identity.Name;
                List<CheckList> response = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckList>>(queryString, CABPTMethodConstants.CHECKLIST);
                ViewBag.RoleList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<Role>>(queryString, CABPTMethodConstants.GETROLELIST);
                return View(response);
            }
            
        }
        public JsonResult AddCheckList(CheckList rlst)
        {
             CheckList checkList = new CheckList
             {
                UserID = HttpContext.User.Identity.Name,
                Description = rlst.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsActive = true,
                RoleId = rlst.RoleId
                
            
            };

            int response = CABusinessProjectTrackingAPIClient.GetHttpResponse<int>(checkList, CABPTMethodConstants.ADDNEWCHECKLIST);

            string queryString = "?UserID=" + HttpContext.User.Identity.Name;

            List<CheckList> checkListResponse = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckList>>(queryString, CABPTMethodConstants.CHECKLIST);

            string result = JsonConvert.SerializeObject(checkListResponse);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCheckList()
        {
            string queryString = "?UserID=" + HttpContext.User.Identity.Name;

            List<CheckList> checkListResponse = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckList>>(queryString, CABPTMethodConstants.CHECKLIST);

            string result = JsonConvert.SerializeObject(checkListResponse);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllActiveCheckList(int RoleId = 1, int StageID = 1,int ProjectId = 0)
        {
            string queryString = "?UserID=UserName&ProjectId=" + ProjectId +"&StageId="+ StageID;

            queryString=queryString.Replace("UserName", HttpContext.User.Identity.Name);

            List<CheckList> checkListResponse = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckList>>(queryString, CABPTMethodConstants.CHECKLIST);
  
            if (checkListResponse != null)
            {
                string result = JsonConvert.SerializeObject(checkListResponse.Where(x => x.IsActive && x.RoleId == RoleId));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

           
        }
        public JsonResult GetAllRoleList()
        {
            string queryString = "?UserID=" + HttpContext.User.Identity.Name;

            List<Role> roleListResponse = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<Role>>(queryString, CABPTMethodConstants.GETROLELIST);

            string result = JsonConvert.SerializeObject(roleListResponse);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCheckListData(int ProjectId)
        {
            string queryString = "?UserID=UserName&ProjectId=" + ProjectId ;

            queryString = queryString.Replace("UserName", HttpContext.User.Identity.Name);

            List<CheckListDetail> checkListResponse = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckListDetail>>(queryString, CABPTMethodConstants.GETCHECKLIST);

            string result = JsonConvert.SerializeObject(checkListResponse);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveCheckListMapping(CheckListMappingList checkListMapping)
        {
            string[] CheckListIDs = checkListMapping.CheckListIDS;

            for (int i = 0; i < CheckListIDs.Length; i++)
            {
               
                CheckListMapping checkListAccount = new CheckListMapping
                {
                    UserID = HttpContext.User.Identity.Name,
                    CheckListId = Convert.ToInt32(CheckListIDs[i]),
                    ProjectId = Convert.ToInt32(checkListMapping.ProjectId.ToString()),
                    StageId = Convert.ToInt32(checkListMapping.StageId.ToString()),
                    RoleId = Convert.ToInt32(checkListMapping.RoleId.ToString()),
                    CreatedBy = HttpContext.User.Identity.Name, 
                    CreatedDate =DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsActive = true             
                };
                if(i==0)
                {
                    bool response_ = CABusinessProjectTrackingAPIClient.GetHttpResponse<bool>(checkListAccount, CABPTMethodConstants.UPDATECHECKLISTMAPPING);
                }
                bool response = CABusinessProjectTrackingAPIClient.GetHttpResponse<bool>(checkListAccount, CABPTMethodConstants.ADDCHECKLISTMAPPING);

                string result = JsonConvert.SerializeObject("");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveCheckListAccount(CheckListAccountLink checkListAccountLink)
        {
            string[] CheckListIDs = checkListAccountLink.CheckListIDS;

            for (int i = 0; i < CheckListIDs.Length; i++)
            {
                Console.WriteLine(CheckListIDs[i]);
           
                CheckListAccount checkListAccount = new CheckListAccount
                {
                    UserID = HttpContext.User.Identity.Name,
                    CheckListID = Convert.ToInt32(CheckListIDs[i].ToString()),
                    DeliverableID = Convert.ToInt32(checkListAccountLink.DeliverableID.ToString()),
                    DeliverableDetailID = Convert.ToInt32(checkListAccountLink.DeliverableDetailID.ToString()),
                    ProjectID = Convert.ToInt32(checkListAccountLink.ProjectID.ToString()),
                    StageID = Convert.ToInt32(checkListAccountLink.Stage.ToString()),
                    StatusID = Convert.ToInt32(checkListAccountLink.State.ToString()),
                    ResourceEmail = checkListAccountLink.ResourceName,
                    Notes = "",
                };

                bool response = CABusinessProjectTrackingAPIClient.GetHttpResponse<bool>(checkListAccount, CABPTMethodConstants.ADDNEWCHECKLISTACOUNTLINK); string result = JsonConvert.SerializeObject("");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetCheckListAccountLink(pdDTO del)
        //{
        //    CheckListAccountResponse checkListAccountResponse = new CheckListAccountResponse();

        //    string queryString = "?UserID=" + HttpContext.User.Identity.Name;

        //    checkListAccountResponse.CheckListAccountList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckListAccount>>(queryString, CABPTMethodConstants.GETCHECKLISTACOUNTLIST);

        //    checkListAccountResponse.StateList= CABusinessProjectTrackingAPIClient.GetHttpResponse<List<State>>(queryString, CABPTMethodConstants.GETStateList);

        //    if(checkListAccountResponse.CheckListAccountList !=null && checkListAccountResponse.CheckListAccountList.Count > 0)
        //    {
        //        checkListAccountResponse.CheckListAccountList = checkListAccountResponse.CheckListAccountList.ToList().Where(x => x.ProjectID == del.pid && x.DeliverableID == del.did && x.DeliverableDetailID == del.dtlid).ToList();
        //    }

        //    string result = JsonConvert.SerializeObject(checkListAccountResponse);


        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

      

        public JsonResult GetCheckListAccountLink(CheckListAccount checkListAccount)
        {
            CheckListAccountResponse checkListAccountResponse = new CheckListAccountResponse();

            string queryString = "?UserID=" + HttpContext.User.Identity.Name;

            checkListAccount.UserID = HttpContext.User.Identity.Name;
            checkListAccount.CreatedDate = DateTime.Now;
            checkListAccount.UpdatedDate = DateTime.Now;
            checkListAccount.SignedDate = DateTime.Now;
            checkListAccountResponse.CheckListAccountList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<CheckListAccount>>(checkListAccount, CABPTMethodConstants.GETCHECKLISTACOUNTLIST);

            checkListAccountResponse.StateList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<State>>(queryString, CABPTMethodConstants.GETStateList);

            if (checkListAccountResponse.CheckListAccountList != null && checkListAccountResponse.CheckListAccountList.Count > 0)
            {
                checkListAccountResponse.CheckListAccountList = checkListAccountResponse.CheckListAccountList.ToList().Where(x => x.ProjectID == checkListAccount.ProjectID && x.DeliverableID == checkListAccount.DeliverableID && x.DeliverableDetailID == checkListAccount.DeliverableDetailID).ToList();
            }

            string result = JsonConvert.SerializeObject(checkListAccountResponse);


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCheckListAccountLink(List<CheckListAccount> checkListAccountList)
        {
            string result = JsonConvert.SerializeObject("");

           bool response = CABusinessProjectTrackingAPIClient.GetHttpResponse<bool>(checkListAccountList, CABPTMethodConstants.UPDATECHECKLISTACOUNTLINK);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCheckList(CheckList checkList)
        {
            checkList = new CheckList
            {
                UserID = HttpContext.User.Identity.Name,
                CheckListID = checkList.CheckListID,
                Description = checkList.Description,
                UpdatedDate = DateTime.Now,
                IsActive = checkList.IsActive
            };

            bool response = CABusinessProjectTrackingAPIClient.GetHttpResponse<bool>(checkList, CABPTMethodConstants.UPDATECHECKLIST);           

            string result = JsonConvert.SerializeObject(response);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}