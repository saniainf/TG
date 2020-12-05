using System;
using System.Collections.Generic;
using FontAwesome5;

namespace TG.Models.Company
{
    internal class User
    {
        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }

        public EFontAwesomeIcon AwesomeIcon { get; set; }
    }

    internal class Department
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public string Description { get; set; }
    }
}
