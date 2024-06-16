using Event_Management.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Event_Management.API.Controllers
{
	[ApiController]
	[Route("api/v1")]
	public class HelperController : Controller
	{

		[HttpGet("enums")]
		public IActionResult GetAllEnums()
		{
			string namespaceName = "Event_Management.Domain.Enum";

			// Lấy tất cả các lớp trong namespace của project 2
			Type[] types = Assembly.Load("Event_Management.Domain").GetTypes()
				.Where(t => t.Namespace == namespaceName)
				.ToArray();

			// In tên các lớp
			foreach (Type type in types)
			{
				Console.WriteLine(type.Name);
			}


			string enumNamespace = "Event_Management.Domain.Enum";
			var assembly = Assembly.Load("Event_Management.Domain");
			var enumTypes = assembly.GetTypes()
									.Where(t => t.IsEnum && String.Equals(t.Namespace, enumNamespace, StringComparison.OrdinalIgnoreCase));

			var enumDictionary = new Dictionary<string, Dictionary<string, int>>();

			foreach (var enumType in enumTypes)
			{
				var values = Enum.GetValues(enumType).Cast<Enum>().ToDictionary(e => e.ToString(), e => Convert.ToInt32(e));
				enumDictionary.Add(enumType.Name, values);
			}

			return Ok(enumDictionary);
		}

		[HttpGet("constants")]
		public IActionResult GetAllConstants()
		{
			// Định nghĩa namespace chứa các constants
			string constantNamespace = "Event_Management.Domain.Constants";
			var assembly = Assembly.Load("Event_Management.Domain");
			var constantTypes = assembly.GetTypes()
										.Where(t => t.IsClass && t.Namespace == constantNamespace);

			var constantDictionary = new Dictionary<string, Dictionary<string, object>>();

			foreach (var constantType in constantTypes)
			{
				var fields = constantType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
										 .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
										 .ToDictionary(fi => fi.Name, fi => fi.GetRawConstantValue());

				if (fields.Any())
				{
					constantDictionary.Add(constantType.Name, fields!);
				}
			}

			return Ok(constantDictionary);
		}
        [HttpGet("reponse-message")]
		public IActionResult GetAllResponseMessage()
		{
            // Định nghĩa namespace chứa các constants
            string constantNamespace = "Event_Management.Application.Message";
            var assembly = Assembly.Load("Event_Management.Application");
            var constantTypes = assembly.GetTypes()
                                        .Where(t => t.IsClass && t.Namespace == constantNamespace);

            var constantDictionary = new Dictionary<string, Dictionary<string, object>>();

            foreach (var constantType in constantTypes)
            {
                var fields = constantType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                         .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                                         .ToDictionary(fi => fi.Name, fi => fi.GetRawConstantValue());

                if (fields.Any())
                {
                    constantDictionary.Add(constantType.Name, fields!);
                }
            }

            return Ok(constantDictionary);
        }
    }
}
