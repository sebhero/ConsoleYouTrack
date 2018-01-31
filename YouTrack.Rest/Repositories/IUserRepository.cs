using System.Collections.Generic;

namespace YouTrack.Rest.Repositories
{
    public interface IUserRepository
    {
        void CreateUser(string login, string password, string email, string fullname = null);
        void DeleteUser(string login);
        bool UserExists(string login);
        IUser GetUser(string login);
        IUserGroup CreateUserGroup(string userGroupName);
        IEnumerable<IUserGroup> GetUserGroups();
        void DeleteUserGroup(string userGroupName);
    }
}