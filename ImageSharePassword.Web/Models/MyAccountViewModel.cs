using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageSharePassword.Data;


namespace ImageSharePassword.Web.Models
{
    public class MyAccountViewModel
    {
        public IEnumerable<Image> Images { get; set; }
    }
}