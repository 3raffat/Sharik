using Microsoft.AspNetCore.Identity;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Domain.User.Enums;

namespace Sharik.Infrastructure.Auth
{
    public sealed class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public int TotalPointsEarned { get; set; }
        public double Rating { get; set; }

        public ProfileStatus ProfileStatus { get; set; } = ProfileStatus.Incomplete;

        private List<Exchange> _providedExchanges = new();
        public IEnumerable<Exchange> ProvidedExchanges => _providedExchanges.AsReadOnly();

        private List<Exchange> _requestedExchanges = new();
        public IEnumerable<Exchange> RequestedExchanges => _requestedExchanges.AsReadOnly();

        private List<Rating> _receivedRatings = new();
        public IEnumerable<Rating> ReceivedRatings => _receivedRatings.AsReadOnly();

        private List<Rating> _givenRatings = new();
        public IEnumerable<Rating> RivenRatings => _givenRatings.AsReadOnly();

        private readonly List<UserSkill> _userSkills = new();
        public IEnumerable<UserSkill> UserSkills => _userSkills.AsReadOnly();
        private AppUser() { }
    }
}
