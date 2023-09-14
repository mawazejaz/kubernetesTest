using Microsoft.AspNetCore.Mvc;

namespace kubernetesTest.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AzureTestController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};
		private readonly string UploadsDirectory = "uploads"; // Directory to store uploaded files

		private readonly ILogger<AzureTestController> _logger;

		public AzureTestController(ILogger<AzureTestController> logger)
		{
			_logger = logger;
			// Ensure the uploads directory exists
			Directory.CreateDirectory(UploadsDirectory);
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest("No file uploaded.");
			}

			// Generate a unique filename for the uploaded file
			string uniqueFileName = "Test_File.pdf";

			// Combine the uploads directory path and the unique filename to create the full file path
			string filePath = Path.Combine(UploadsDirectory, uniqueFileName);

			if (System.IO.File.Exists(filePath))
			{
				// If a file with the same name already exists, delete it
				System.IO.File.Delete(filePath);
			}

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			// You can now process the uploaded file or return a success message
			return Ok($"File uploaded successfully. File path: {filePath}");
		}
	}
}