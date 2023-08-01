using System.ComponentModel.DataAnnotations;

namespace APIFurnitureStoreAPI.Controllers
{
    public class UserRegistrarionRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string EmailAdress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}