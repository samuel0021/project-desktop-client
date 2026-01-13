using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Api.DTO.Reviews
{
    public class ReviewDetailsDto
    {     
        public int Id { get; set; }
        public string Title { get; set; }     
        public string Category { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string? ImageUrl { get; set; } // caminho para o cliente exibir

    }
}
