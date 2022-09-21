using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ServiceApp.Models
{
    public partial class ServicePhoto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string PhotoPath { get; set; } = null!;
        [NotMapped]
        public string AbsolutePath
        {
            get
            {
                
                string projectDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../../../"));
                return projectDir + PhotoPath;
            }
        }
        public virtual Service Service { get; set; } = null!;
    }
}
