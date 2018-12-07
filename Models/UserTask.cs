
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ControlEnvRazor.Models
{
    public class UserTask
    {
        // Класс для описания сущности "Задание по программированию"
        // Содержит -- название, общее описание, список вариантов
        public int Id { get; set; }

        [Required(ErrorMessage = "Должно быть указано название задания!")]
        public string TaskName { get; set; } // Название задачи

        public string CommonDescription { get; set; }

        // Задание содержит варианты -- 
        public virtual ICollection<TaskVariant> UserTaskVariants { get; set; }

        // У задания есть создатель 
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual string ApplicationUserId { get; set; }

        public UserTask()
        {
            this.UserTaskVariants = new List<TaskVariant>();
        }
    }
}
