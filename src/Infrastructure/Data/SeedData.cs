using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(PlantDbContext context)
    {
        context.Database.Migrate();

       if (context.Plants.Any())
        {    
            return;
        }

        var plants = new List<Plant>
        {
            new()
            {
                Name = "Monstera Deliciosa",
                Species = "Monstera",
                Location = "Living Room, near window",
                WaterFrequency = WaterFrequency.Weekly,
                LastWatered = DateTime.Today.AddDays(-3)
            },
            new()
            {
                Name = "Snake Plant",
                Species = "Sansevieria",
                Location = "Bedroom",
                WaterFrequency = WaterFrequency.EveryTwoWeeks,
                LastWatered = DateTime.Today.AddDays(-10)
            }
        };

        context.Plants.AddRange(plants);
        context.SaveChanges();
    }
}
