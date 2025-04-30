using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that the endpoint does not require authorization to be accessed, even though the entire controller requires authorization..
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
internal class AllowAnonymousAttribute : Attribute { }
