using GrapeCity.Documents.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DioDocsFileIOFunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("output/result.xlsx", FileAccess.Write)] Stream outputfile,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string Message = string.IsNullOrEmpty(name)
                ? "Hello, World!!"
                : $"Hello, {name}!!";

            //Workbook.SetLicenseKey("");

            Workbook workbook = new Workbook();

            workbook.Worksheets[0].Range["B2"].Value = Message;

            workbook.Save(outputfile);

            return new OkObjectResult("Finished.");
        }
    }
}
