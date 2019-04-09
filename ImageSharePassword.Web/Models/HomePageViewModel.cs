using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharePassword.Web.Models
{
    public class HomePageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}