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
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        ///<summary>
        ///Gets all projects
        ///</summary>
        ///<returns>List of projects</returns>
        ///<response code="200">Returns list of projects</response>
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _context.Projects.ToListAsync();

            return Ok(projects);
        }

        ///<summary>
        ///Creates project
        ///</summary>
        /// <param name="project"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "ProjectName": "project1"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns message "Project created successfully"</response>
        /// <response code="400">Invalid data submitted</response>
        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectVM project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectEntity = new Project
            {
                ProjectName = project.ProjectName
            };

            _context.Projects.Add(projectEntity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Project created successfully" });
        }

        ///<summary>
        ///Edits project
        ///</summary>
        /// <param name="project"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "id": 1,
        ///        "ProjectName": "project1"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns message "Project updated successfully"</response>
        /// <response code="404">Project not found</response>
        [HttpPut("edit")]
        public async Task<IActionResult> EditProject([FromBody] ProjectUpdateVM projectDTO)
        {
            var existingProject = await _context.Projects.FindAsync(projectDTO.Id);

            if (existingProject == null)
            {
                return NotFound("Project not found");
            }

            existingProject.ProjectName = projectDTO.ProjectName;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Project updated successfully" });
        }

        ///<summary>
        ///Deletes project
        ///</summary>
        /// <param name="project"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns message "Project deleted successfully"</response>
        /// <response code="404">Project not found</response>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProject([FromBody] ProjectDeleteVM projectDTO)
        {
            var project = await _context.Projects.FindAsync(projectDTO.Id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Project deleted successfully" });
        }

        ///<summary>
        ///Assigns users to project
        ///</summary>
        /// <param name="project"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "id": 1,
        ///        "userId": [1,2,3]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns message "Users assigned to the project successfully"</response>
        /// <response code="404">Project not found / User not found</response>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUsersToProject([FromBody] AssignUsersVM assignment)
        {
            var project = await _context.Projects.FindAsync(assignment.Id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            foreach (int userId in assignment.UserId)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (project.Users == null)
                {
                    project.Users = new List<User>();
                }

                project.Users.Add(user);
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Users assigned to the project successfully" });
        }

        ///<summary>
        ///Gets tasks for given project
        ///</summary>
        /// <param name="project"></param>
        /// <response code="200">Returns all tasks for given project</response>
        /// <response code="404">Returns message "No tasks found for the given project"</response>
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(int projectId)
        {
            var tasks = await _context.Tasks
                                      .Where(t => t.ProjectId == projectId)
                                      .Include(t => t.Users)
                                      .Select(t => new TaskDto
                                      {
                                          Id = t.Id,
                                          TaskName = t.TaskName,
                                          TaskDescription = t.TaskDescription,
                                          TaskStatus = t.TaskStatus,
                                          Users = t.Users.Select(u => new UserDto
                                          {
                                              Id = u.Id,
                                              UserName = u.UserName
                                          }).ToList()
                                      })
                                      .ToListAsync();

            var projectName = await _context.Projects
                .Where(p => p.Id == projectId)
                .Select(p => p.ProjectName)
                .FirstOrDefaultAsync();

            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found for the given project.");
            }

            var response = new
            {
                ProjectName = projectName,
                Tasks = tasks
            };

            return Ok(response);
        }
    }
}
