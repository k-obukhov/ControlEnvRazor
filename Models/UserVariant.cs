
namespace ControlEnvRazor.Models
{
    public enum State
    {
        WaitForChecking = 1, // Ожидает проверки
        HavingErrors = 2,    // Преподаватель нашел ошибку
        Finihed = 4          // Завершено
    }

    public class UserVariant
    {
        // Вариант, выданный определенному пользователю вместе с его решением

        public int Id { get; set; } // Связать с решением пользователя

        public string GithubRepo { get; set; } // Ссылка на репозиторий

        public State State { get; set; } // В каком состоянии решение пользователя

        // Связь ApplicationUser -> TaskVariant
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual string ApplicationUserId { get; set; }

        // Связь TaskVariant -> UserVariant
        public virtual TaskVariant TaskVariant { get; set; }
        public virtual int TaskVariantId { get; set; }
    }
}
