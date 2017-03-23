using SPARQL_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDS.RDF.Query;

namespace SPARQL_Application.Classes
{
    public class authorDetailsSearch
    {
        private readonly BookDBContext db = new BookDBContext();
        int minutesOld = -2;
        public void search(string authorLink)
        {
            //Checks if the author is in the database
            if (db.authorDetailsTable.Any(o => o.AuthorLink.Equals(authorLink)))
            {
                var queryDate = from items in db.authorDetailsTable
                                where items.AuthorLink == authorLink
                                select items;

                var dateTimeNow = DateTime.Now;
                var dateTimeOldest = dateTimeNow.AddMinutes(minutesOld);
                var dateTimeSearch = queryDate.FirstOrDefault().dataAndTime;

                //Check the date
                if (dateTimeSearch <= dateTimeNow && dateTimeSearch >= dateTimeOldest)
                {
                    //Information in the database is within the date period no need to get new information
                }
                else if (dateTimeSearch < dateTimeOldest)
                {
                    //Information in database is too old, need newer information
                    //Delete current information
                    var authors = from b in db.authorDetailsTable select b;
                    authors = authors.Where(s => s.AuthorLink.Equals(authorLink));
                    foreach (BookAuthorDetails i in authors)
                    {
                        db.authorDetailsTable.Remove(db.authorDetailsTable.Find(i.AuthorID));
                    }
                    db.SaveChanges();

                    //Request new information
                    SparqlResultSet resultSetAuthorDetails = utilities.QueryDbpedia(utilities.QueryAuthorDetails(authorLink));
                    SparqlResultSet resultSetAuthorBooks= utilities.QueryDbpedia(utilities.QueryAuthorBooks(authorLink));

                    LoopValuesToDatabase(resultSetAuthorDetails, resultSetAuthorBooks);
                }
                else if (dateTimeSearch > dateTimeNow)
                {
                    Console.WriteLine("ERROR: Database information is new than possible");
                }
            }
            else
            {
                SparqlResultSet resultSetAuthorDetails = utilities.QueryDbpedia(utilities.QueryAuthorDetails(authorLink));
                SparqlResultSet resultSetAuthorBooks = utilities.QueryDbpedia(utilities.QueryAuthorBooks(authorLink));

                if (!resultSetAuthorDetails.IsEmpty && !resultSetAuthorBooks.IsEmpty)
                {
                    LoopValuesToDatabase(resultSetAuthorDetails, resultSetAuthorBooks);
                }
            }
        }
        private void LoopValuesToDatabase(SparqlResultSet resultSetAuthorDetails, SparqlResultSet resultSetAuthorBooks)
        {
            if (!(resultSetAuthorDetails.IsEmpty && resultSetAuthorBooks.IsEmpty))
            {
                String authorLink = resultSetAuthorDetails.Last()["authorLink"].ToString();
                String authorName = resultSetAuthorDetails.Last()["authorName"].ToString();
                String placeOfBirthLink = resultSetAuthorDetails.Last()["placeOfBirthLink"].ToString();
                String placeOfBirth = resultSetAuthorDetails.Last()["PlaceOfBirth"].ToString();
                String stringLatitude = resultSetAuthorDetails.Last().Value("latitude").ToString(); // ["latitude"]["value"].ToString());
                String stringLongitude = resultSetAuthorDetails.Last().Value("longitude").ToString();

                //Convert latitude and longitude to float

                float latitude = float.Parse(utilities.NumberConverter(stringLatitude));
                float longitude = float.Parse(utilities.NumberConverter(stringLongitude));

                if (authorName.Length > 3 && placeOfBirth.Length > 3)
                {
                    authorName = authorName.Substring(0, authorName.Length -3);
                    placeOfBirth = placeOfBirth.Substring(0, placeOfBirth.Length -3);
                }

                //Create a list of all the authors books
                List<BookDetails> authorBooks = new List<BookDetails>();
                foreach (SparqlResult result in resultSetAuthorBooks)
                {
                    String bookLink = result["bookLink"].ToString();
                    String name = result["bookName"].ToString();
                    String bookAbstract = result["bookAbstract"].ToString();
                    String stringNumberOfPages = "";
                    if (!(result["numberOfPages"] == null))
                    {
                        stringNumberOfPages = result["numberOfPages"].ToString();
                    }
                    String comment = result["comment"].ToString();

                    //Remove @en from the string
                    name = utilities.RemoveLast3Cahracters(name);
                    bookAbstract = utilities.RemoveLast3Cahracters(bookAbstract);
                    comment = utilities.RemoveLast3Cahracters(comment);

                    //Convert numberOfPages to int
                    int numberOfPages = 0;
                    if (!(result["numberOfPages"] == null))
                    {
                        numberOfPages = int.Parse(utilities.NumberConverter(stringNumberOfPages));
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
            db.authorDetailsTable.Add(book);

            //Submit changes to database
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("error adding to database");
            }
        }
    }
}