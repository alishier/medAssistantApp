
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace medAssisTantApp.Models
{
    public class Doctor 
    {
        public Doctor(List<Patient> patients)
        {
            Patients = new List<Patient>();
        }

        public Doctor()
        {
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Speciality { get; set; }

        public List<Patient> Patients{ get; set; }

    }


}
