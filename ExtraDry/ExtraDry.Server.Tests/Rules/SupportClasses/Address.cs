using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;

namespace ExtraDry.Core.Tests.Rules {
    public class Address {

        [Key]
        public int Id { get; set; }

        [Rules(DeleteValue = ActiveType.Deleted)]
        public ActiveType Active { get; set; } = ActiveType.Pending;

        public string Line { get; set; }

    }
}
