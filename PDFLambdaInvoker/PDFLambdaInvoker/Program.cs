using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;

namespace PDFLambdaInvoker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new AmazonLambdaClient
            AmazonLambdaClient client = new AmazonLambdaClient("accessKey", "aws_SecretKey", RegionEndpoint.USWest2);

            //Create new InvokeRequest with published function name.
            InvokeRequest invoke = new InvokeRequest
            {
                FunctionName = "MyNewFunction",
                InvocationType = InvocationType.RequestResponse,
                Payload = "\"Test\""
            };

            //Get the invokeResponse from client InvokeRequest
            InvokeResponse response = client.Invoke(invoke);

            //Read the response stream
            var stream = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(stream);
            var serializer = new JsonSerializer();
            var respsonseText = serializer.Deserialize(reader);

            //Convert output to PDF
            byte[] bytes = Convert.FromBase64String(respsonseText.ToString());
            FileStream fileStream = new FileStream("Magak_CV.pdf", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fileStream);
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            System.Diagnostics.Process.Start("Magak_CV.pdf");

        }
    }
}
