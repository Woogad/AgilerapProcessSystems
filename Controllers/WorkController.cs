using AgilerapProcessSystems.Common;
using AgilerapProcessSystems.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AgilerapProcessSystems.Controllers
{
    public class WorkController : Controller
    {

        private Agile_Process_SystemsContext db = new Agile_Process_SystemsContext();
        private static List<SelectListItem> _StatusDropdownList = new List<SelectListItem>();
        private static List<SelectListItem> _UserDropdownList = new List<SelectListItem>();

        public async Task<IActionResult> Index()
        {
            //! Get SessionUser
            User? user = await GetSessionUser();

            //! If User Null return to Login
            if (user == null) return RedirectToAction("Login", "Home");

            ViewBag.UserName = user.Name;
            var workDBList = await GetDBWork();
            return View(workDBList);

        }

        public async Task<IActionResult> Create()
        {
            ClearStatic();
            await InitStatic();

            //! Get SessionUser
            User? user = await GetSessionUser();
            //! If User Null return to Login
            if (user == null) return RedirectToAction("Login", "Home");

            var workDBList = await GetDBWork();
            Work work = new Work() { IsDeleted = false, CreateDate = DateTime.Now, CreateByID = user.ID, UpdateByID = user.ID };
            workDBList.Add(work);
            ViewbagData();
            return View(workDBList.Where(s => s.IsDeleted == false));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Work work)
        {
            Status statusDefault = await db.Status.Where(s => s.StatusName == work.statusDefaultNoDueDate).FirstOrDefaultAsync();
            work.Provider = new List<Provider>();

            if (work.DueDate == null)
            {
                work.StatusID = statusDefault.ID;
            }

            if (work.StatusID == statusDefault.ID && work.DueDate != null)
            {
                Status status2Default = await db.Status.Where(s => s.StatusName == work.statusDefaultWithDueDate).FirstOrDefaultAsync();
                work.StatusID = status2Default.ID;
            }

            bool hasProvider = false;
            if (work.GroupProvider != null || work.IsSelectedAll)
            {
                hasProvider = true;
            }
            if (hasProvider)
            {
                if (work.IsSelectedAll)
                {
                    foreach (var i in await db.User.ToListAsync())
                    {
                        Provider provider = new Provider()
                        {
                            CreateByID = work.CreateByID, //! Change later for user login
                            UpdateByID = work.UpdateByID,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            IsDelete = false,
                            UserID = i.ID
                        };
                        work.Provider.Add(provider);
                        db.Provider.Add(provider);
                    }
                }
                else
                {
                    List<string> providerIDList = work.GroupProvider.Split(',').ToList();
                    foreach (var i in providerIDList)
                    {
                        if (Int32.TryParse(i, out int id))
                        {
                            Provider provider = new Provider()
                            {
                                CreateByID = work.CreateByID,
                                UpdateByID = work.UpdateByID,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                IsDelete = false,
                                UserID = id
                            };
                            db.Provider.Add(provider);
                            work.Provider.Add(provider);
                        }
                    }
                }
            }

            WorkLog workLog = new WorkLog()
            {
                No = 1,
                Project = work.Project,
                Name = work.Name,
                DueDate = work.DueDate,
                StatusID = work.StatusID,
                Remark = work.Remark,
                CreateByID = work.CreateByID,
                UpdateByID = work.UpdateByID,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsDelete = false
            };
            workLog.ProviderLog = new List<ProviderLog>();
            work.WorkLog = new List<WorkLog>()
            {
                workLog
            };
            foreach (var i in work.Provider)
            {
                ProviderLog providerLog = new ProviderLog()
                {
                    UserID = i.UserID,
                    CreateByID = i.CreateByID,
                    UpdateByID = i.UpdateByID,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDelete = false
                };
                workLog.ProviderLog.Add(providerLog);
                db.ProviderLog.Add(providerLog);
            }
            db.Work.Add(work);
            db.WorkLog.Add(workLog);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //! Get SessionUser
            User? user = await GetSessionUser();
            //! If User Null return to Login
            if (user == null) return RedirectToAction("Login", "Home");

            ClearStatic();
            await InitStatic();
            if (id == null)
            {
                return NotFound();
            }
            var workDBList = await GetDBWork();

            var workEditItem = workDBList.Where(s => s.ID == id).First();

            //! UpdateByID to current User that Editing
            workEditItem.UpdateByID = user.ID;

            ViewBag.EditID = id;

            if (workEditItem.Provider != null)
            {
                foreach (var i in workEditItem.Provider.Where(s => s.IsDelete == false))
                {
                    foreach (var j in _UserDropdownList)
                    {
                        if (i.UserID.ToString() == j.Value)
                        {
                            j.Selected = true;
                            continue;
                        }
                    }
                }
            }

            ViewbagData();
            return View(workDBList);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Work work)
        {
            bool hasModifyProvider = false;
            if (work.GroupProvider != null || work.IsSelectedAll)
            {
                hasModifyProvider = true;
            }
            if (hasModifyProvider)
            {
                if (work.IsSelectedAll)
                {
                    work.GroupProvider = "";
                    var userDBList = await db.User.ToListAsync();
                    foreach (var j in userDBList)
                    {
                        if (j == userDBList.Last())
                        {
                            work.GroupProvider += j.ID;
                        }
                        else
                        {
                            work.GroupProvider += j.ID + ",";
                        }
                    }
                }
                List<string> providerIDList = work.GroupProvider.Split(',').ToList();
                foreach (var j in work.Provider)
                {
                    bool isExist = false;
                    foreach (var i in providerIDList)
                    {
                        if (Int32.TryParse(i, out int id))
                        {
                            if (j.UserID == id)
                            {
                                isExist = true;
                                if (j.IsDelete)
                                {
                                    db.Entry(j).State = EntityState.Modified;
                                    j.UpdateDate = DateTime.Now;
                                    j.IsDelete = false;
                                }
                                providerIDList.Remove(i);
                                break;
                            }
                        }
                    }

                    if (isExist == false)
                    {
                        db.Entry(j).State = EntityState.Modified;
                        j.IsDelete = true;
                        j.UpdateDate = DateTime.Now;
                    }
                }

                if (providerIDList.Count > 0)
                {
                    foreach (var i in providerIDList)
                    {
                        if (Int32.TryParse(i, out int id))
                        {
                            Provider provider = new Provider()
                            {
                                CreateByID = work.UpdateByID,
                                UpdateByID = work.UpdateByID,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                IsDelete = false,
                                UserID = id
                            };
                            db.Provider.Add(provider);
                            work.Provider.Add(provider);
                        }
                    }
                }
            }
            else
            {
                Work oldWork = await db.Work.FindAsync(work.ID);
                if (work.IsEqual(oldWork, true))
                {
                    return RedirectToAction("Index");
                }
                db.ChangeTracker.Clear();
            }

            db.Entry(work).State = EntityState.Modified;
            work.UpdateDate = DateTime.Now;


            var workLogDBList = await db.WorkLog.Where(s => s.WorkID == work.ID).Include(s => s.ProviderLog).ToListAsync();
            WorkLog workLog = new WorkLog()
            {
                WorkID = work.ID,
                Project = work.Project,
                Name = work.Name,
                No = workLogDBList.Last().No + 1,
                DueDate = work.DueDate,
                StatusID = work.StatusID,
                Remark = work.Remark,
                CreateByID = work.UpdateByID,
                UpdateByID = work.UpdateByID,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsDelete = work.IsDeleted
            };
            workLog.ProviderLog = new List<ProviderLog>();
            if (work.Provider != null)
            {
                foreach (var i in work.Provider)
                {
                    ProviderLog providerLog = new ProviderLog()
                    {
                        UserID = i.UserID,
                        CreateByID = work.UpdateByID,
                        UpdateByID = work.UpdateByID,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        IsDelete = i.IsDelete
                    };
                    workLog.ProviderLog.Add(providerLog);
                    db.ProviderLog.Add(providerLog);
                }
            }
            else
            {
                foreach (var i in workLog.ProviderLog)
                {
                    ProviderLog providerLog = new ProviderLog()
                    {
                        UserID = i.UserID,
                        CreateByID = work.UpdateByID,
                        UpdateByID = work.UpdateByID,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        IsDelete = false
                    };
                    workLog.ProviderLog.Add(providerLog);
                    db.ProviderLog.Add(providerLog);
                }
            }
            db.WorkLog.Add(workLog);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> History(int? id)
        {
            ViewBag.HistoryID = id;
            if (id == null)
            {
                return NotFound();
            }
            var work = await db.Work
                .Include(s => s.WorkLog).ThenInclude(s => s.ProviderLog).ThenInclude(s => s.User)
                .Include(s => s.WorkLog).ThenInclude(s => s.Status)
                .Include(s => s.WorkLog).ThenInclude(s => s.UpdateBy)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (work == null)
            {
                return NoContent();
            }
            var workDBList = await GetDBWork();
            if (work.WorkLog.Count < 2)
            {
                ViewBag.HistoryText = "Project " + work.Project + " has no changed";
                return View(workDBList);
            }

            for (int i = 1; i < work.WorkLog.Count; i++)
            {
                WorkLog workLogNext = GetWorkLogNextByIndex(i, work);
                work.WorkLog.ToList()[i - 1].WorkLogChangeNext = workLogNext;
            }

            var reverseWorkLogList = work.WorkLog.Reverse().ToList();
            work.WorkLog = reverseWorkLogList;
            workDBList[workDBList.FindIndex(s => s.ID == id)] = work;
            return View(workDBList);
        }

        private async Task InitStatic()
        {
            foreach (var i in await db.Status.ToListAsync())
            {
                _StatusDropdownList.Add(new SelectListItem() { Text = i.StatusName, Value = i.ID.ToString() });
            }
            foreach (var i in await db.User.ToListAsync())
            {
                _UserDropdownList.Add(new SelectListItem() { Text = i.Name, Value = i.ID.ToString() });
            }
        }

        private void ClearStatic()
        {
            _StatusDropdownList.Clear();
            _UserDropdownList.Clear();
        }

        private void ViewbagData()
        {
            ViewBag.StautsDropdownList = _StatusDropdownList;
            ViewBag.UserDropdownList = _UserDropdownList;
        }

        private async Task<List<Work>> GetDBWork()
        {
            return await db.Work.Where(s => s.IsDeleted == false)
               .Include(s => s.Provider).ThenInclude(inc => inc.User)
               .Include(s => s.Status)
               .Include(s => s.CreateBy)
               .ToListAsync();
        }

        private WorkLog GetWorkLogNextByIndex(int index, Work workForLog)
        {
            WorkLog tempWorkLogNext = new WorkLog()
            {
                Project = workForLog.WorkLog.ToList()[index].Project,
                Name = workForLog.WorkLog.ToList()[index].Name,
                DueDate = workForLog.WorkLog.ToList()[index].DueDate,
                UpdateByID = workForLog.WorkLog.ToList()[index].UpdateByID,
                UpdateBy = workForLog.WorkLog.ToList()[index].UpdateBy,
                Status = workForLog.WorkLog.ToList()[index].Status,
                StatusID = workForLog.WorkLog.ToList()[index].StatusID,
                Remark = workForLog.WorkLog.ToList()[index].Remark,
                ProviderLog = workForLog.WorkLog.ToList()[index].ProviderLog
            };
            return tempWorkLogNext;
        }

        private async Task<User?> GetSessionUser()
        {
            //! Check is UserSession has Value?
            if (HttpContext.Session.GetString(Agile.SessionName.UserSession.ToString()) != null)
            {
                //! Get Value from UserSession and Find User
                string? userSession = HttpContext.Session.GetString(Agile.SessionName.UserSession.ToString());
                User? user = await db.User.Where(s => s.Email == userSession).FirstOrDefaultAsync();
                return user;
            }
            else
            {
                //! Else return null
                return null;
            }
        }
    }
}
