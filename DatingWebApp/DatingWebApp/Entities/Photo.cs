﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DatingWebApp.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public string ?PublicId { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }


    }
}
