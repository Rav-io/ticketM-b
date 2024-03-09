using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketmanager.Models;
using System.Threading.Tasks;
using ticketmanager.ViewModels;
using ticketmanager.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;


namespace ticketmanager.Controllers
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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projectsUser = await _context.Projects
                .Include(p => p.UserProjects)
                .ToListAsync();

            var projects = projectsUser.Select(project => new ProjectUserDto
            {
                ProjectId = project.Id,
                ProjectName = project.ProjectName,
                UserIds = project.UserProjects.Select(up => up.UserId).ToList()
            }).ToList();

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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUsersToProject([FromBody] AssignUsersVM assignment)
        {
            var project = await _context.Projects.FindAsync(assignment.Id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            var userProjects = new List<UserProject>();

            foreach (int userId in assignment.UserId)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var userProject = new UserProject
                {
                    UserId = userId,
                    ProjectId = assignment.Id
                };

                userProjects.Add(userProject);
            }

            _context.UserProjects.AddRange(userProjects);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Users assigned to the project successfully" });
        }

        ///<summary>
        ///Unassigns user from project
        ///</summary>
        /// <param name="project"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "id": 1,
        ///        "userId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns message "Users unassigned from the project successfully"</response>
        /// <response code="404">Project not found / User not found</response>
        [Authorize]
        [HttpPost("unassign")]
        public async Task<IActionResult> UnassignUsersFromProject([FromBody] UnassignUserVM assignment)
        {
            var project = await _context.Projects.FindAsync(assignment.Id);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            var user = await _context.Users.FindAsync(assignment.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userProject = await _context.UserProjects.FirstOrDefaultAsync(up => up.ProjectId == assignment.Id && up.UserId == assignment.UserId);

            if (userProject == null)
            {
                return NotFound("User project not found");
            }

            _context.UserProjects.Remove(userProject);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User unassigned from the project successfully" });
        }

        ///<summary>
        ///Gets tasks for given project
        ///</summary>
        /// <param name="project"></param>
        /// <response code="200">Returns all tasks for given project</response>
        [Authorize]
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(int projectId)
        {
            var projectName = await _context.Projects
                .Where(p => p.Id == projectId)
                .Select(p => p.ProjectName)
                .FirstOrDefaultAsync();

            if (projectName == null)
            {
                return NotFound($"Project with ID {projectId} not found");
            }
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
                                          }).ToList(),
                                          CreationDate = t.CreationDate
                                      })
                                      .ToListAsync();
            var response = new
            {
                ProjectName = projectName,
                Tasks = tasks
            };

            return Ok(response);
        }

        ///<summary>
        ///Gets users assigned to given project
        ///</summary>
        /// <param name="project"></param>
        /// <response code="200">Returns all users assigned for given project</response>
        [Authorize]
        [HttpGet("{projectId}/users")]
        public async Task<IActionResult> GetUsersByProject(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.UserProjects)
                .ThenInclude(up => up.User)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return NotFound("Project not found");
            }

            var users = project.UserProjects.Select(up => new
            {
                UserId = up.UserId,
                UserName = up.User.UserName
            }).ToList();

            return Ok(users);
        }
    }
}
