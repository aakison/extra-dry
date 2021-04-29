﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public class SectionViewModel {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Icon { get; set; }

        public Collection<ArticleViewModel> Articles { get; } = new Collection<ArticleViewModel>();
    }
}