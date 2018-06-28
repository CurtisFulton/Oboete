using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MigrationProject
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
                Console.WriteLine("error");

            var cts = new CancellationTokenSource();

            var Task = StartListener(cts.Token);

            Console.ReadKey();

            cts.Cancel();

            Task.Wait();
        }

        private static async Task StartListener(CancellationToken token)
        {
            var prefixes = new string[] { "http://*:8080/test/" };

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            var listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes) {
                listener.Prefixes.Add(s);
            }
            listener.Start();

            Console.WriteLine("Listening...");
            token.Register(() => listener.Abort());

            while (!token.IsCancellationRequested) {
                HttpListenerContext context;

                try {
                    context = await listener.GetContextAsync().ConfigureAwait(false);

                    HandleRequest(context); // Note that this is *not* awaited

                    Console.WriteLine("Response Sent");
                } catch {
                    // Handle errors
                }
            }
        }

        async static Task HandleRequest(HttpListenerContext context)
        {
            await Task.Run(() => {
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            });
        }

        private static string Pyramid(int n)
        {
            // Programming Challenge
            string result = "";
            int charPerRow = ((n - 1) * 2) + 1;
            for (int i = n - 1; i >= 0; i--) {
                string row = new String('#', charPerRow - (2 * i));
                row = row.PadLeft(charPerRow - i);
                row = row.PadRight(charPerRow);
                result += $"{row}\r\n";
            }
            return result;
        }
    }
}
