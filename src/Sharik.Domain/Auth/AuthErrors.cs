using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.Auth
{
    public static class AuthErrors
    {

        public static Error InvalidCredentials =>Error.Unexpected("Auth.InvalidCredentials",
                                                                  "The provided credentials are invalid.");


        public static Error InvalidPassword => Error.Unexpected("Auth.InvalidPassword",
                                                                "The password is incorrect.");

        public static Error InvalidUserName => Error.Unexpected("Auth.InvalidUserName",
                                                                "The username is incorrect.");

        public static Error InvalidEmail => Error.Unexpected("Auth.InvalidEmail",
                                                             "The email is incorrect.");

        public static Error InvalidEmailOrPassword => Error.Unexpected("Auth.InvalidEmailOrPassword",
                                                                       "The email or password is incorrect.");

        public static Error UsernameAlreadyExists => Error.Unexpected("Auth.UsernameAlreadyExists",
                                                                       "The username is already taken.");
    }
}
