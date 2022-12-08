using Newtonsoft.Json; //JSON Parsing
using System;
using System.Net.Http; //HTTP Requests
using System.Threading; //Thread control
using System.Threading.Tasks; //Async/Await Functions

namespace Anime_Details_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Anime name: ");
                string animeName = Console.ReadLine();
                Console.WriteLine("------------------------");

                //Calls AnimePahe API to get search results based on the 
                //anime name
                try
                {
                    var results = GetAnimeLink(animeName.ToLower()).GetAwaiter().GetResult();

                    //Restarts the loop if an invalid anime name is entered
                    if (results.Data == null)
                    {
                        Console.WriteLine("Anime not found.");
                        Console.WriteLine("------------------------");
                        Thread.Sleep(3000);
                        continue;
                    }

                    //Moves through each search result and prints its information to the screen
                    foreach (var result in results.Data)
                    {
                        Console.WriteLine($"Name: {result.Title}");
                        Console.WriteLine($"Status: {result.Status}");
                        Console.WriteLine($"Episodes: {result.Episodes}");
                        Console.WriteLine($"Type: {result.Type}");
                        Console.WriteLine($"Year: {result.Year}");
                        Console.WriteLine("------------------------");
                    }

                    Thread.Sleep(3000);
                }

                //Restarts loop if there is no internet connection
                catch (HttpRequestException)
                {
                    Console.WriteLine("Please make sure you are connected to the Internet.");
                    Console.WriteLine("------------------------");
                    Thread.Sleep(3000);
                    continue;
                }
            }
        }

        //Accesses AnimePahe's search API and gets the reponses
        static async Task<SearchResults> GetAnimeLink(string query)
        {
            //Formats the query to get accurate results 
            query = query.Replace(" ", "%20");
            query = query.Replace(":", "%3A");

            var clientHandler = new HttpClientHandler
            {
                UseCookies = false,
            };
            var client = new HttpClient(clientHandler);

            //Makes a GET request to said API
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://animepahe.com/api?m=search&q={query}"),
                Headers =
                {
                    { "cookie", "amp=1; res=720; aud=jpn; av1=0; SERVERID=janna; latest=4876; __cf_bm=SkObgksQvQ3d0IS7BnItizCkotitstO2facIGmdzkws-1664197114-0-AbvaDwo7LzwbJS5FAqYmnq9/wWdP/u/mzjoywGOBcOf2e13tRlWZ8jxBtGcXPT+rvg==; XSRF-TOKEN=eyJpdiI6Im5YbWZORjN2WXR4TnJaTzF4LzZFanc9PSIsInZhbHVlIjoiSWhUQnJOaksyeUNhL2V4U1NqK256dXl2alBldWhxMmNJTWF4YTlBSXlJWkxNc0JkYWdFUjB5azYxTldXdlhndHlwNks5UVUwL1pmOC9GUVBJZm1yeU5wZlA2UmI4MjhYUkJ6b0lTNkpLWHNhN3VaZWtUWHpqa1dESkZKUDh2RDYiLCJtYWMiOiI1OGI4MzgwYjY0NWUxMGJlNWNlMDkxYTk2YmExZTllMDFlNTAxNDFkZTc5OWZlNDJhZmZlZmNiY2E2ZGE4ODRjIiwidGFnIjoiIn0^%^3D; laravel_session=eyJpdiI6Im9XZnp2Vk0zcTB5RnlyT2NNbXNlZHc9PSIsInZhbHVlIjoiSFZacTR3bHRtR1NLRWUvd2toaFAzSm9UTU5KRHNUaXBjeWsvQ1BKbnlxdHV1TzI2ekZjejFXTEpOUmhDaXFYcHJzV0FhMEJiZkNMZXdqdlhkTjRZNUNYbHRlY2F3WnhmV2F2d1lNWFBCOStMT1Vsd2c0NXJ5VVdOa296R1VKRzEiLCJtYWMiOiJmMjc2Mzk4MTFjNDIzNGRjYTIzYzZiOGQxZTI1NTI4Y2RlM2QwODIwNWYwMmZkODVjMWU0N2VjYjBmMzBkODhlIiwidGFnIjoiIn0^%^3D" },
                    { "authority", "animepahe.com" },
                    { "accept", "application/json, text/javascript, */*; q=0.01" },
                    { "accept-language", "en-US,en;q=0.9" },
                    { "cache-control", "no-cache" },
                    { "dnt", "1" },
                    { "pragma", "no-cache" },
                    { "referer", "https://animepahe.com/" },
                    { "sec-ch-ua", @"^\^Microsoft" },
                },
            };

            //Gets the JSON response and parses it into a SearchResults class object
            //Returns the class object
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                SearchResults result = JsonConvert.DeserializeObject<SearchResults>(body);
                return result;
            }
        }


    }
}