using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows;

namespace ServiceApp.Models
{
    public partial class Service
    {
        public Service()
        {
            ClientServices = new HashSet<ClientService>();
            ServicePhotos = new HashSet<ServicePhoto>();
            
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Cost { get; set; }
        public int DurationInSeconds { get; set; }
        public string? Description { get; set; }
        public double? Discount { get; set; }
        public string? MainImagePath { get; set; }
        public string AbsolutePath
        {
            get
            {

                string projectDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../../../"));
                return projectDir + MainImagePath;
            }
        }

        [NotMapped]
        public decimal? CostWithDisount
        {
            get
            {
                if (Discount.HasValue)
                {
                    return Cost - (Cost * ((decimal)Discount * 0.01M));
                }
                return default(decimal?);
            }
        }
        [NotMapped]
        public int DurationInMinute
        {
            get
            {
                return DurationInSeconds / 60;
            }
        }
        [NotMapped]
        public Visibility IsAdmin => Helper._isAdmin ? Visibility.Visible : Visibility.Collapsed;

        public virtual ICollection<ClientService> ClientServices { get; set; }
        public virtual ICollection<ServicePhoto> ServicePhotos { get; set; }
    }
}
