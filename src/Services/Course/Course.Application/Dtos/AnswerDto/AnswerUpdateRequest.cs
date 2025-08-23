using System.ComponentModel.DataAnnotations;

namespace Course.Application.Dtos.AnswerDto
{
    public class AnswerUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string AnswerText { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Order { get; set; }
    }
}
