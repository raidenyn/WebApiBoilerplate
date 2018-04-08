using JetBrains.Annotations;

namespace WebApiBoilerplate.Protocol
{
    public class UserInfo
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class User
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class ListUserRequest: IPagedRequest
    {
        [CanBeNull]
        public string Name { get; set; }

        public int? PageSize { get; } = 50;

        public int? PageIndex { get; } = 0;
    }

    public class CreateUserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
