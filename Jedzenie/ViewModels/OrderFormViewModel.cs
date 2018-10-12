using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jedzenie.ViewModels
{
    public class OrderFormViewModel
    {
        [Display(Name = "Imie")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Surname { get; set; }
        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Address { get; set; }
        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Telephone { get; set; }
    }
}