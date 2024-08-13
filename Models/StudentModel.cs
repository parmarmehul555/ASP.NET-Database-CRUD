using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Student name can not be empty ")]

        public string StudentName { get; set; }
        [Required(ErrorMessage = "Enrollment number can not be empty ")]

        public string EnrollmentNo { get; set; }
        [Required(ErrorMessage = "Password can not be empty ")]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Roll number can not be empty ")]

        public int RollNo { get; set; }
        public int CurrentSem { get; set; }
        [Required(ErrorMessage ="Email institute can not be empty ")]
        [EmailAddress]
        public string EmailInstitute { get; set; }
        [Required(ErrorMessage = "Email personal can not be empty ")]
        [EmailAddress]

        public string Emailpersonal { get; set; }
        [Phone]
        [Range(1,100)]
        [MinLength(6)]
        public string ContactNumber { get; set; }
        public int CastId { get; set; }
        public int CityId { get; set; }
        public string Remarks { get; set; }
        public DateTime BirthDate{ get; set; }
        public int Age { get; set; }
    }
}
