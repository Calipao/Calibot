using System;
using System.Collections.Generic;
using System.Text;

namespace CalibotCS.Services.Reddit
{
    public class Post
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Score { get; set; }

        public string Thumbnail { get; set; }

        public string Url { get; set; }

        public string Permalink { get; set; }

        public int NumberOfComments { get; set; }

        public bool Spoiler { get; set; }

        public bool Nsfw { get; set; }

        public string Pinned { get; set; }
    }
}
