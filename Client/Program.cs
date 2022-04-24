using System.Net;

CookieContainer cookies = new CookieContainer();
HttpClientHandler handler = new HttpClientHandler();
handler.CookieContainer = cookies;

HttpClient client = new HttpClient(handler);

var baseUri = new Uri("http://localhost:8888/");

var uri = new Uri(baseUri, "MyName");

var response = await client.GetAsync(uri);
var responseString = await response.Content.ReadAsStringAsync();
Console.WriteLine(responseString);


uri = new Uri(baseUri, "Information");
try
{
    response = await client.GetAsync(uri);
}
catch (HttpRequestException ex)
{
    Console.WriteLine(HttpStatusCode.EarlyHints);
}

uri = new Uri(baseUri, "Success");
response = await client.GetAsync(uri);
Console.WriteLine(response.StatusCode.ToString());

uri = new Uri(baseUri, "Redirection");
response = await client.GetAsync(uri);
Console.WriteLine(response.StatusCode.ToString());

uri = new Uri(baseUri, "ClientError");
response = await client.GetAsync(uri);
Console.WriteLine(response.StatusCode.ToString());

uri = new Uri(baseUri, "ServerError");
response = await client.GetAsync(uri);
Console.WriteLine(response.StatusCode.ToString());

uri = new Uri(baseUri, "MyNameByHeader");
response = await client.GetAsync(uri);
Console.WriteLine(response.Headers.FirstOrDefault(b => b.Key == "X-MyName").Value.FirstOrDefault());

uri = new Uri(baseUri, "MyNameByCookie");
response = await client.GetAsync(uri);
var cookie = cookies.GetCookies(uri).FirstOrDefault(b => b.Name == "MyName");
Console.WriteLine(cookie?.Value);

uri = new Uri(baseUri, "Terminate");
response = await client.GetAsync(uri);
responseString = await response.Content.ReadAsStringAsync();
Console.WriteLine(responseString);

Console.ReadKey();