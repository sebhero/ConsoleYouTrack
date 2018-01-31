using YouTrack.Rest.Exceptions;
using YouTrack.Rest.Factories;
using YouTrack.Rest.Requests.Projects;

namespace YouTrack.Rest.Repositories
{
    class ProjectRepository : IProjectRepository
    {
        private readonly IConnection connection;
        private readonly IProjectFactory projectFactory;

        public ProjectRepository(IConnection connection, IProjectFactory projectFactory)
        {
            this.connection = connection;
            this.projectFactory = projectFactory;
        }

        public IProject GetProject(string projectId)
        {
            return projectFactory.CreateProject(projectId, connection);
        }

        public IProject CreateProject(string projectId, string projectName, string projectLeadLogin, int startingNumber = 1, string description = null)
        {
            connection.Put(new CreateNewProjectRequest(projectId, projectName, projectLeadLogin, startingNumber, description));

            return GetProject(projectId);
        }

        public bool ProjectExists(string projectId)
        {
            //Relies on the "not found" exception if project doesn't exist. Could use some improving.

            try
            {
                connection.Get(new GetProjectRequest(projectId));

                return true;
            }
            catch (RequestNotFoundException)
            {
                return false;
            }
        }

        public void DeleteProject(string projectid)
        {
            connection.Delete(new DeleteProjectRequest(projectid));
        }
    }
}