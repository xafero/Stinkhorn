using FubarDev.FtpServer.AccountManagement;
using System.Collections.Generic;

namespace Stinkhorn.VFS.API
{
    class FixedMemberProvider : IMembershipProvider
    {
        readonly IDictionary<string, string> users;

        public FixedMemberProvider(string user, string pass)
        {
            users = new Dictionary<string, string>();
            users[user] = pass;
        }

        public MemberValidationResult ValidateUser(string username, string password)
        {
            string storedPass;
            if (users.TryGetValue(username, out storedPass) && storedPass.Equals(password))
                return new MemberValidationResult(MemberValidationStatus.AuthenticatedUser, new FtpUser(username));
            return new MemberValidationResult(MemberValidationStatus.InvalidLogin);
        }
    }
}