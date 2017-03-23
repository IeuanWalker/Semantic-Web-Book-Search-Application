using SPARQL_Application.Models;
using System;
using System.Linq;
using VDS.RDF.Query;

namespace SPARQL_Application.Classes
{
    public class UserSearch
    {
        private readonly BookDBContext db = new BookDBContext();
        int minutesOld = -2;
        public void search(string searchString)
        {
            //Checks if item has been searched before
            if (db.bookNameTable.Any(o => o.SearchedFor.Equals(searchString)))
            {

                var queryDate = from BNT in db.bookNameTable
                                where BNT.SearchedFor == searchString
                                select BNT;

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
                    var books = from b in db.bookNameTable select b;
                    books = books.Where(s => s.SearchedFor.Equals(searchString));
                    foreach (BookUserSearch i in books)
                    {
                        db.bookNameTable.Remove(db.bookNameTable.Find(i.ID));
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error removing items from database");
                        Console.WriteLine(e);
                    }

                    //Request new information
                    SparqlResultSet resultSet = utilities.QueryDbpedia(utilities.QueryUserSearchBookName(searchString));
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
                SparqlResultSet resultSet = utilities.QueryDbpedia(utilities.QueryUserSearchBookName(searchString));
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
                name = utilities.RemoveLast3Cahracters(name);
                author = utilities.RemoveLast3Cahracters(author);

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
            db.bookNameTable.Add(book);

            //Submit changes to database
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("error adding to database");
                Console.WriteLine(e);
            }

        }
    }
}