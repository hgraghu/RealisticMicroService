using ProductService.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Product : IEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = String.Empty;
        public int DisplayOrder { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = String.Empty;
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; } = String.Empty;
    }

}
