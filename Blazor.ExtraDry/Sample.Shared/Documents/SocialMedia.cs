using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sample.Shared {
    public class SocialMedia {

        public ICollection<Video> Videos { get; set; } = new Collection<Video>();

    }
}
