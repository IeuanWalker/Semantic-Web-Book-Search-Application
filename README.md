# Semantic Web Book Search Application

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/09e277a338b743c698269112489c8aeb)](https://www.codacy.com/app/ieuan.walker007/Semantic-Web-Book-Search-Application?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=IeuanWalker/Semantic-Web-Book-Search-Application&amp;utm_campaign=Badge_Grade)

This is a Project from University where i created a web application that sources data using semantic web technologies.

The objective of this assignment is to design and develop a web-based application with a database using ASP.NET and semantic web technologies. 

_“It enables data to be linked from a source to any other source and to be understood by computers so that they can perform increasingly sophisticated tasks on our behalf.”_

By using semantic web it means all the information that I will store and display will be from another web service, for example in my project I have used www.Dbpedia.com which is a semantic version of www.wikipedia.com. 

My project is to create a book searching web application that allows users to navigate easily to find out more information about a specific book or author. The web application will have 3 separate pages these are user search results, an individual book page (Book details) and an individual authors page (Author details). The user will be able to navigate to any of these pages from one another. 

Here is a link to a video demo - 

[![Youtube Thumbnail](https://img.youtube.com/vi/MZ-YIkh5DY4/0.jpg)](https://www.youtube.com/watch?v=MZ-YIkh5DY4)

## Programming Languages and technologies
To create this web application I have used a variety of different languages and technologies such as C#, .NET, SPARQL, JQuery, HTML5 and SASS.

I decided to use C# and the .NET framework because I have previous experience using these technologies. These two were used for the logic of the application, for example, it controlled the process of uses request. 
SPARQL is a semantic RDF query language able to retrieve and manipulate data stored in Resource Description Framework (RDF) format. So, in my application, I use multiple SPARQL queries to retrieve data from DBpedia and store a local record of the results in a relational database. 

And finally, I used multiple languages for the look and feel of the website. For example, I use HTML5 for the structure of the website, JQuery which is a JavaScript library to add interactivity to the website and compass which is a framework for SASS which is an extension to CSS3 to style the web application and to make it responsive. 

## SQL Database 
I have created a simple SQL relational database using the entity framework code first approach. 

To store all the book information, I have used 3 tables. One is to store all the user searches and the others 2 is used to store the information about the author and his books.

## Design (Optimisation based on time of search and reduce calls across a network)
I have optimised the web application to reduce the number of calls across the network. I’ve done this by creating a local database of the returned results. By doing this it means if someone has already searched for the same thing before there is no need to call for information from another site as all the data needed is already stored locally. 

But I also want the user to have up to date information. So, if the user search is matched to a result in my database it will be checked when the last time the information was updated. If the information is too old the matched results will be removed from the database and SPARQL query will request new information and the local database will be updated. 

Explained this in more detail in the video above.

## Google Maps API
After creating the demo video I had time to add the google map API to the author details page to show where the author was born. 

In the video demo, you may have noticed I displayed the longitude and the latitude of where the author was born. In the updated version, I have used these values to display a google map in their place.

I have also created a custom style for the google map to fit the design of the web application. 

## PagedList NuGet package
For the user search page, I found a NuGet package that allowed me to create pagination for the returned results. I decided to implement this because sometimes the returned result could contain hundreds if not thousands of results and the page would be too long. 


