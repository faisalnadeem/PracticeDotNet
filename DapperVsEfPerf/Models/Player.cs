﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DapperVsEfPerf.Models
{
    [Table("Player")]
    public partial class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}
