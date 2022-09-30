using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoCofigSettingApi.Models
{
    public class Setting
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Token { get; set; }

        public string NameUnit { get; set; }
    }
}