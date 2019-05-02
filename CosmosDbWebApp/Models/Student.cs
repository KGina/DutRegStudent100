using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CosmosDbWebApp.Models
{
    public class Student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Student Number")]
        [Display(Name = "Student Number")]
        public int studentNumber { get; set; }
        [JsonProperty(PropertyName = "Student Name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(40, ErrorMessage = "Name cannot be longer than 40 characters.")]
        [Display(Name = "Student Name")]
        public string studentName { get; set; }
        [JsonProperty(PropertyName = "Surname")]
        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is required.")]
        //[ValidateRegExp(@"^[\p{L}\p{N}]*$", "Disallowed characters in Surname")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please Provide Last Name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "First Name Should be min 5 and max 20 length")]
        //[StringLength(40, ErrorMessage = "Surname cannot be longer than 40 characters.")]
        public string surname { get; set; }

        [JsonProperty(PropertyName = "Email Address")]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        [JsonProperty(PropertyName = "Telephone Number")]
      
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string telphone_No { get; set; }
        [JsonProperty(PropertyName = "Mobile phone")]
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string mobile { get; set; }
        [JsonProperty(PropertyName = "Is Active")]
        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }
    }
}