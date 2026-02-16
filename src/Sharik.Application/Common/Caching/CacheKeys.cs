namespace Sharik.Application.Common.Caching
{
    public static class CacheKeys
    {
        public static readonly TimeSpan Expiration = TimeSpan.FromMinutes(20);

        public static readonly TimeSpan LongExpiration = TimeSpan.FromHours(1);

        public static readonly TimeSpan ShortExpiration = TimeSpan.FromMinutes(1);


        public static class User
        {
            public const string AllUsers = "User:All";
            public static string UserById(Guid id) => $"User:{id}";
        }

        public static class Exchange
        {
            public const string AllExchanges = "Exchange:All";
            public static string ExchangeByProviderId(Guid id) => $" Provider Exchange:{id}";
        }

        public static class Rating
        {
            public const string AllRatings = "Rating:All";
            public static string RatingById(Guid id) => $"Rating:{id}";
        }

        public static class UserSkill
        {
            public const string AllUserSkills = "UserSkill:All";
            public static string UserSkillById(Guid id) => $"UserSkill:{id}";
        }

        public static class Skill
        {
            public const string AllSkills = "Skill:All";
            public static string SkillById(Guid id) => $"Skill:{id}";
        }

        public static class Category
        {
            public const string AllCategories = "Category:All";
            public static string CategoryById(Guid id) => $"Category:{id}";
        }
        public static class Notification
        {
            public const string SystemNotification = "SystemNotfication:All";
            public static string NotficationByUserId(Guid id) => $"Notification:{id}";
        }

        public static class Message
        {
            public static string MessagesByExchangeId(Guid id) => $"MessageExchange:{id}";
        }
    }
}
