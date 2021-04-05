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
        private const string FunctionName = "AddNumbers";
        private readonly ILogger<AddNumbers> logger;

        public AddNumbers(
           ILogger<AddNumbers> logger)
        {
            this.logger = logger;
        }

        [FunctionName(FunctionName)]
        [Display(Name = "Add 2 numbers", Description = "Add two numbers and return result.")]
        [ProducesResponseType(typeof(ResponseObject), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Detail retrieved", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Invalid request data", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.InternalServerError, Description = "Internal error caught and logged", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is unknown or invalid", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = "Insufficient access", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.TooManyRequests, Description = "Too many requests being sent, by default the API supports 150 per minute.", ShowSchema = false)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AddNumbers/{input1}/{input2}")] HttpRequest req,
                                             [FromQuery] string input1,
                                             [FromQuery] string input2)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");


            var responseObject = new ResponseObject();
            try
            {
                var number1 = double.Parse(input1);
                var number2 = double.Parse(input2);

                await Task.Run(() => responseObject.Result = (number1 + number2).ToString());
                logger.LogInformation($"{input1} + {input2} = {responseObject.Result}");
            }
            catch (Exception exception)
            {
                await Task.Run(() => responseObject.Result = exception.Message);
                logger.LogError(exception.StackTrace);
                logger.LogError($"Crap info sent through {input1} and {input2}");
            }

            return new OkObjectResult(responseObject);
        }
    }
}

