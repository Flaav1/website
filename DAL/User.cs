//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        public System.DateTime DateofBirth { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public string salt { get; set; }
        public bool Seller { get; set; }
        public bool VerEmail { get; set; }
    }
}
