using Microsoft.AspNetCore.Mvc;
using CourseManagement.ViewModels;
using CourseManagement.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseManagement.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ILessonsService _lessonsService;
        private readonly ICoursesService _coursesService;
        int PAGESIZE = 10;
        public LessonsController(ILessonsService lessonsService, ICoursesService coursesService)
        {
            _lessonsService = lessonsService;
            _coursesService = coursesService;
        }

        // GET: Lessons
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? courseId, int? pageNumber)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("title") ? "title_desc" : "";
            ViewData["DateCreatedSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("date_created") ? "date_created_desc" : "date_created";

            var courses = await _coursesService.GetAll();
            ViewData["CourseId"] = courses.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString(),
                Selected = courseId.HasValue && courseId == x.Id
            });
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            return View(await _lessonsService.GetAllFilter(sortOrder, currentFilter, searchString, courseId, pageNumber, PAGESIZE));
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _lessonsService.GetById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        // GET: Lessons/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CourseId"] = new SelectList(await _coursesService.GetAll(), "Id", "Title");
            return View();
        }

        // POST: Lessons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonRequest request)
        {
            if (ModelState.IsValid)
            {
                await _lessonsService.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonsService.GetById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(await _coursesService.GetAll(), "Id", "Title");
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonViewModel lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _lessonsService.Update(lesson);
                return RedirectToAction(nameof(Index));
            }
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _lessonsService.GetById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _lessonsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
