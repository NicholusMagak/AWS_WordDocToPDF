using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Drawing;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MyLamdaProject
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /*public string FunctionHandler(string input, ILambdaContext context)
        {
            return input?.ToUpper();
        }*/

        //Convertion function located below
        public string FunctionHandler(string input, ILambdaContext context)
        {
            //Holds the location path
            string filePath = Path.GetFullPath(@"Data/Nicholus Magak CV.docx");

            //Load the file from the disk
            FileStream filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            WordDocument document = new WordDocument(filestream, FormatType.Docx);

            DocIORenderer renderer = new DocIORenderer();
            PdfDocument pdf = renderer.ConvertToPDF(document);

            //save document into the stream
            MemoryStream stream = new MemoryStream();
            //save the PDF doc in the memory
            pdf.Save(stream);
            document.Close();
            renderer.Dispose();
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}
