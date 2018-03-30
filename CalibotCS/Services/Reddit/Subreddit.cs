using System;
using System.Collections.Generic;
using System.Text;

namespace CalibotCS.Services.Reddit
{
    public class Subreddit
    {
        public string Name { get; set; }

        public List<Post> Posts { get; set; }

        public int PageNumber { get; set; }


        public void NextPage()
        {

        }

        public void PreviousPage()
        {

        }
    }
}
