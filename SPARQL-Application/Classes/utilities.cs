using System;
using VDS.RDF.Query;

namespace SPARQL_Application.Classes
{
    public static class Utilities
    {
        //Creates the multiple different Sparql queries
        public static string QueryUserSearchBookName(string userSearch)
        {
            string query =
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                "PREFIX ontology: <http://dbpedia.org/ontology/> " +
                "SELECT distinct ?s ?bookName ?authorLink ?author  WHERE { " +
                    "?s rdf:type ontology:Book; " +
                       "rdfs:label ?bookName; " +
                       "ontology:author ?authorLink. " +
                    "?authorLink rdfs:label ?author " +

                    "FILTER ( regex (str(?bookName), '" + userSearch + "', 'i') ). " +
                    "FILTER (lang(?author) = 'en') " +
                    "FILTER (lang(?bookName) = 'en') " +
                "} ";

            return query;
        }

        public static string QueryAuthorBooks(string authorLink)
        {
            String query =
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                "PREFIX ontology: <http://dbpedia.org/ontology/> " +
                "SELECT distinct ?authorLink ?bookLink ?bookName ?bookAbstract (MAX(?numberOfPages) as ?numberOfPages) ?comment WHERE { " +
                    "?bookLink rdf:type ontology:Book; " +
                        "rdfs:label ?bookName; " +
                        "dbo:abstract ?bookAbstract; " +
                        "rdfs:comment ?comment; " +
                        "ontology:author ?authorLink. " +
                        "OPTIONAL {?bookLink dbo:numberOfPages ?numberOfPages.}" +
                    "FILTER(regex (str(?authorLink), '" + authorLink + "' , 'i') ). " +
                    "FILTER(lang(?bookName) = 'en') " +
                    "FILTER(lang(?bookAbstract) = 'en') " +
                    "FILTER(lang(?comment) = 'en') " +
                "}";
            return query;
        }

        public static string QueryAuthorDetails(string authorLink)
        {
            String query =
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                "PREFIX ontology: <http://dbpedia.org/ontology/> " +
                "SELECT distinct ?authorLink ?authorName ?placeOfBirthLink ?PlaceOfBirth ?latitude ?longitude WHERE { " +
                    "?authorLink a dbo:Person; " +
                        "rdfs:label ?authorName; " +
                        "dbo:birthPlace ?placeOfBirthLink. " +
                    "?placeOfBirthLink rdfs:label ?PlaceOfBirth; " +
                        "geo:lat ?latitude; " +
                        "geo:long ?longitude. " +

                    "FILTER(regex (str(?authorLink), '" + authorLink + "', 'i') ). " +
                    "FILTER(lang(?authorName) = 'en') " +
                    "FILTER(lang(?PlaceOfBirth) = 'en') " +
                "}";
            return query;
        }

        //Method to Query Dbpedia and return a Sparql Result set
        public static SparqlResultSet QueryDbpedia(string query)
        {
            //Define a remote endpoint
            //Use the DBPedia SPARQL endpoint with the default Graph set to DBPedia
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org");

            //Make a SELECT query against the Endpoint
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            return results;
        }

        //Method to remove the @en at the end of strings
        public static string RemoveLast3Cahracters(string word)
        {
            string result = String.Empty;

            if (word.Length > 3)
            {
                result = word.Substring(0, word.Length - 3);
            }

            return result;
        }

        //Numbers received from dbpedia are not purely number it also a link. This method remove the link
        public static string NumberConverter(string word)
        {
            string result = String.Empty;

            if (!string.IsNullOrEmpty(word))
            {
                int index = word.IndexOf("^", StringComparison.Ordinal);
                result = word.Substring(0, index);
            }

            return result;
        }
    }
}