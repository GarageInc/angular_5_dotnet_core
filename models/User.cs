using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace depot {

    public class RegisterModel {
        [Required (ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Не указан пароль")]
        [DataType (DataType.Password)]
        public string Password { get; set; }

        [DataType (DataType.Password)]
        [Compare ("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel {
        [Required (ErrorMessage = "Не указан Email")]
        public string Username { get; set; }

        [Required (ErrorMessage = "Не указан пароль")]
        [DataType (DataType.Password)]
        public string Password { get; set; }
    }

    [Table ("User")]
    public class User : IdentityUser<int> {
        public string Password { get; set; }

        public int? PartsSupplierID { get; set; }

        [ForeignKey ("PartsSupplierID")]
        public PartsSupplier PartsSupplier { get; set; }
        public Role Role { get; set; }

        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual void OnBeforeInsert () => this.CreatedAt = this.GetTimeStamp;
        public virtual void OnBeforeUpdate () => this.UpdatedAt = this.GetTimeStamp;

        protected int GetTimeStamp => (Int32) (DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
    }

    [Table ("Role")]
    public class Role : IdentityRole<int> {
        public List<User> Users { get; set; }
        public Role () {
            Users = new List<User> ();
        }
    }
}