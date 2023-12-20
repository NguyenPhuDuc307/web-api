using System.Drawing.Printing;
using System.Net.Http.Headers;
using AutoMapper;
using CourseManagement.Data;
using CourseManagement.Data.Entities;
using CourseManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Services
{
    public class LessonsService : ILessonsService
    {
        private readonly CourseDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public LessonsService(CourseDbContext context, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> Create(LessonRequest request)
        {
            var lesson = _mapper.Map<Lesson>(request);

            // Save image file
            if (request.Image != null)
            {
                lesson.ImagePath = await SaveFile(request.Image);
            }
            _context.Add(lesson);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                if (!string.IsNullOrEmpty(lesson.ImagePath))
                    await _storageService.DeleteFileAsync(lesson.ImagePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                _context.Lessons.Remove(lesson);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<LessonViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? courseId, int? pageNumber, int pageSize)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var lessons = from m in _context.Lessons select m;

            if (courseId != null)
            {
                lessons = lessons.Where(s => s.CourseId == courseId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                lessons = lessons.Where(s => s.Title!.Contains(searchString)
                || s.Introduction!.Contains(searchString));
            }

            lessons = sortOrder switch
            {
                "title_desc" => lessons.OrderByDescending(s => s.Title),
                "date_created" => lessons.OrderBy(s => s.DateCreated),
                "date_created_desc" => lessons.OrderByDescending(s => s.DateCreated),
                _ => lessons.OrderBy(s => s.Title),
            };

            return PaginatedList<LessonViewModel>.Create(_mapper.Map<IEnumerable<LessonViewModel>>(await lessons.ToListAsync()), pageNumber ?? 1, pageSize);
        }

        public async Task<LessonViewModel> GetById(int id)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<LessonViewModel>(lesson);
        }

        public async Task<int> Update(LessonViewModel request)
        {
            if (!LessonExists(request.Id))
            {
                throw new Exception("Lesson does not exist");
            }
            // Save image file
            if (request.Image != null)
            {
                if (!string.IsNullOrEmpty(request.ImagePath))
                    await _storageService.DeleteFileAsync(request.ImagePath.Replace("/" + USER_CONTENT_FOLDER_NAME + "/", ""));
                request.ImagePath = await SaveFile(request.Image);
            }

            _context.Update(_mapper.Map<Lesson>(request));
            return await _context.SaveChangesAsync();
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}