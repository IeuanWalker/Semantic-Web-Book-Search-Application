

using SPARQL_Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace SPARQL_Application.Models
{
    public class BookAuthorDetails
    {
        public BookAuthorDetails()
        {
            AuthorBooks = new List<BookDetails>();
        }
        [Key]
        public int AuthorId { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorName { get; set; }
        public string PlaceOfBirthLink { get; set; }
        public string PlaceOfBirth { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime DataAndTime { get; set; } = DateTime.Now; //Time of last search

        //Create relationship
        public virtual List<BookDetails> AuthorBooks { get; set; }
    }
}

