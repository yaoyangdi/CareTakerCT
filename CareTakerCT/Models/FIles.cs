namespace CareTakerCT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Files
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
