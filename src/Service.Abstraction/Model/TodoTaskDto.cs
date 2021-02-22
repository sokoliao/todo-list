using System;
using System.ComponentModel.DataAnnotations;

namespace Service.Abstraction.Model
{
    public class TodoTaskDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Priority { get; set; }

        [Required]
        public TodoTaskDtoStatus Status { get; set; }
    }
}