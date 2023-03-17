using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTracker.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiEndpointAttribute : Attribute
    {
        public string EndpointName { get; set; }

        public ApiEndpointAttribute(string endpointName)
        {
            EndpointName = endpointName;
        }
    }
}
