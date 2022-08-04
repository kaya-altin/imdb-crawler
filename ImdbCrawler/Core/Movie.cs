using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace ImdbCrawler.Core
{
    public partial class Movie : Movies
    {
        public class Search
        {
            private Search()
            {
            }


            public class TMDB
            {
                protected static internal SearchResultCollection Run(string title)
                {
                    return GetMatches(title);
                }

                private static SearchResultCollection GetMatches(string title)
                {
                    try
                    {
                        SearchResultCollection moviesArrayList = new SearchResultCollection();
                        TMDbClient client = new TMDbClient(CrawlerSettings.TmdbApiKey);
                        SearchContainer<SearchMovie> results = client.SearchMovie(title);

                        foreach (SearchMovie result in results.Results)
                            moviesArrayList.Add(new Movie.Search.SearchResult(result.Id.ToString(), result.Title));

                        return moviesArrayList;
                    }
                    catch (System.Exception e)
                    {
                        //XtraMessageBox.Show(e.Message);
                        return null;
                    }

                }


            }

            public class IMDB
            {

                protected static internal SearchResultCollection Run(string title)
                {
                    //return GetMatches("http://www.imdb.com/find?tt=on;nm=on;mx=20;q=", title);
                    return GetMatches("http://www.imdb.com/find?s=all&q=", title);
                }

                protected static internal SearchResultCollection GetExtendedMatches(string title)
                {
                    return GetMatches("http://www.imdb.com/find?more=tt;q=", title);
                }

                private static SearchResultCollection GetMatches(string urlType, string title)
                {
                    try
                    {
                        System.Uri url = new System.Uri(urlType + title.Replace("[\\p{Blank}]+", "%20"));

                        string stringData;
                        System.Uri responseUri = null;
                        stringData = HTTPRetriever._GET.Retrieve(url, System.Text.Encoding.UTF8, out responseUri);
                        SearchResultCollection moviesArrayList = new SearchResultCollection();
                        if (responseUri != null)
                        {
                            //string responseUrl = responseUri.AbsoluteUri;
                            //string ImdbIdSetting = "tt[0-9]{1,15}";
                            //Regex regexImdbId = new Regex(ImdbIdSetting);

                            //Match idMatch = regexImdbId.Match(responseUrl);
                            //if (idMatch.Length > 0)
                            //{
                            //    //id 
                            //    int nStartPos = responseUrl.IndexOf("/tt");
                            //    int endpos;

                            //    string movieId = Convert.ToString(idMatch);

                            //    //title 
                            //    string start = "<title>";
                            //    string end = "</title>";

                            //    nStartPos = stringData.IndexOf(start);
                            //    endpos = stringData.IndexOf(end, nStartPos);
                            //    string movieTitle = stringData.Substring(nStartPos + start.Length, endpos - nStartPos - +start.Length);

                            //    SearchResult result = new SearchResult(movieId, movieTitle);
                            //    moviesArrayList.Add(result);

                            //}

                            //else
                            //{
                            moviesArrayList = GetSearchMatches(stringData);
                            //}
                            return moviesArrayList;
                        }
                        else
                        {
                            return null;

                        }
                    }
                    catch (System.Exception e)
                    {
                        //XtraMessageBox.Show(e.Message);
                        return null;
                    }

                }

                private static SearchResultCollection GetMoviesData(ArrayList moviesRawList)
                {
                    SearchResultCollection imdbArrayList = new SearchResultCollection();

                    SearchResult movie;
                    int truncateLength = 0;
                    int truncateStartLength = 3;
                    Regex regexImdbId = new Regex("tt[0-9]{1,15}");
                    Regex regexImdbTitle = new Regex("/\\D>[\\w\\s,()/>]{1,60}");

                    foreach (string rawData in moviesRawList)
                    {
                        Match idMatch = regexImdbId.Match(rawData);
                        if (idMatch.ToString().Length > 0)
                        {
                            movie = new SearchResult();
                            Match titleMatch = regexImdbTitle.Match(rawData);
                            if (titleMatch.ToString().Length > truncateLength)
                            {
                                movie = new SearchResult();
                                movie.Key = Convert.ToString(idMatch);
                                movie.Title = Convert.ToString(titleMatch);
                                movie.Title = movie.Title.Substring(truncateStartLength, movie.Title.Length - truncateLength - truncateStartLength);
                                imdbArrayList.Add(movie);

                            }
                        }
                    }
                    return imdbArrayList;
                }

                private static SearchResultCollection GetSearchMatches(string stringData)
                {
                    string movieId;
                    SearchResultCollection searchResultCollection = new SearchResultCollection();
                    Regex moviesRegex = new Regex("<td class=\"result_text\"> <a href=\"/title/tt[0-9]{1,15}");

                    //[0-9]{4} 
                    MatchCollection movieMatches;
                    if (stringData.Length > 0)
                    {
                        movieMatches = moviesRegex.Matches(stringData);

                        Regex yearRegex = new Regex("[0-9]{4}");
                        if (movieMatches.Count != 0)
                        {
                            foreach (Match movieMatch in movieMatches)
                            {

                                int nStartPos = stringData.IndexOf("tt", movieMatch.Index);
                                int endpos = stringData.IndexOf("/?ref", nStartPos);

                                movieId = stringData.Substring(nStartPos, endpos - nStartPos);

                                //title 
                                nStartPos = stringData.IndexOf(">", nStartPos);
                                endpos = stringData.IndexOf("</a>", nStartPos);
                                string title = stringData.Substring(nStartPos + 1, endpos - nStartPos - 1);




                                //year 
                                string stringYear = stringData.Substring(endpos, 12);
                                Match yearMatch = yearRegex.Match(stringYear);
                                if (yearMatch != null && yearMatch.Length > 0)
                                {
                                    title += " (" + Convert.ToString(yearMatch) + ")";
                                }

                                if (!string.IsNullOrEmpty(title))
                                {
                                    SearchResult result = new SearchResult(movieId, title);
                                    searchResultCollection.Add(result);
                                }
                            }
                        }
                        else
                        {
                        }
                    }
                    return searchResultCollection;

                }

            }

 
            public class SearchResult
            {
                public virtual string Key
                {
                    get { return _key; }
                    set { _key = value; }
                }

                public virtual string Title
                {
                    get { return _title; }
                    set { _title = HttpUtility.HtmlDecode(value); }
                }

                public virtual string Size
                {
                    get { return _size; }
                    set { _size = value; }
                }

                public virtual string Poster
                {
                    get { return _poster; }
                    set { _poster = value; }
                }


                private string _key;
                private string _title;
                private string _size;
                private string _poster;


                public SearchResult(string key)
                {
                    this.Key = key;
                    this.Title = "";
                }

                public SearchResult(string key, string title)
                {
                    this.Key = key;
                    this.Title = title;
                }

                public SearchResult(string key, string title, string size)
                {
                    this.Key = key;
                    this.Title = title;
                    this.Size = size;
                }

                public SearchResult(string key, string title, string poster, string size)
                {
                    this.Key = key;
                    this.Title = title;
                    this.Poster = poster;
                }

                public SearchResult()
                {
                }

                public override string ToString()
                {
                    return _title;
                }
            }

            public class SearchResultCollection : CollectionBase
            {
                public SearchResultCollection()
                {
                }

                public int Add(SearchResult item)
                {
                    return List.Add(item);
                }

                public SearchResult GetSearchResultByIdenfier(string key)
                {
                    foreach (SearchResult movie in this)
                    {
                        if (movie.Key == key)
                        {
                            return movie;
                        }
                    }
                    return null;
                }
                public SearchResult Item(int index)
                {
                    return (SearchResult)List[index];
                }
            }

        }

        public class Parse
        {
            private Parse()
            {
            }

            public class Tmdb : Movie
            {
                protected internal Tmdb(Movie.Search.SearchResult searchResult, Boolean bPosterDownload)
                {
                    TMDbClient client = new TMDbClient(CrawlerSettings.TmdbApiKey);
                    TMDbLib.Objects.Movies.Movie movie = client.GetMovie(searchResult.Key, MovieMethods.Credits | MovieMethods.Images | MovieMethods.Undefined);

                    this.ContentProvider = (int)Enums.WebType.TMDB;
                    this.ImdbNumber = movie.ImdbId;
                    this.TmdbNumber = Util.convertToString(movie.Id);
                    this.OrginalName = movie.Title;
                    if (movie.ReleaseDate != null)
                        this.Year = movie.ReleaseDate.Value.Year.ToString();
                    this.UserRating = movie.VoteAverage.ToString();
                    this.Votes = Util.convertToDecimalString(movie.VoteCount);
                    this.EnglishPlot = movie.Overview;
                    this.Budget = Util.convertToMoney(movie.Budget.ToString());
                    this.ProductionCompany = String.Join(", ", movie.ProductionCompanies.Select(i => i.Name).ToArray()); ;

                    this.Director = String.Join(", ", movie.Credits.Crew.Where(i => i.Department == "Directing").Select(i => i.Name).ToArray());
                    this.Writer = String.Join(", ", movie.Credits.Crew.Where(i => i.Department == "Writing").Select(i => i.Name).ToArray());
                    this.Genre = GenreReplace(String.Join(", ", movie.Genres.Select(i => i.Name).ToArray()));
                    this.Country = String.Join(", ", movie.ProductionCountries.Select(i => i.Name).ToArray());
                    this.Language = movie.SpokenLanguages.FirstOrDefault().Name;
                    this.RunningTime = movie.Runtime.ToString();
                    this.UserRating = movie.VoteAverage.ToString();

                    this.URL = "https://www.themoviedb.org/movie/" + movie.Id;

                    this.Casts = new List<Casts>();
                    foreach (Cast item in movie.Credits.Cast)
                    {
                        if (!String.IsNullOrEmpty(item.ProfilePath))
                            this.Casts.Add(new Casts(item.Name, item.Character, "https://www.themoviedb.org/person/" + item.Id, "https://image.tmdb.org/t/p/w185" + item.ProfilePath, item.Id.ToString(), this.MovieID, 1));
                        else
                            this.Casts.Add(new Casts(item.Name, item.Character, "https://www.themoviedb.org/person/" + item.Id, "", item.Id.ToString(), this.MovieID, 1));

                        //if (Settings.ShowCastImage && !String.IsNullOrEmpty(item.ProfilePath))
                        //{
                            try
                            {
                                WebClient dl = new WebClient();
                                dl.DownloadFile("https://image.tmdb.org/t/p/w185" + item.ProfilePath, CrawlerSettings.CastPath + item.Id.ToString() + ".jpg");
                                dl.Dispose();
                            }
                            catch (Exception ex)
                            {
                            }
                        //}
                    }


                    if (bPosterDownload)
                    {
                        try
                        {
                            String poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath;
                            this.Poster = poster;

                            WebClient dl = new WebClient();
                            dl.DownloadFile(poster, CrawlerSettings.PosterPath + this.ImdbNumber + ".jpg");
                            dl.Dispose();
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
            }

            public class Imdb : Movie
            {
                protected internal Imdb(Movie.Search.SearchResult searchResult)
                {
                    ImdbRun(searchResult, false);
                }

                protected internal Imdb(Movie.Search.SearchResult searchResult, bool bPosterDownload)
                {
                    ImdbRun(searchResult, bPosterDownload);
                }

                private void ImdbRun(Movie.Search.SearchResult searchResult, bool bPosterDownload)
                {
                    string strHtml;
                    string strStart;
                    string strEnd;
                    string strTemp;
                    int nStartPos = 0;

                    strHtml = HTTPRetriever._GET.Retrieve("http://www.imdb.com/title/" + searchResult.Key + "/");

                    strHtml = Decode(strHtml);

                    this.URL = "http://www.imdb.com/title/" + searchResult.Key;

                    this.ImdbNumber = searchResult.Key;
                    this.ContentProvider = (int)Enums.WebType.IMDB;

                    // Title 
                    strStart = "<title>";
                    strEnd = "</title>";
                    this.OrginalName = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    strTemp = this.OrginalName.Replace("IMDb - ", "");

                    if (this.OrginalName.Contains("("))
                    {
                        strTemp = this.OrginalName.Substring(OrginalName.LastIndexOf("("), OrginalName.Length - OrginalName.LastIndexOf("(")).Trim();
                        this.OrginalName = this.OrginalName.Replace(strTemp, "").Trim();
                    }


                    //if (Settings.The == true && this.OrginalName.StartsWith("The "))
                    //{
                    //    this.OrginalName = this.OrginalName.Substring(4, this.OrginalName.Length - 4) + ", The";
                    //}

                    this.OrginalName = this.OrginalName.Replace("IMDb - ", "");
                    if (this.OrginalName.Length > 255)
                        this.OrginalName = this.OrginalName.Remove(255);


                    strStart = "img_primary";
                    strEnd = "itemprop=\"image\" />";
                    this.Poster = GetSubText(strHtml, strStart, strEnd, "src=\"", "\"", "", nStartPos, ref nStartPos);

                    if (String.IsNullOrEmpty(this.Poster))
                    {
                        strStart = "class=\"poster\"";
                        strEnd = "</div>";
                        this.Poster = GetSubText(strHtml, strStart, strEnd, "src=\"", "\"", "", nStartPos, ref nStartPos);
                    }

                    if (!String.IsNullOrEmpty(this.Poster))
                    {
                        this.Poster = this.Poster.Substring(0, this.Poster.LastIndexOf("._")) + ".UX500.jpg";
                    }

                    // Year 
                    this.Year = Util.RemoveSpecialCharacters(strTemp);
                    if (this.Year.Length > 4)
                        this.Year = this.Year.Remove(4);

                    // User Rating 
                    strStart = @"itemprop=""ratingValue"">";
                    strEnd = "</span>";
                    this.UserRating = GetSubText(strHtml, strStart, strEnd, "", "", "", nStartPos, ref nStartPos);

                    if (String.IsNullOrEmpty(this.UserRating))
                    {
                        this.UserRating = GetText(strHtml, strStart, strEnd, 0, ref nStartPos);
                    }

                    this.UserRating = this.UserRating.Replace(".", ",");
                    if (this.UserRating.Length > 10)
                        this.UserRating = this.UserRating.Remove(10);

                    // Votes 
                    strStart = @"itemprop=""ratingCount"">";
                    strEnd = "</span>";
                    strTemp = Util.RemoveSpecialCharacters(GetSubText(strHtml, strStart, strEnd, "", "", "", nStartPos, ref nStartPos));
                    if (!string.IsNullOrEmpty(strTemp))
                        this.Votes = strTemp;


                    // Plot 
                    strStart = "<div class=\"summary_text\" itemprop=\"description\">";
                    strEnd = "</div>";
                    this.EnglishPlot = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    this.EnglishPlot = StripHTML(EnglishPlot);
                    this.EnglishPlot = StripBlanks(EnglishPlot);
                    if (string.IsNullOrEmpty(this.EnglishPlot))
                    {
                        strStart = "itemprop=\"description\">";
                        strEnd = "</p>";
                        this.EnglishPlot = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                        this.EnglishPlot = StripHTML(EnglishPlot);
                        this.EnglishPlot = StripBlanks(EnglishPlot);
                    }


                    // Director 
                    strStart = "<h4 class=\"inline\">Directors:</h4>";
                    strEnd = "</div>";
                    this.Director = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    if (string.IsNullOrEmpty(this.Director))
                    {
                        strStart = "<h4 class=\"inline\">Director:</h4>";
                        strEnd = "</div>";
                        this.Director = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    }
                    this.Director = StripHTML(Director);
                    this.Director = StripBlanks(Director);
                    if (!string.IsNullOrEmpty(this.Director) && this.Director.Contains("more credit"))
                    {
                        this.Director = this.Director.Remove(this.Director.LastIndexOf(","), this.Director.Length - this.Director.LastIndexOf(","));
                    }
                    if (this.Director.Length > 255)
                        this.Director = this.Director.Remove(255);


                    // Writers 
                    strStart = "<h4 class=\"inline\">Writers:</h4>";
                    strEnd = "</div>";
                    this.Writer = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    if (string.IsNullOrEmpty(this.Writer))
                    {
                        strStart = "<h4 class=\"inline\">Writer:</h4>";
                        strEnd = "</div>";
                        this.Writer = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    }
                    this.Writer = StripHTML(Writer);
                    this.Writer = StripBlanks(Writer);
                    if (this.Writer.Contains("more credit") && this.Writer.Contains(","))
                    {
                        this.Writer = this.Writer.Remove(this.Writer.LastIndexOf(","), this.Writer.Length - this.Writer.LastIndexOf(","));
                    }
                    if (this.Writer.Length > 255)
                        this.Writer = this.Writer.Remove(255);


                    // Genre 
                    strStart = "Genres:";
                    strEnd = "</div>";
                    this.Genre = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    this.Genre = StripHTML(Genre);
                    this.Genre = StripBlanks(Genre);
                    this.Genre = this.Genre.Replace(" See more »", "");
                    this.Genre = this.Genre.Replace(" more", "");
                    this.Genre = GenreReplace(this.Genre);
                    if (this.Genre.Length > 255)
                        this.Genre = this.Genre.Remove(255);


                    // Country 
                    strStart = "Country:";
                    strEnd = "</div>";
                    this.Country = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    this.Country = StripHTML(Country);
                    this.Country = StripBlanks(Country);
                    if (this.Country.Length > 255)
                        this.Country = this.Country.Remove(255);


                    // Language 
                    strStart = "Language:";
                    strEnd = "</div>";
                    this.Language = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    this.Language = StripHTML(Language);
                    this.Language = StripBlanks(Language);
                    if (this.Language.Length > 255)
                        this.Language = this.Language.Remove(255);


                    // ********* Budget ********* //
                    strStart = "<h4 class=\"inline\">Budget:</h4> ";
                    strEnd = "</div>";
                    this.Budget = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    if (!String.IsNullOrEmpty(this.Budget))
                    {
                        this.Budget = StripHTML(this.Budget);
                        this.Budget = this.Budget.Replace("(estimated)", "");
                        this.Budget = StripBlanks(this.Budget).Replace(",", ".");
                    }

                    // ********* Production Company ********* //
                    strStart = "<h4 class=\"inline\">Production Co:</h4>";
                    strEnd = "</div>";
                    this.ProductionCompany = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    if (!String.IsNullOrEmpty(this.ProductionCompany))
                    {
                        this.ProductionCompany = StripHTML(this.ProductionCompany);
                        this.ProductionCompany = StripBlanks(this.ProductionCompany);
                        if (this.ProductionCompany.Contains("See more"))
                        {
                            this.ProductionCompany = this.ProductionCompany.Substring(0, this.ProductionCompany.LastIndexOf("See more")).Trim();
                        }

                    }


                    // Runtime 
                    strStart = "<h4 class=\"inline\">Runtime:</h4> ";
                    strEnd = "</div>";
                    this.RunningTime = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);
                    this.RunningTime = StripHTML(RunningTime);
                    this.RunningTime = StripBlanks(RunningTime);
                    if (!string.IsNullOrEmpty(this.RunningTime))
                        this.RunningTime = this.RunningTime.Replace("min", "");
                    if (this.RunningTime.Length > 255)
                        this.RunningTime = this.RunningTime.Remove(255);



                    //getCast(this.ImdbNumber);


                    if (bPosterDownload == true & !String.IsNullOrEmpty(this.Poster))
                    {

                        try
                        {
                            WebClient dl = new WebClient();
                            dl.DownloadFile(this.Poster, CrawlerSettings.PosterPath + searchResult.Key + ".jpg");
                            dl.Dispose();

                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                private void getCast(String imdbNumber)
                {
                    this.Casts = new List<Casts>();

                    string strHtml = HTTPRetriever._GET.Retrieve("http://imdb.com/title/" + imdbNumber + "/fullcredits#cast");

                    String strStart = "<table class=\"cast_list\">";
                    String strEnd = "</table>";
                    int nStartPos = 0;

                    String strCast = GetText(strHtml, strStart, strEnd, nStartPos, ref nStartPos);

                    Regex castRegex = new Regex("<tr class=\"odd\">|<tr class=\"even\">");
                    MatchCollection castMatches = castRegex.Matches(strCast);
                    if (castMatches.Count > 0)
                    {
                        foreach (Match matcher in castMatches)
                        {
                            int iStartIndex = matcher.Index;
                            int iStartPos = 0;
                            int iEndPos = 0;

                            iStartPos = strCast.IndexOf("href=\"/name/", iStartIndex);
                            iEndPos = strCast.IndexOf("/?ref", iStartPos);
                            String key = strCast.Substring(iStartPos + 12, iEndPos - iStartPos - 12);

                            iStartPos = strCast.IndexOf("<span class=\"itemprop\" itemprop=\"name\">", iStartPos);
                            iEndPos = strCast.IndexOf("</span>", iStartPos);
                            String actor = strCast.Substring(iStartPos + 39, iEndPos - iStartPos - 39);
                            actor = StripHTML(actor);

                            iStartPos = strCast.IndexOf("<td class=\"character\">", iStartPos);
                            iEndPos = strCast.IndexOf("</td>", iStartPos);
                            String character = strCast.Substring(iStartPos + 22, iEndPos - iStartPos - 22);
                            character = StripHTML(character);
                            character = StripBlanks(character);
                            character = character.Replace("&nbsp;", "");

                            String poster = "";
                            iStartPos = strCast.IndexOf("loadlate=\"", iStartIndex);
                            if (iStartPos != -1 && iStartPos < iEndPos)
                            {
                                try
                                {
                                    iEndPos = strCast.IndexOf(".jpg", iStartPos);
                                    poster = strCast.Substring(iStartPos + 10, iEndPos - iStartPos - 6);
                                    poster = poster.Substring(0, poster.LastIndexOf("._")) + "._V1_UX150.jpg";
                                }
                                catch
                                {
                                    poster = "";
                                }
                            }


                            this.Casts.Add(new Casts(actor, character, "http://www.imdb.com/name/" + key + "/", poster, key, this.MovieID, 1));

                           // if (Settings.ShowCastImage && !String.IsNullOrEmpty(poster))
                            //{
                                try
                                {
                                    WebClient dl = new WebClient();
                                    dl.DownloadFile(poster + ".jpg", CrawlerSettings.CastPath + key + ".jpg");
                                    dl.Dispose();
                                }
                                catch (Exception)
                                {
                                }
                            //}
                        }



                    }
                }

                public static string getPoster(string cmd)
                {
                    System.Net.HttpWebRequest req;
                    System.Net.HttpWebResponse res;
                    System.IO.StreamReader sr;
                    string strHtml;

                    req = (HttpWebRequest)System.Net.WebRequest.Create("http://imdb.com/title/" + cmd + "/");

                    res = (HttpWebResponse)req.GetResponse();

                    sr = new StreamReader(res.GetResponseStream(), Encoding.Default);

                    strHtml = sr.ReadToEnd();

                    sr.Close();
                    res.Close();

                    int posterIndex = 0;
                    if (strHtml != null)
                    {
                        posterIndex = strHtml.IndexOf("<td rowspan=\"2\" id=\"img_primary\">");
                    }

                    if (posterIndex > 0)
                    {
                        int displayingIndex = strHtml.IndexOf("src=\"", posterIndex);
                        int results = displayingIndex + "src=\"".Length;
                        int endResults = strHtml.IndexOf("\"", results);

                        return strHtml.Substring(results, endResults - results);
                    }
                    else
                    {

                        return string.Empty;
                    }
                }


            }




            #region Helpers
            static internal string Decode(string Input)
            {
                if (string.IsNullOrEmpty(Input))
                    return "";

                Input = Input.Replace("&#34;", "\"");
                Input = Input.Replace("&#39;", "`");
                Input = Input.Replace("&#38;", "&");
                Input = Input.Replace("&#60;", "less");
                Input = Input.Replace("&#62;", "greater");

                Input = Input.Replace("&#160;", " ");
                Input = Input.Replace("&#161;", "!");
                Input = Input.Replace("&#164;", "currency");
                Input = Input.Replace("&#162;", "cent");
                Input = Input.Replace("&#163;", "pound");
                Input = Input.Replace("&#165;", "yen");
                Input = Input.Replace("&#166;", "|");
                Input = Input.Replace("&#167;", "section");
                Input = Input.Replace("&#168;", "..");
                Input = Input.Replace("&#169;", "(C)");
                Input = Input.Replace("&#170;", "a");
                Input = Input.Replace("&#171;", "``");

                Input = Input.Replace("&#172;", "not");
                Input = Input.Replace("&#173;", "-");
                Input = Input.Replace("&#174;", "(R)");
                Input = Input.Replace("&#8482;", "TM");
                Input = Input.Replace("&#175;", "-");
                Input = Input.Replace("&#176;", "o");
                Input = Input.Replace("&#177;", "+/-");
                Input = Input.Replace("&#178;", "^2");
                Input = Input.Replace("&#179;", "^3");
                Input = Input.Replace("&#180;", "`");
                Input = Input.Replace("&#181;", "u");
                Input = Input.Replace("&#182;", "P");
                Input = Input.Replace("&#183;", ".");
                Input = Input.Replace("&#184;", ",");
                Input = Input.Replace("&#185;", "^1");
                Input = Input.Replace("&#186;", "o");
                Input = Input.Replace("&#187;", "``");

                Input = Input.Replace("&#188;", "1/4");
                Input = Input.Replace("&#189;", "1/2");
                Input = Input.Replace("&#190;", "3/4");
                Input = Input.Replace("&#191;", "?");
                Input = Input.Replace("&#215;", "x");
                Input = Input.Replace("&#247;", "/");
                Input = Input.Replace("&#192;", "A");
                Input = Input.Replace("&#193;", "A");
                Input = Input.Replace("&#194;", "A");
                Input = Input.Replace("&#195;", "A");
                Input = Input.Replace("&#196;", "A");
                Input = Input.Replace("&#197;", "A");
                Input = Input.Replace("&#198;", "AE");
                Input = Input.Replace("&#199;", "Ç");
                Input = Input.Replace("&#200;", "E");
                Input = Input.Replace("&#201;", "E");
                Input = Input.Replace("&#202;", "E");
                Input = Input.Replace("&#203;", "E");
                Input = Input.Replace("&#204;", "I");
                Input = Input.Replace("&#205;", "I");
                Input = Input.Replace("&#206;", "I");
                Input = Input.Replace("&#207;", "I");
                Input = Input.Replace("&#208;", "D");
                Input = Input.Replace("&#209;", "N");
                Input = Input.Replace("&#210;", "O");
                Input = Input.Replace("&#211;", "O");
                Input = Input.Replace("&#212;", "O");
                Input = Input.Replace("&#213;", "O");
                Input = Input.Replace("&#214;", "Ö");
                Input = Input.Replace("&#216;", "O");
                Input = Input.Replace("&#217;", "U");
                Input = Input.Replace("&#218;", "U");
                Input = Input.Replace("&#219;", "U");
                Input = Input.Replace("&#220;", "Ü");
                Input = Input.Replace("&#221;", "Y");
                Input = Input.Replace("&#222;", "P");
                Input = Input.Replace("&#223;", "ss");
                Input = Input.Replace("&#224;", "a");
                Input = Input.Replace("&#225;", "a");
                Input = Input.Replace("&#226;", "a");
                Input = Input.Replace("&#227;", "a");
                Input = Input.Replace("&#228;", "a");
                Input = Input.Replace("&#229;", "a");
                Input = Input.Replace("&#230;", "ae");
                Input = Input.Replace("&#231;", "ç");
                Input = Input.Replace("&#232;", "e");
                Input = Input.Replace("&#233;", "e");
                Input = Input.Replace("&#234;", "e");
                Input = Input.Replace("&#235;", "e");
                Input = Input.Replace("&#236;", "i");
                Input = Input.Replace("&#237;", "i");
                Input = Input.Replace("&#238;", "i");
                Input = Input.Replace("&#239;", "i");
                Input = Input.Replace("&#240;", "eth");
                Input = Input.Replace("&#241;", "n");
                Input = Input.Replace("&#242;", "o");
                Input = Input.Replace("&#243;", "o");
                Input = Input.Replace("&#244;", "o");
                Input = Input.Replace("&#245;", "o");
                Input = Input.Replace("&#246;", "ö");
                Input = Input.Replace("&#248;", "o");
                Input = Input.Replace("&#249;", "u");
                Input = Input.Replace("&#250;", "u");
                Input = Input.Replace("&#251;", "u");
                Input = Input.Replace("&#252;", "ü");
                Input = Input.Replace("&#253;", "y");
                Input = Input.Replace("&#254;", "p");
                Input = Input.Replace("&#255;", "y");

                Input = Input.Replace("&#338;", "OE");
                Input = Input.Replace("&#339;", "oe");
                Input = Input.Replace("&#352;", "S");
                Input = Input.Replace("&#353;", "s");
                Input = Input.Replace("&#376;", "Y");
                Input = Input.Replace("&#710;", "^");
                Input = Input.Replace("&#732;", "~");
                Input = Input.Replace("&#8194;", " ");
                Input = Input.Replace("&#8195;", " ");
                Input = Input.Replace("&#8201;", " ");
                Input = Input.Replace("&#8204;", "|");
                Input = Input.Replace("&#8205;", "|");
                Input = Input.Replace("&#8206;", "|");
                Input = Input.Replace("&#8207;", "|");
                Input = Input.Replace("&#8211;", "-");
                Input = Input.Replace("&#8212;", "-");
                Input = Input.Replace("&#8216;", "`");
                Input = Input.Replace("&#8217;", "`");
                Input = Input.Replace("&#8218;", "`");
                Input = Input.Replace("&#8220;", "``");
                Input = Input.Replace("&#8221;", "``");
                Input = Input.Replace("&#8222;", "``");
                Input = Input.Replace("&#8224;", "+");
                Input = Input.Replace("&#8225;", "++");
                Input = Input.Replace("&#8230;", "...");
                Input = Input.Replace("&#8240;", "0/00");
                Input = Input.Replace("&#8249;", "(");
                Input = Input.Replace("&#8250;", ")");
                Input = Input.Replace("&#8264;", "euro");
                Input = Input.Replace("&#x27;", "'");
                Input = Input.Replace("&#xB7;", "-");
                Input = Input.Replace("&#xBD;", "½");
                Input = Input.Replace("&#xE8;", "è");
                Input = Input.Replace("&#xE9;", "è");
                Input = Input.Replace("&#xF4;", "ô");
                Input = Input.Replace("&#xE4;", "ä");
                Input = Input.Replace("&#xF9;", "ù");
                Input = Input.Replace("&#xE5;", "å");


                return Input;

            }

            static internal string GetSubText(string strHtml, string strStart, string strEnd, string strStartSubString, string strEndSubString, string delimiter, int nStart, ref int nEnd)
            {
                int nStringStart = 0;
                string strSubItem = null;
                string strSubItems = null;
                string strSubItemsData = null;

                string strReturnValue = null;

                strSubItems = String.Empty;

                strSubItemsData = GetText(strHtml, strStart, strEnd, nStart, ref nStart);

                strSubItem = GetText(strSubItemsData, strStartSubString, strEndSubString, nStringStart, ref nStringStart);
                strSubItems = strSubItem;
                while (strSubItem != string.Empty)
                {
                    strSubItem = GetText(strSubItemsData, strStartSubString, strEndSubString, nStringStart, ref nStringStart);

                    if (strSubItem.Length > 0)
                    {
                        strSubItems += delimiter + strSubItem;
                    }
                }
                strReturnValue = strSubItems;

                nEnd = nStart;
                return strReturnValue;
            }

            static internal string GetText(string strData, string strStartString, string strEndString, int nStartPos, ref int nEndPos)
            {
                int nStart = strData.IndexOf(strStartString, nStartPos);
                if (nStart == -1)
                {
                    nEndPos = nStartPos;
                    return string.Empty;
                }
                nStart += strStartString.Length;

                string strEndSearchString = strEndString;

                int nEnd = 0;
                if (strEndSearchString.Length == 0)
                {
                    nEnd = strData.Length;
                }
                else
                {
                    nEnd = strData.IndexOf(strEndSearchString, nStart);
                }
                if (nEnd == -1)
                {
                    nEndPos = nStartPos;
                    return string.Empty;
                }
                nEnd -= nStart;

                string strResult = strData.Substring(nStart, nEnd);

                nEndPos = nStart + nEnd + strEndString.Length;
                return strResult;
            }

            static internal string StripHTML(string strHtml)
            {
                if (String.IsNullOrEmpty(strHtml))
                    return null;

                return (Regex.Replace(strHtml, "<[^>]*>", ""));
            }

            static internal string StripBlanks(string strText)
            {
                if (String.IsNullOrEmpty(strText))
                    return String.Empty;

                //strText = strText.Replace("\r", "");
                // strText = strText.Replace("\n", "");
                //strText = System.Text.RegularExpressions.Regex.Replace(strText, @"\s+", " ").Trim();
                //strText = strText.Trim();

                return System.Text.RegularExpressions.Regex.Replace(strText, @"\s+", " ").Trim();
            }

            static internal string RemoveBlanks(string str)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    switch (c)
                    {
                        case '\r':
                        case '\n':
                        case '\t':
                        case ' ':
                            continue;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
                return sb.ToString();
            }


            static internal string StripBlanks2(string strText)
            {
                if (String.IsNullOrEmpty(strText))
                    return String.Empty;

                //strText = strText.Replace("\r", "");
                // strText = strText.Replace("\n", "");
                //strText = System.Text.RegularExpressions.Regex.Replace(strText, @"\s+", " ").Trim();
                //strText = strText.Trim();

                return System.Text.RegularExpressions.Regex.Replace(strText, @"\s", " ").Trim();
            }

            static internal string GenreReplace(string strInput)
            {
                //if (!string.IsNullOrEmpty(strInput))
                //{
                //    Section genres = GrieeX.GrieeXBase.Language.Sections.Where(c => c.Name == "Genres").FirstOrDefault();

                //    if (genres != null)
                //    {
                //        foreach (GrieeX.GrieeXBase.Key item in genres.Keys)
                //        {
                //            strInput = strInput.Replace(item.Name, GrieeX.GrieeXBase.Language.FindKey("Genres", item.Name).Value);
                //        }
                //    }
                //}

                return strInput;
            }

            #endregion
        }

    }
}
