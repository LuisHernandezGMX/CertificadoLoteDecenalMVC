using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CertificadoLoteDecenalMVC.Models.DAL.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }
    }
}