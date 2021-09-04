using ExtraDry.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Core.Tests.Rules {
    public class User {

        [Key]
        public int Id { get; set; }

        [Rules(DeleteValue = ActiveType.Deleted)]
        public ActiveType Active { get; set; } = ActiveType.Pending;

        public string Name { get; set; }

        public Address Address { get; set; }

    }
}
