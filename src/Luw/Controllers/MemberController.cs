using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Luw.Data;
using Luw.Models;
using Luw.Models.MemberViewModels;

namespace Luw.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Member
        public async Task<IActionResult> Index(string status = "Active")
        {
            var viewModel = new MemberIndexViewModel
            {
                Status = status,
                Members = new List<MemberIndexRow>()
            };

            IList<ApplicationUser> applicationUsers;
            var chapters = _context.Chapters.ToList();

            if (status == "All")
            {
                applicationUsers = _context.ApplicationUser.Include(a => a.Chapters).ToList();
            }
            else
            {
                applicationUsers = _context.ApplicationUser
                    .Where(a => a.Status == status)
                    .Include(a => a.Chapters).ToList();
            }
            foreach (var au in applicationUsers)
            {
                viewModel.Members.Add(new MemberIndexRow
                {
                    Id = au.Id,
                    FirstName = au.FirstName,
                    LastName = au.LastName,
                    City = au.City,
                    Status = au.Status,
                    Email = au.Email,
                    WhenExpires = au.WhenExpires,
                    Chapter = GetChapterNames(au.Chapters.ToList(), chapters)
                });
            }

            return View(viewModel);
        }
        
        private string GetChapterNames(List<MemberChapter> memberChapter, List<Chapter> chapters)
        {
            var name = "";
            var mc = memberChapter.Where(m => m.WhenLeft == null).ToList();
            if (mc.Count >= 1)
            {
                name = chapters.FirstOrDefault(c => c.Id == mc[0].ChapterId).Name;
            }
            if (mc.Count == 2)
            {
                name += ", " + chapters.FirstOrDefault(c => c.Id == mc[1].ChapterId).Name;
            }
            return name;
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var chapters = await _context.MemberChapters
                .Include(mc => mc.Chapter)
                .Where(mc => mc.ApplicationUserId == applicationUser.Id && mc.WhenLeft == null)
                .ToListAsync();

            var viewModel = new MemberDetailsViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                ZipCode = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                WhenExpires = applicationUser.WhenExpires,
                Notes = await _context.ApplicationUserNotes.Where(n => n.ApplicationUserId == applicationUser.Id).OrderByDescending(n => n.WhenAdded).ToListAsync(),
                Chapters = chapters
            };

            return View(viewModel);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            var model = new MemberCreateViewModel
            {
                State = "UT",
                Status = "Active",
                WhenJoined = DateTime.Now,
                WhenExpires = DateTime.Now.AddYears(1)
            };
            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = _context.Chapters.OrderBy(c => c.Name).ToList();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
            model.Chapters = chapterSelect;

            return View(model);
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City,Email,FirstName,LastName,State,Street1,Street2,Zip,Phone,Email,Note,WhenJoined,WhenExpires,Status,Chapter1,Chapter2")] MemberCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    City = viewModel.City,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    State = viewModel.State,
                    Street1 = viewModel.Street1,
                    ZipCode = viewModel.Zip,
                    PhoneNumber = viewModel.Phone,
                    WhenJoined = viewModel.WhenJoined,
                    WhenExpires = viewModel.WhenExpires,
                    Status = viewModel.Status
                };
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(viewModel.Note))
                {
                    var note = new ApplicationUserNote
                    {
                        ApplicationUserId = applicationUser.Id,
                        WhenAdded = DateTime.Now,
                        Note = viewModel.Note
                    };
                    _context.Add(note);
                }
                
                if (viewModel.Chapter1 != 0)
                {
                    _context.Add(new MemberChapter
                    {
                        ApplicationUserId = applicationUser.Id,
                        ChapterId = viewModel.Chapter1,
                        WhenJoined = DateTime.Now
                    });
                }
                if (viewModel.Chapter2 != 0)
                {
                    _context.Add(new MemberChapter
                    {
                        ApplicationUserId = applicationUser.Id,
                        ChapterId = viewModel.Chapter2,
                        WhenJoined = DateTime.Now
                    });
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = _context.Chapters.OrderBy(c => c.Name).ToList();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
            viewModel.Chapters = chapterSelect;

            return View(viewModel);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            
            var memberChapters = await _context.MemberChapters
                .Where(m => m.ApplicationUserId == id && m.WhenLeft == null).ToListAsync();

            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = await _context.Chapters.OrderBy(c => c.Name).ToListAsync();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }

            var viewModel = new MemberEditViewModel
            {

                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Street1 = applicationUser.Street1,
                Street2 = applicationUser.Street2,
                City = applicationUser.City,
                State = applicationUser.State,
                Zip = applicationUser.ZipCode,
                Phone = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WhenJoined = applicationUser.WhenJoined,
                Status = applicationUser.Status,
                WhenExpires = applicationUser.WhenExpires,
                Chapters = chapterSelect,
                Chapter1 = memberChapters.Count >= 1 ? memberChapters[0].ChapterId : 0,
                Chapter2 = memberChapters.Count == 2 ? memberChapters[1].ChapterId : 0
            };
            return View(viewModel);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,City,Email,FirstName,LastName,State,Street1,Street2,Zip,Phone,Email,Note,WhenJoined,WhenExpires,Status,Chapter1,Chapter2")] MemberEditViewModel viewModel)
        {

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == viewModel.Id);
            if (viewModel.Id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUser.City = viewModel.City;
                    applicationUser.Email = viewModel.Email;
                    applicationUser.FirstName = viewModel.FirstName;
                    applicationUser.LastName = viewModel.LastName;
                    applicationUser.Street1 = viewModel.Street1;
                    applicationUser.Street2 = viewModel.Street2;
                    applicationUser.City = viewModel.City;
                    applicationUser.State = viewModel.State;
                    applicationUser.ZipCode = viewModel.Zip;
                    applicationUser.Email = viewModel.Email;
                    applicationUser.PhoneNumber = viewModel.Phone;
                    applicationUser.Status = viewModel.Status;
                    applicationUser.WhenJoined = viewModel.WhenJoined;
                    applicationUser.WhenExpires = viewModel.WhenExpires;
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(viewModel.Note))
                    {
                        var note = new ApplicationUserNote
                        {
                            ApplicationUserId = applicationUser.Id,
                            WhenAdded = DateTime.Now,
                            Note = viewModel.Note
                        };
                        _context.Add(note);
                        await _context.SaveChangesAsync();
                    }

                    var memberChapters = _context.MemberChapters
                        .Where(m => m.ApplicationUserId == viewModel.Id && m.WhenLeft == null).ToList();
                    foreach (var mc in memberChapters)                    {
                        if (!(mc.ChapterId == viewModel.Chapter1 || mc.ChapterId == viewModel.Chapter2))
                        {
                            mc.WhenLeft = DateTime.Now;
                        }
                    }
                    if (viewModel.Chapter1 != 0)
                    {
                        if (!memberChapters.Any(m => m.ChapterId == viewModel.Chapter1))
                        {
                            _context.Add(new MemberChapter
                            {
                                ApplicationUserId = viewModel.Id,
                                ChapterId = viewModel.Chapter1,
                                WhenJoined = DateTime.Now
                            });
                        }
                    }
                    if (viewModel.Chapter2 != 0)
                    {
                        if (!memberChapters.Any(m => m.ChapterId == viewModel.Chapter2))
                        {
                            _context.Add(new MemberChapter
                            {
                                ApplicationUserId = viewModel.Id,
                                ChapterId = viewModel.Chapter2,
                                WhenJoined = DateTime.Now
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            var chapterSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "None" }
            };
            var chapters = await _context.Chapters.OrderBy(c => c.Name).ToListAsync();
            foreach (var chapter in chapters)
            {
                chapterSelect.Add(new SelectListItem
                {
                    Value = chapter.Id.ToString(),
                    Text = chapter.Name
                });
            }
            viewModel.Chapters = chapterSelect;
            return View(applicationUser);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
