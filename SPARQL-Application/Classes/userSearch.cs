using SPARQL_Application.Models;
using System;
using System.Linq;
using VDS.RDF.Query;

namespace SPARQL_Application.Classes
{
    public class UserSearch
    {
        private readonly BookDbContext _db = new BookDbContext();
        private readonly int _minutesOld = -2;

        public void Search(string searchString)
        {
            //Checks if item has been searched before
            if (_db.BookNameTable.Any(o => o.SearchedFor.Equals(searchString)))
            {
                var queryDate = from bnt in _db.BookNameTable
                                where bnt.SearchedFor == searchString
                                select bnt;

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
                    var books = from b in _db.BookNameTable select b;
                    books = books.Where(s => s.SearchedFor.Equals(searchString));
                    foreach (BookUserSearch i in books)
                    {
                        _db.BookNameTable.Remove(_db.BookNameTable.Find(i.Id));
                    }
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error removing items from database");
                        Console.WriteLine(e);
                    }

                    //Request new information
                    SparqlResultSet resultSet = Utilities.QueryDbpedia(Utilities.QueryUserSearchBookName(searchString));
                    LoopValuesToDatabase(searchString, resultSet);
                }
                else if (dateTimeSearch > dateTimeNow)
                {
                    Console.WriteLine("ERROR: Database information is newer than possible");
                }
            }
            else
            {
                //Not in the database, send query and add to database
                SparqlResultSet resultSet = Utilities.QueryDbpedia(Utilities.QueryUserSearchBookName(searchString));
                LoopValuesToDatabase(searchString, resultSet);
            }
        }

        private void LoopValuesToDatabase(String searchString, SparqlResultSet resultSet)
        {
            foreach (SparqlResult result in resultSet)
            {
                String bookLink = result["s"].ToString();
                String name = result["bookName"].ToString();
                String authorLink = result["authorLink"].ToString();
                String author = result["author"].ToString();

                //remove the @en
                name = Utilities.RemoveLast3Cahracters(name);
                author = Utilities.RemoveLast3Cahracters(author);

                AddToDatabase(searchString, name, authorLink, author, bookLink);
            }
        }

        private void AddToDatabase(String search, String name, String authorlink, String author, String bookLink)
        {
            //Create new BookName object
            BookUserSearch book = new BookUserSearch
            {
                SearchedFor = search,
                Name = name,
                AuthorLink = authorlink,
                Author = author,
                BookLink = bookLink
            };

            //Add the new object to the BookName collection
            _db.BookNameTable.Add(book);

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