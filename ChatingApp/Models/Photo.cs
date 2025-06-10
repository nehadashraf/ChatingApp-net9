using System.ComponentModel.DataAnnotations.Schema;

namespace ChatingApp.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }=null!;
    }
}