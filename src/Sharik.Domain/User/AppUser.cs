using Microsoft.AspNetCore.Identity;
using Sharik.Domain.Common.Results;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Domain.User;
using Sharik.Domain.User.Enums;
using System.Net.Mail;

namespace Sharik.Infrastructure.Auth
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Bio { get; private set; }
        public int TotalPointsEarned { get; private set; }
        public double Rating { get; private set; }

        public ProfileStatus ProfileStatus { get; set; } = ProfileStatus.Incomplete;

        private List<Exchange> _providedExchanges = new();
        public IEnumerable<Exchange> ProvidedExchanges => _providedExchanges.AsReadOnly();

        private List<Exchange> _requestedExchanges = new();
        public IEnumerable<Exchange> RequestedExchanges => _requestedExchanges.AsReadOnly();

        private List<Rating> _receivedRatings = new();
        public IEnumerable<Rating> ReceivedRatings => _receivedRatings.AsReadOnly();

        private List<Rating> _givenRatings = new();
        public IEnumerable<Rating> GivenRatings => _givenRatings.AsReadOnly();

        private readonly List<UserSkill> _userSkills = new();
        public IEnumerable<UserSkill> UserSkills => _userSkills.AsReadOnly();
        private AppUser() { }


        private AppUser(string username, string email)
        {
            this.UserName = username;
            this.Email = email;
        }
        public static Result<AppUser> Create(string username, string email)
        {
            var validation = Validate(username, email);

            if (validation.IsFailure)
                return validation.Errors;

            return new AppUser(username, email);
        }

        public Result<Updated> CompleteProfile(string firstName, string lastName, string bio)
        {
            if (ProfileStatus == ProfileStatus.Complete)
                return AppUserErrors.ProfileAlreadyComplete;

            var validation = Validate(firstName, lastName, bio);

            if (validation.IsFailure)
                return validation.Errors;

            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            ProfileStatus = ProfileStatus.Complete;
            TotalPointsEarned += 50;

            return Result.Updated;
        }

        private static Result<Success> Validate(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                return AppUserErrors.UserNameRequired;

            if (string.IsNullOrWhiteSpace(email))
                return AppUserErrors.EmailRequired;

            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                return AppUserErrors.EmailInvalid;
            }
            return Result.Success;
        }

        private static Result<Success> Validate(string firstName, string lastName, string bio)
        {

            if (firstName.Length < 3)
                return AppUserErrors.FirstNameTooShort;

            if (firstName.Length > 50)
                return AppUserErrors.FirstNameTooLong;


            if (lastName.Length < 3)
                return AppUserErrors.LastNameTooShort;

            if (lastName.Length > 50)
                return AppUserErrors.LastNameTooLong;

            if (bio.Length < 50)
                return AppUserErrors.BioTooShort;

            if (bio.Length > 500)
                return AppUserErrors.BioTooLong;

            return Result.Success;
        }

        public string GetIdString() => Id.ToString();

    }
}
