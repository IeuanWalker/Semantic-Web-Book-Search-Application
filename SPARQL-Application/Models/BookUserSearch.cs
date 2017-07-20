using System;

namespace SPARQL_Application.Models
{
    public class BookUserSearch
    {
        public int Id { get; set; }
        public string SearchedFor { set; get; }
        public string Name { get; set; }
        public string AuthorLink { get; set; }
        public string Author { get; set; }
        public string BookLink { get; set; }
        public DateTime DataAndTime { get; set; } = DateTime.Now; //Time of last search
    }
}