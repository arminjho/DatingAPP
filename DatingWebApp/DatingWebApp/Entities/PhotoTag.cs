﻿namespace DatingWebApp.Entities
{
    public class PhotoTag
    {
        public int PhotoId { get; set; }
        public Photo Photo { get; set; } = null!;
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
