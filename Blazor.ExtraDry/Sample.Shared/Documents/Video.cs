using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sample.Shared {
    public class Video {

        [Display(Name = "Video Title", ShortName = "Title", Description = "Title of video for display to users as links, etc.", Prompt = "Enter title")]
        public string Title { get; set; }


        [Display(Name = "URL for Video", Description = "Link to the video, e.g. https://www.youtube.com/watch?abc", Prompt = "Enter link")]
        [Url]
        public string Uri { get; set; }

    }
}
