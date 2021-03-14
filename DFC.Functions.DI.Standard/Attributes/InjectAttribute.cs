using System;
using Microsoft.Azure.WebJobs.Description;

namespace DFC.Functions.DI.Standard.Attributes
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}