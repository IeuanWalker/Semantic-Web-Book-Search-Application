using PagedList;
using SPARQL_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SPARQL_Application.Classes;

namespace SPARQL_Application.Controllers
{
    public class HomeController : Controller
    {
        //Access to database
        private readonly BookDbContext _db = new BookDbContext();

        //Access to different search methods
        private readonly UserSearch _userSearch = new UserSearch();

        private readonly AuthorDetailsSearch _authorSearch = new AuthorDetailsSearch();

        //Different page requests
        public async Task<ActionResult> Index(string id = null, int page = 1, int pageSize = 10)
        {
            //Run the search method if user has search for an item i.e. id isn't null
            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(id.Trim()))
                {
                    _userSearch.Search(id);
                }
            }

            //LINQ Query to select the book
            var books = from b in _db.BookNameTable select b;

            //Filters the books returned to the view by the searched query
            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(id.Trim()))
                {
                    books = books.Where(s => s.SearchedFor.Equals(id));
                }
            }

            //Create a list of searched book in date order
            IEnumerable<BookUserSearch> bookList = await books.OrderByDescending(s => s.DataAndTime).ToListAsync();

            //Using pagedList package to add pagination
            PagedList<BookUserSearch> model = new PagedList<BookUserSearch>(bookList, page, pageSize);

            return View(model);
        }

        public ActionResult AuthorDetails(string authorLink)
        {
            //Checks if the user link has the proper parameters, if not the user will be redirected to the home page
            if (!String.IsNullOrEmpty(authorLink))
            {
                if (!String.IsNullOrEmpty(authorLink.Trim()))
                {
                    _authorSearch.Search(authorLink);
                }
                else
                {
                    Response.Redirect("Index");
                }
            }
            else
            {
                Response.Redirect("Index");
            }

            //Creates a list of the author details
            var details = from b in _db.AuthorDetailsTable select b;
            details = details.Where(s => s.AuthorLink.Equals(authorLink));

            return View(details);
        }

        public ActionResult BookDetails(string bookLink, string authorLink)
        {
            //Checks if the user link has the proper parameters, if not the user will be redirected to the home page
            if (!String.IsNullOrEmpty(authorLink))
            {
                if (!String.IsNullOrEmpty(authorLink.Trim()))
                {
                    _authorSearch.Search(authorLink);
                }
                else
                {
                    Response.Redirect("Index");
                }
            }
            else
            {
                Response.Redirect("Index");
            }

            //Creates a list of the book details
            var details = from b in _db.BookDetailsTable select b;
            details = details.Where(s => s.BookLink.Equals(bookLink));

            return View(details);
        }

        ~HomeController()
        {
            _db.Dispose();
        }

        public new void Dispose()
        {
            _db.Dispose();
        }
    }
}