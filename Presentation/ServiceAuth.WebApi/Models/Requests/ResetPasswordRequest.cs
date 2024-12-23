#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace ServiceAuth.WebApi.Models.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
