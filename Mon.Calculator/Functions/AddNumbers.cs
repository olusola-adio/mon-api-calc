using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Mon.Calculator.Models;

namespace Mon.Calculator.Functions
{
    public class AddNumbers
    {
        [FunctionName("AddNumbers")]
        [Display(Name = "Get detail by SOC", Description = "Retrieves a job-group detail.")]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Detail retrieved", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Invalid request data", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.InternalServerError, Description = "Internal error caught and logged", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is unknown or invalid", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = "Insufficient access", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.TooManyRequests, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string input1 = req.Query["input1"];
            string input2 = req.Query["input2"];

            var responseObject = new ResponseObject();
            try
            {
                var number1 = double.Parse(input1);
                var number2 = double.Parse(input2);

                await Task.Run(() => responseObject.Result = (number1 + number2).ToString());
                log.LogInformation("{0} + {1} = {2}", input1, input2, responseObject.Result);
            }
            catch (Exception exception)
            {
                await Task.Run(() => responseObject.Result = exception.Message);
                log.LogError(exception.StackTrace);
                log.LogError("Crap info sent through {0} and {1}", input1, input2);
            }

            return new OkObjectResult(responseObject);
        }
    }
}

