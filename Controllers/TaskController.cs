using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_zespolowy.Models;
using System.Threading.Tasks;
using projekt_zespolowy.ViewModels;
using projekt_zespolowy.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace projekt_zespolowy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        ///<summary>
        ///Adds task
        ///</summary>
        /// <param name="task"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "taskName": "task1",
        ///        "taskDescription": "taskdescription1",
        ///        "taskStatus": 0,
        ///        "projectId": 3,
        ///        "userIds": [1,2,3]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Task created successfully</response>
        /// <response code="400">Wrong data provided</response>
        /// <response code="404">Project not found</response>
        [HttpPost("addtask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskVM taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = new projekt_zespolowy.Models.Task
            {
                TaskName = taskViewModel.TaskName,
                TaskDescription = taskViewModel.TaskDescription,
                TaskStatus = taskViewModel.TaskStatus,
                ProjectId = taskViewModel.ProjectId
            };

            var project = await _context.Projects.FindAsync(taskViewModel.ProjectId);
            if (project == null)
            {
                return NotFound($"Project with ID {taskViewModel.ProjectId} not found.");
            }

            List<UserDto> userDtos = new List<UserDto>();
            if (taskViewModel.UserIds != null && taskViewModel.UserIds.Count > 0)
            {
                var selectedUsers = await _context.Users
                    .Where(u => taskViewModel.UserIds.Contains(u.Id))
                    .ToListAsync();

                task.Users = selectedUsers;

                userDtos = selectedUsers.Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName
                }).ToList();
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskDto = new TaskDto
            {
                Id = task.Id,
                TaskName = task.TaskName,
                TaskDescription = task.TaskDescription,
                TaskStatus = task.TaskStatus,
                ProjectId = task.ProjectId,
                Users = userDtos
            };

            return Ok(taskDto);
        }

        ///<summary>
        ///Edits specific task
        ///</summary>
        /// <param name="task"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "taskName": "task1",
        ///        "taskDescription": "taskdescription1",
        ///        "taskStatus": 0,
        ///        "projectId": 3,
        ///        "userIds": [1,2,3]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Task updated successfully</response>
        /// <response code="400">Wrong data provided</response>
        /// <response code="404">Wrong data provided</response>
        [HttpPut("edittask/{id}")]
        public async Task<IActionResult> EditTask(int id, [FromBody] TaskVM taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            task.TaskName = taskViewModel.TaskName;
            task.TaskDescription = taskViewModel.TaskDescription;
            task.TaskStatus = taskViewModel.TaskStatus;

            if (taskViewModel.UserIds != null && taskViewModel.UserIds.Any())
            {
                var selectedUsers = await _context.Users
                    .Where(u => taskViewModel.UserIds.Contains(u.Id))
                    .ToListAsync();
                task.Users = selectedUsers;
            }

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            var updatedTaskDto = new TaskDto
            {
                Id = task.Id,
                TaskName = task.TaskName,
                TaskDescription = task.TaskDescription,
                TaskStatus = task.TaskStatus,
                Users = task.Users.Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName
                }).ToList()
            };

            return Ok(updatedTaskDto);
        }

        ///<summary>
        ///Deletes specific task
        ///</summary>
        /// <param name="task"></param>
        /// <response code="200">Task deleted successfully</response>
        /// <response code="404">Task not found</response>
        [HttpDelete("deletetask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ///<summary>
        ///Updates specific task status
        ///</summary>
        /// <param name="task"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "taskStatus": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Task status updated</response>
        /// <response code="400">Wrong data provided</response>
        /// <response code="404">Task not found</response>
        [HttpPut("updatetaskstatus/{id}")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatusUpdateVM updateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.TaskStatus = updateViewModel.TaskStatus;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            var updatedTaskDto = new TaskDto
            {
                Id = task.Id,
                TaskName = task.TaskName,
                TaskDescription = task.TaskDescription,
                TaskStatus = task.TaskStatus,
                Users = task.Users?.Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName
                }).ToList()
            };

            return Ok(updatedTaskDto);
        }

    }
}
