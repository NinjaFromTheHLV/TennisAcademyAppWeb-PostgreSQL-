using System;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TableName { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string OperationType { get; set; } = null!;

        [Required]
        public DateTime OperationTimestamp { get; set; }
    }
}