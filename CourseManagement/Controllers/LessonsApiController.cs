using CourseManagement.Services;
using CourseManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsApiController : ControllerBase
    {
        private readonly ILessonsService _lessonService;

        public LessonsApiController(ILessonsService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] LessonRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _lessonService.Create(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _lessonService.GetById(id);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFilter(string? sortOrder, string? currentFilter, string? searchString, int? courseId, int? pageNumber, int pageSize = 10)
        {
            var result = await _lessonService.GetAllFilter(sortOrder!, currentFilter!, searchString!, courseId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _lessonService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] LessonViewModel request)
        {
            var result = await _lessonService.Update(request);
            return Ok(result);
        }
    }
}