namespace Parser
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Worker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(5)]
        public string Prefix { get; set; }

        [Required]
        [StringLength(50)]
        public string Position { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime BirthDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        public int StateID { get; set; }

        [Column(TypeName = "money")]
        public decimal Salary { get; set; }

        public bool IsAlcoholic { get; set; }
    }
}
