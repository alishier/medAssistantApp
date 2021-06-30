using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medAssisTantApp.Models
{

    public enum GenderEnumeration
    {
        MALE,
        FEMALE,
        CHILD
    }

    public class Patient
    {
        public Patient(List<MedCard> medCards)
        {
            MedCards = new List<MedCard>();
        }

        public Patient()
        {
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        [EnumDataType(typeof(GenderEnumeration))]
        public GenderEnumeration Gender { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        public List<MedCard> MedCards{ get; set; }

        public Doctor Doctor { get; set; }

    }
}
