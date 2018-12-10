using POC01.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace POC01.Model
{
    public class Patient : IAddress
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address1 { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address2 { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(2)]
        public string State { get; set; }
        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(2)]
        public string Country { get { return "US"; } }
        [Required]
        public int DentistId { get; set; }
        public string TenantId { get; set; }
    }
}