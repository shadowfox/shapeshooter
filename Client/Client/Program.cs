using System;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Client client = new Client())
            {
                client.Run();
            }
        }
    }
}

