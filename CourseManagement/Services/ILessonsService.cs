using CourseManagement.ViewModels;

namespace CourseManagement.Services
{
    public interface ILessonsService
    {
        Task<PaginatedList<LessonViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? courseId, int? pageNumber, int pageSize);
        Task<LessonViewModel> GetById(int id);
        Task<int> Create(LessonRequest request);
        Task<int> Update(LessonViewModel request);
        Task<int> Delete(int id);
    }
}