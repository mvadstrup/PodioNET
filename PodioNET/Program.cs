using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PodioNET.Models;
using RestSharp;

namespace PodioNET
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new PodioClient();

            var connected = client.Authenticate("podiousername", "password");

            Console.WriteLine("Is connected: " + connected);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;
            request.Resource = "/file//";

            var file = client.RestExecute<List<PodioFile.FileResponse>>(request);

            foreach (var fileResponse in file)
            {
                Console.Write(fileResponse.Name);
            }

        


        }
    }
}
