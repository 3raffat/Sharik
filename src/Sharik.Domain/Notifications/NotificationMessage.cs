namespace Sharik.Domain.Notifications
{
    public static class NotificationMessage
    {
        public static string NewExchangeRequest(string senderName)
            => $"You have a new exchange request from {senderName}.";

        public static string ExchangeAccepted(Guid exchangeId)
            => $"Your exchange request #{exchangeId} has been accepted.";

        public static string ExchangeRejected(Guid exchangeId )
            => $"Your exchange request #{exchangeId} was rejected.";

        public static string ExchangeCompleted(Guid exchangeId)
            => $"Exchange #{exchangeId} completed successfully.";

        public static string ExchangeCanceled(Guid exchangeId)
            => $"Exchange #{exchangeId} has been canceled.";

        public static string NewMessage(string senderName , string messagePreview)
            => $"New message from {senderName}: {messagePreview}";

        public static string NewRating(int rating)
            => $"You received a new rating: {rating}/5.";

        public static string PointsEarned(string points)
            => $"You earned {points} points.";

        public static string PointsDeducted(string points)
            => $"{points} points were deducted from your account.";

        public static string WelcomePoints()
            => $"Welcome aboard! 🎉 You've received 50 bonus points to get started.";

        public static string ProfileCompleted()
            => $"Great job! 🎉 You received 50 bonus points for completing your profile.";

    }

}
