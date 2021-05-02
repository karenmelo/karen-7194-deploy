using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 2 e 60 caracteres")]
        [MinLength(2, ErrorMessage = "Este campo deve conter entre 2 e 60 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(16, ErrorMessage = "Este campo deve conter entre 8 e 16 caracteres")]
        [MinLength(8, ErrorMessage = "Este campo deve conter entre 8 e 16 caracteres")]
        public string Password { get; set; }

        public string Role { get; set; }
    }

}