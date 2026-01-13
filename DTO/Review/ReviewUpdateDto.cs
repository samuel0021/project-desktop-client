using System.ComponentModel.DataAnnotations;

namespace Project.Api.DTO.Reviews
{
    public class ReviewUpdateDto
    {
        [MaxLength(150)]
        public string? Title { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }
     
        public string? Description { get; set; }

        public int? UserId { get; set; }

        //public IFormFile? Image { get; set; } // opcional, para trocar imagem

    }
}