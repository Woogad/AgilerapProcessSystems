using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgilerapProcessSystems.Models
{
    public class UserMetadata
    {
        //[Required(ErrorMessage ="ss"),DisplayName("User Name")]
        [DisplayName("User Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage ="Password Required")]
        public string? Password { get; set; }

        //[Required(ErrorMessage ="ConfirmPassword Required")]
        //public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Email Required")]
        public string? Email { get; set; }
    }
    [ModelMetadataType(typeof(UserMetadata))]
    public partial class User
    {
        [NotMapped]
        public string? ConfirmPassword { get; set; }

    }
}
