using System.Collections.Generic;

namespace YouTrack.Rest
{
    public interface ISession
    {
        bool IsAuthenticated { get; }
        IDictionary<string, string> AuthenticationCookies { get; }
        void Login();
    }
}