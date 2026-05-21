using System;
using System.Linq;
using System.Threading.Tasks;
using Itransition_Course_Project.Data;
using Itransition_Course_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Itransition_Course_Project.Services
{
    public class CustomIdGeneratorService : ICustomIdGeneratorService
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random = new Random();

        public CustomIdGeneratorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync(int inventoryId, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return Guid.NewGuid().ToString().Substring(0, 8);
            }

            string generatedId = format;

            if (generatedId.Contains("{X5_}"))
                generatedId = generatedId.Replace("{X5_}", _random.Next(0, 0xFFFFF).ToString("X5"));

            if (generatedId.Contains("{X8_}"))
                generatedId = generatedId.Replace("{X8_}", _random.Next().ToString("X8"));

            generatedId = generatedId.Replace("{YYYY}", DateTime.UtcNow.Year.ToString());
            generatedId = generatedId.Replace("{MM}", DateTime.UtcNow.Month.ToString("D2"));

            if (generatedId.Contains("{SEQ}"))
            {
                var highestIdRecord = await _context.Items
                    .Where(item => item.InventoryId == inventoryId)
                    .OrderByDescending(item => item.Id)
                    .Select(item => (int?)item.Id)
                    .FirstOrDefaultAsync();

                int sequentialCounter = (highestIdRecord ?? 0) + 1;
                generatedId = generatedId.Replace("{SEQ}", sequentialCounter.ToString("D4"));
            }

            return generatedId;
        }
    }
}