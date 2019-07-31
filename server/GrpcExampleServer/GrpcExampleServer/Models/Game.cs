using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcExampleServer.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int GameCode { get; set; }
        public string Name { get; set; }
        public Platform Platform { get; set; }
        public string LandingUrl { get; set; }
        public string ImageUrl { get; set; }
    }

    public enum Platform
    {
        Pc = 0,
        Mobile = 1,
        Console = 2,
        Etc = 3
    }
}
