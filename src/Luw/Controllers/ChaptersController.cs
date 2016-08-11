using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Luw.Data;
using Luw.Models;
using Luw.Models.ChapterViewModels;

namespace Luw.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChaptersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Chapters
        public async Task<IActionResult> Index()
        {
           var model = await _context.Chapters.ToListAsync();

            var viewModel = new List<IndexViewModel>();
            foreach (var c in model)
            {
                viewModel.Add(new IndexViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    MeetingInfo = BuildMeetingInfo(c.MeetingWeek, c.MeetingDay, c.StartTime, c.EndTime),
                    City = c.City
                });
            }
            return View(viewModel);
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            var viewModel = new DetailsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                SubName = model.SubName,
                Description = model.Description,
                Venue = model.Venue,
                Street1 = model.Street1,
                Street2 = model.Street2,
                City = model.City,
                Zip = model.Zip,
                MeetingInfo = BuildMeetingInfo(model.MeetingWeek, model.MeetingDay, model.StartTime, model.EndTime),
                Url = model.Url,
                Email = model.Email,
                Phone = model.Phone,
                Notes = model.Notes
            };

            return View(viewModel);
        }

        // GET: Chapters/Create
        public IActionResult Create()
        {
            var viewModel = new Chapter
            {
                State = "UT"
            };
            return View(viewModel);
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Description,Email,EndTime,MeetingDay,MeetingWeek,Name,Notes,Phone,StartTime,State,Street1,Street2,SubName,Url,Venue,Zip")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Description,Email,EndTime,MeetingDay,MeetingWeek,Name,Notes,Phone,StartTime,State,Street1,Street2,SubName,Url,Venue,Zip")] Chapter chapter)
        {
            if (id != chapter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.Id))
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
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapters.SingleOrDefaultAsync(m => m.Id == id);
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.Id == id);
        }

        private string BuildMeetingInfo(int? weekOfMonth, int? dayOfWeek, string startTime, string endTime)
        {
            string retVal = "";

            string week = ConvertToWeekNumber(weekOfMonth);
            string day = ConvertToDayOfWeek(dayOfWeek);
            string time = ConvertMeetingTime(startTime, endTime);

            retVal = week + day + ConvertMeetingTime(startTime, endTime);

            return retVal;
        }

        private string ConvertMeetingTime(string startTime, string endTime)
        {
            var retVal = startTime;
            
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                retVal += "-";
            }
            retVal += endTime;
            return retVal;
        }
        
        private string ConvertToWeekNumber(int? weekOfMonth)
        {
            switch (weekOfMonth)
            {
                case 1:
                    return "First ";
                case 2:
                    return "Second ";
                case 3:
                    return "Third ";
                case 4:
                    return "Fourth ";
                default:
                    return "";
            }
        }

        private string ConvertToDayOfWeek(int? dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return "Sunday ";
                case 2:
                    return "Monday ";
                case 3:
                    return "Tuesday ";
                case 4:
                    return "Wednesday ";
                case 5:
                    return "Thursday ";
                case 6:
                    return "Friday ";
                case 7:
                    return "Saturday ";
                default:
                    return "";
            }
        }
    }
}
