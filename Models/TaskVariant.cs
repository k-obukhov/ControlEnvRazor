
using System.Collections.Generic;

namespace ControlEnvRazor.Models
{
    public class TaskVariant
    {
        public int Id { get; set; }

        public int NumberOfVariant { get; set; } // Номер варианта (на данном этапе без проверок)
        public string Description { get; set; } // Текст задания на вариант

        // Вариант ссылается на задание
        public virtual int UserTaskId { get; set; }
        public virtual UserTask UserTask { get; set; }

        // Вариант содержит пары (пользователь-вариант)
        public virtual ICollection<UserVariant> UserVariants { get; set; }

        public TaskVariant()
        {
            this.UserVariants = new List<UserVariant>();
        }
    }
}
