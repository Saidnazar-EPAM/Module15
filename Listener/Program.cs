using System.Net;
using System.Reflection;

public class Program
{

    public static bool runServer = true;
    public static HttpListenerContext context;
    public static HttpListenerRequest request;
    public static HttpListenerResponse response;
    public static string responseString;
    public static void Main()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/");
        listener.Start();
        Console.WriteLine("Listening...");

        while (runServer)
        {
            context = listener.GetContext();
            request = context.Request;
            response = context.Response;
            responseString = null;
            Execute();


            if (responseString != null)
            {
                Stream output = response.OutputStream;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            response.Close();
        }

        listener.Stop();
        Console.WriteLine("Stopped.");
    }
    public static void Execute()
    {
        var path = request.Url.AbsolutePath.Replace("/", "").Trim();

        var methodName = request.HttpMethod + path;

        MethodInfo methodToExecute = typeof(Program)
            .GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        if (methodToExecute == null)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        methodToExecute.Invoke(null, null);
    }

    public static void GetMyName()
    {
        responseString = "Saidnazar";
    }
    public static void GetInformation()
    {
        response.StatusCode = 103;
    }
    public static void GetSuccess()
    {
        response.StatusCode = 200;
    }
    public static void GetRedirection()
    {
        response.StatusCode = 300;
    }
    public static void GetClientError()
    {
        response.StatusCode = 400;
    }
    public static void GetServerError()
    {
        response.StatusCode = 500;
    }

    public static void GetMyNameByHeader()
    {
        response.Headers.Add("X-MyName", "Saidnazar");
    }

    public static void GetMyNameByCookie()
    {
        response.Cookies.Add(new Cookie("MyName", "Saidnazar"));
    }

    public static void GetTerminate()
    {
        responseString = "Server stopped.";
        runServer = false;
    }
}