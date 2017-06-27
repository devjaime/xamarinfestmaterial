using System;
using SQLitePCL;

namespace GPSImagesTag.Core.Models
{
    public class Photo
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double  Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
