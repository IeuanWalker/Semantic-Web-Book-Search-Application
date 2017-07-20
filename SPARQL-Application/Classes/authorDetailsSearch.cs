using SPARQL_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDS.RDF.Query;

namespace SPARQL_Application.Classes
{
    public class AuthorDetailsSearch
    {
        private readonly BookDbContext _db = new BookDbContext();
        private readonly int _minutesOld = -2;

        public void Search(string authorLink)
        {
            //Checks if the author is in the database
            if (_db.AuthorDetailsTable.Any(o => o.AuthorLink.Equals(authorLink)))
            {
                var queryDate = from items in _db.AuthorDetailsTable
                                where items.AuthorLink == authorLink
                                select items;

                var dateTimeNow = DateTime.Now;
                var dateTimeOldest = dateTimeNow.AddMinutes(_minutesOld);
                var dateTimeSearch = queryDate.FirstOrDefault().DataAndTime;

                //Check the date
                if (dateTimeSearch <= dateTimeNow && dateTimeSearch >= dateTimeOldest)
                {
                    //Information in the database is within the date period no need to get new information
                }
                else if (dateTimeSearch < dateTimeOldest)
                {
                    //Information in database is too old, need newer information
                    //Delete current information
                    var authors = from b in _db.AuthorDetailsTable select b;
                    authors = authors.Where(s => s.AuthorLink.Equals(authorLink));
                    foreach (BookAuthorDetails i in authors)
                    {
                        _db.AuthorDetailsTable.Remove(_db.AuthorDetailsTable.Find(i.AuthorId));
                    }
                    _db.SaveChanges();

                    //Request new information
                    SparqlResultSet resultSetAuthorDetails = Utilities.QueryDbpedia(Utilities.QueryAuthorDetails(authorLink));
                    SparqlResultSet resultSetAuthorBooks = Utilities.QueryDbpedia(Utilities.QueryAuthorBooks(authorLink));

                    LoopValuesToDatabase(resultSetAuthorDetails, resultSetAuthorBooks);
                }
            }
            else
            {
                SparqlResultSet resultSetAuthorDetails = Utilities.QueryDbpedia(Utilities.QueryAuthorDetails(authorLink));
                SparqlResultSet resultSetAuthorBooks = Utilities.QueryDbpedia(Utilities.QueryAuthorBooks(authorLink));

                if (!resultSetAuthorDetails.IsEmpty && !resultSetAuthorBooks.IsEmpty)
                {
                    LoopValuesToDatabase(resultSetAuthorDetails, resultSetAuthorBooks);
                }
            }
        }

        private void LoopValuesToDatabase(SparqlResultSet resultSetAuthorDetails, SparqlResultSet resultSetAuthorBooks)
        {
            if (resultSetAuthorDetails.IsEmpty && resultSetAuthorBooks.IsEmpty)
                return;
            string authorLink = resultSetAuthorDetails.Last()["authorLink"].ToString();
            string authorName = resultSetAuthorDetails.Last()["authorName"].ToString();
            string placeOfBirthLink = resultSetAuthorDetails.Last()["placeOfBirthLink"].ToString();
            string placeOfBirth = resultSetAuthorDetails.Last()["PlaceOfBirth"].ToString();
            string stringLatitude = resultSetAuthorDetails.Last().Value("latitude").ToString(); // ["latitude"]["value"].ToString());
            string stringLongitude = resultSetAuthorDetails.Last().Value("longitude").ToString();

            //Convert latitude and longitude to float

            float latitude = float.Parse(Utilities.NumberConverter(stringLatitude));
            float longitude = float.Parse(Utilities.NumberConverter(stringLongitude));

            if (authorName.Length > 3 && placeOfBirth.Length > 3)
            {
                authorName = authorName.Substring(0, authorName.Length - 3);
                placeOfBirth = placeOfBirth.Substring(0, placeOfBirth.Length - 3);
            }

            //Create a list of all the authors books
            List<BookDetails> authorBooks = new List<BookDetails>();
            foreach (SparqlResult result in resultSetAuthorBooks)
            {
                string bookLink = result["bookLink"].ToString();
                string name = result["bookName"].ToString();
                string bookAbstract = result["bookAbstract"].ToString();
                string stringNumberOfPages = "";
                if (result["numberOfPages"] != null)
                {
                    stringNumberOfPages = result["numberOfPages"].ToString();
                }
                string comment = result["comment"].ToString();

                //Remove @en from the string
                name = Utilities.RemoveLast3Cahracters(name);
                bookAbstract = Utilities.RemoveLast3Cahracters(bookAbstract);
                comment = Utilities.RemoveLast3Cahracters(comment);

                //Convert numberOfPages to int
                int numberOfPages = 0;
                if (result["numberOfPages"] != null)
                {
                    numberOfPages = int.Parse(Utilities.NumberConverter(stringNumberOfPages));
                }

                //Create a BookDetails object
                BookDetails book = new BookDetails
                {
                    BookLink = bookLink,
                    Name = name,
                    Abstract = bookAbstract,
                    NumberOfPages = numberOfPages,
                    Comment = comment
                };
                authorBooks.Add(book);
            }

            //
            AddToDatabase(authorLink, authorName, placeOfBirthLink, placeOfBirth, latitude, longitude, authorBooks);
        }

        private void AddToDatabase(String authorLink, String authorName, String placeOfBirthLink, String placeOfBirth, float latitude, float longitude, List<BookDetails> authorBooks)
        {
            //Create new BookName object
            BookAuthorDetails book = new BookAuthorDetails
            {
                AuthorLink = authorLink,
                AuthorName = authorName,
                PlaceOfBirthLink = placeOfBirthLink,
                PlaceOfBirth = placeOfBirth,
                Latitude = latitude,
                Longitude = longitude,
                AuthorBooks = authorBooks
            };
            //Add the new object to the BookName collection
            _db.AuthorDetailsTable.Add(book);

            //Submit changes to database
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("error adding to database");
                Console.WriteLine(e);
            }
        }
    }
}