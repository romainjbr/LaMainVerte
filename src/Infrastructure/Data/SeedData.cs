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
                Location = "Living Room",
                WaterFrequency = WaterFrequency.Weekly,
                LastWatered = DateTime.Today.AddDays(-3),
                ImageUrl = "https://lamainvertestorage.blob.core.windows.net/lamainverte-plantimageblob/plants/d4fe00d7-6a64-4cf2-9776-58e79a059cc2.jpeg"
            },
            new()
            {
                Name = "Snake Plant",
                Species = "Sansevieria",
                Location = "Bedroom",
                WaterFrequency = WaterFrequency.EveryTwoWeeks,
                LastWatered = DateTime.Today.AddDays(-10),
                ImageUrl = "https://lamainvertestorage.blob.core.windows.net/lamainverte-plantimageblob/plants/2a711231-bfec-4cf3-b751-0a15a19f71dc.png"
            },
            new()
            {
                Name = "Barrel Cactus",
                Species = "Mammillaria",
                Location = "Bedroom",
                WaterFrequency = WaterFrequency.Monthly,
                LastWatered = DateTime.Today.AddDays(-10),
                ImageUrl = "https://lamainvertestorage.blob.core.windows.net/lamainverte-plantimageblob/plants/c01d0840-e99b-4e5c-8856-eca938afb51a.jpeg"
            },
            new()
            {
                Name = "Zanzibar gem",
                Species = "Zamioculcas",
                Location = "Balcony",
                WaterFrequency = WaterFrequency.TwiceAWeek,
                LastWatered = DateTime.Today.AddDays(-10),
                ImageUrl = "https://lamainvertestorage.blob.core.windows.net/lamainverte-plantimageblob/plants/66a21452-758f-4dfe-932f-ae25837abe21.jpg"
            }
        };

        context.Plants.AddRange(plants);
        context.SaveChanges();
    }
}
