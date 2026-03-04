using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace loginpage
{
    class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }

        public override string ToString()
        {
            return $"{Name},{Email},{Password},{Role},{Contact}";
        }

        public static User FromString(string data)
        {
            string[] parts = data.Split(',');

            return new User
            {
                Name = parts[0],
                Email = parts[1],
                Password = parts[2],
                Role = parts[3],
                Contact = parts[4]
            };
        }
    }
}

