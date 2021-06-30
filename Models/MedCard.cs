using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medAssisTantApp.Models
{
    public class MedCard
    {
        public MedCard()
        {
        }
        [Key]
        public int Id { get; set; }

        public string Complain { get; set; }

        public string Description { get; set; }

        public string Diagnosis { get; set; }

        public string Instructions { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

    }
}
