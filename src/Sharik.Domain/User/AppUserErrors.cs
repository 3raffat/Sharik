using Sharik.Domain.Common.Results;

namespace Sharik.Domain.User
{
    public static class AppUserErrors
    {

        public static Error UserNameRequired
            => Error.Validation(
                code: "AppUser.UserName.Required" ,
                description: "user name cannot be empty.");

        public static Error EmailRequired
            => Error.Validation(
                code: "AppUser.Email.Required" ,
                description: "email cannot be empty.");

        public static Error PasswordRequired
            => Error.Validation(
                code: "AppUser.Password.Required" ,
                description: "password cannot be empty.");

        public static Error FirstNameRequired
            => Error.Validation(
                code: "AppUser.FirstName.Required" ,
                description: "first name cannot be empty.");

        public static Error LastNameRequired
                    => Error.Validation(
                        code: "AppUser.LastName.Required" ,
                        description: "last name cannot be empty.");

        public static Error EmailInvalid => Error.Validation(
            code: "AppUser.Email.Invalid" ,
            description: "user email is invalid.");


        public static Error EmailInvalidFormat => Error.Validation(
            code: "AppUser.Email.Invalid" ,
            description: "User email format is invalid.");

        public static Error PasswordInvalidFormat => Error.Validation(
           code: "AppUser.Password.InvalidFormat" ,
           description: "Password must be 8+ chars with uppercase, number & special char.");


        public static Error UserNameTooShort => Error.Validation(
           code: "AppUser.UserName.TooShort" ,
           description: "user name must be at least 3 characters long."
        );

        public static Error UserNameTooLong => Error.Validation(
             code: "AppUser.UserName.TooLong" ,
             description: "user name cannot exceed 15 characters."
         );


        public static Error FirstNameTooShort =>
          Error.Validation(
              code: "AppUser.FirstName.TooShort" ,
              description: "first name must be at least 3 characters long."
          );

        public static Error FirstNameTooLong =>
           Error.Validation(
               code: "AppUser.FirstName.TooLong" ,
               description: "first name cannot exceed 50 characters."
           );

        public static Error LastNameTooShort =>
           Error.Validation(
               code: "AppUser.LastName.TooShort" ,
               description: "last name must be at least 3 characters long."
           );

        public static Error LastNameTooLong =>
           Error.Validation(
               code: "AppUser.LastName.TooLong" ,
               description: "last name cannot exceed 50 characters."
           );

        public static Error BioTooShort =>
         Error.Validation(
             code: "AppUser.Bio.TooShort" ,
             description: "Bio must be at least 50 characters long."
         );

        public static Error BioTooLong =>
           Error.Validation(
               code: "AppUser.Bio.TooLong" ,
               description: "Bio cannot exceed 500 characters."
           );

        public static Error ProfileAlreadyComplete =>
            Error.Conflict(
               code: "AppUser.Profile.AlreadyComplete" ,
               description: "Profile has already been completed.");

        public static Error ProfileIncomplete =>
          Error.Conflict(
              code: "AppUser.Profile.Incomplete" ,
              description: "The user profile must be completed before performing this action."
          );

        public static Error UserIdRequired => Error.Validation(
           code: "AppUser.UserId.Required" ,
           description: "UserId cannot be empty."
        );

        public static Error TokenRequired => Error.Validation(
            code: "AppUser.Token.Required" ,
            description: "Token cannot be empty."
        );
    }
}
