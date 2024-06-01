using Microsoft.AspNetCore.Mvc;
using PdfSharp;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;


namespace Report.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("FeesStructure")]
        public async Task<ActionResult> GeneratePdf(PdfRequest req)
        {
            var data = new PdfDocument();
            string htmlContent = "<div style = 'margin: 20px auto; heigth:1000px; max-width: 600px; padding: 20px; border: 1px solid #ccc; background-color: #FFFFFF; font-family: Arial, sans-serif;' >";
            htmlContent += "<div style = 'margin-bottom: 20px; text-align: center;'>";
            htmlContent += "<img src = 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcROnYPD5QO8ZJvPQt8ClnJNPXduCeX89dSOxA&usqp=CAU' alt = 'School Logo' style = 'max-width: 100px; margin-bottom: 10px;' >";
            htmlContent += "</div>";
            htmlContent += "<p style = 'margin: 0;' >Jobin School Management</p>";
            htmlContent += "<p style = 'margin: 0;' > 123 School Street, Sample Street</p>";
            htmlContent += "<p style = 'margin: 0;' > Phone: 123 - 456 - 7890 </p>";
            htmlContent += "<p style = 'margin: 0;' > Tamilnadu </p>";
            htmlContent += "<div style = 'text-align: center; margin-bottom: 20px;'>";
            htmlContent += "<h1> Fees Structure </h1>";
            htmlContent += "</div>";
            htmlContent += "<h3> StudentDetails:</h3>";
            htmlContent += "<p> Name:" + req.Name + "</p>";
            htmlContent += "<p> STD:" + req.Std + "</p>";
            htmlContent += "<table style = 'width: 100%; border-collapse: collapse;'>";
            htmlContent += "<thead>";
            htmlContent += "<tr>";
            htmlContent += "<th style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' > Fee Description </th>";
            htmlContent += "<th style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' > Amount(INR) </th>";
            htmlContent += "</tr><hr/>";
            htmlContent += "</thead>";
            htmlContent += "<tbody>";
            decimal totalAmount = 0;
            if (req.fees != null && req.fees.Count > 0)
            {
                req.fees.ForEach(x =>
                {
                    htmlContent += "<tr>";
                    htmlContent += "<td style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' >" + x.FeesDescription + " </td>";
                    htmlContent += "<td style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' >Rs " + x.Amount + "/- </td>";
                    htmlContent += "</tr>";
                    if (decimal.TryParse(x.Amount, out decimal feeAmount))
                    {
                        totalAmount += feeAmount;
                    }
                });
                htmlContent += "</tbody>";
                htmlContent += "<tfoot>";
                htmlContent += "<tr>";
                htmlContent += "<td style = 'padding: 8px; text-align: right; font-weight: bold;'> Total:</td>";
                htmlContent += "<td style = 'padding: 8px; text-align: left; border-top: 1px solid #ddd;' >Rs" + totalAmount + "/- </td>";
                htmlContent += "</tr>";
                htmlContent += "</tfoot>";
            }
            htmlContent += "</table>";
            htmlContent += "</div>";        
            PdfGenerator.AddPdfPages(data, htmlContent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                data.Save(ms);
                response = ms.ToArray();
            }
            string fileName = "FeesStructure" + req.date + ".pdf";
            return File(response, "application/pdf", fileName);
        }
    }
}
