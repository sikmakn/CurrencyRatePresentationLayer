using System;

namespace ODataClientConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = "http://localhost:64862/";
            var container = new Default.Container(new Uri(uri));

            var serverHandler = new ServerODataHandler(container);

            var userInterface = new UserInterface(serverHandler);
            userInterface.Start();
        }
    }
}
