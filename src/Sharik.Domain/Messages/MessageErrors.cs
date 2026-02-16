using Sharik.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.Messages
{
    public static class MessageErrors
    {

        public static Error MassageRequired
              => Error.Validation(
               code: "Message.Contect.Required" ,
               description: "Massage content cannot be empty."
              );

        public static Error SenderIdRequired
             => Error.Validation(
                 code: "Message.SenderId.Required" ,
                 description: "Sender ID connot be empty."
             );

        public static Error ExchangeIdRequired
           => Error.Validation(
               code: "Message.ExchangeId.Required" ,
               description: "Exchange ID connot be empty."
           );

        public static Error ContentTooLong => Error.Validation(
               code: "Chat.Content.MaxLength" ,
               description: "Message content cannot exceed 1000 characters."
           );
    }
}
