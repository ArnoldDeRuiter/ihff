using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ihff.Models;
using Newtonsoft.Json.Linq;

namespace ihff.Controllers.Api
{
    public class ImdbParser
    {
        public string GetImdbId(Item item)
        {
            // Zoek film op in IMDB databse aan de hand van Item naam en Year
            // Return is een JSON
            var json = new WebClient().DownloadString("http://www.omdbapi.com/?t=" + item.Name + "&y=" + item.Year + "&plot=short&r=json");

            // Parse JSON en pak de ImdbId waarde
            JObject o = JObject.Parse(json);
            string imdbID = (string)o.GetValue("imdbID");

            return imdbID;
        }

        public string GetImdbPoster(Item item)
        {
            // Zoek film op in IMDB databse aan de hand van Item naam en Year
            // Return is een JSON
            var json = new WebClient().DownloadString("http://www.omdbapi.com/?t=" + item.Name + "&y=" + item.Year + "&plot=short&r=json");

            // Parse JSON en pak de Poster waarde
            JObject o = JObject.Parse(json);
            string imdbPoster = (string)o.GetValue("Poster");

            return imdbPoster;
        }
    }
}