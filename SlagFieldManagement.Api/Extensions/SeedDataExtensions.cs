using Bogus;
using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Entities;
using SlagFieldManagement.Infrastructure;

namespace SlagFieldManagement.Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        SeedRoles(dbContext);
        SeedUsers(dbContext);
        SeedMaterials(dbContext);
        //SeedBuckets(dbContext);
        SeedMaterialSettings(dbContext);
    }
    
    private static void SeedRoles(ApplicationDbContext dbContext)
    {
        if (!dbContext.Roles.Any())
        {
            var roles = new List<Role>
            {
                Role.Create("Admin", "Administrator with full access").Value,
                Role.Create("Manager", "Material management specialist").Value,
                Role.Create("Operator", "System operator").Value
            };
            
            dbContext.Roles.AddRange(roles);
            dbContext.SaveChanges();
        }
    }

    private static void SeedUsers(ApplicationDbContext dbContext)
    {
        if (!dbContext.Users.Any())
        {
            var faker = new Faker();
            var roles = dbContext.Roles.ToList();
            
            var users = new List<User>();
            for (var i = 0; i < 50; i++)
            {
                var userResult = User.Create(
                    roles[faker.Random.Int(0, roles.Count-1)].Id,
                    faker.Internet.UserName(),
                    "dummy-hash", // В реальной системе следует генерировать настоящий хэш
                    faker.Internet.Email()
                );
                
                if (userResult.IsSuccess)
                    users.Add(userResult.Value);
            }
            
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }

    private static void SeedMaterials(ApplicationDbContext dbContext)
    {
        if (!dbContext.Materials.Any())
        {
            var materials = new List<Material>
            {
                Material.Create("Шлак стальной"),
                Material.Create("Шлак доменный"),
                Material.Create("Шлак мартеновский"),
            }.Select(m => m).ToList(); //need fix
            
            dbContext.Materials.AddRange(materials);
            dbContext.SaveChanges();
        }
    }

    private static void SeedBuckets(ApplicationDbContext dbContext)
    {
        if (!dbContext.Buckets.Any())
        {
            var faker = new Faker();
            var buckets = new List<Bucket>();
            
            for (var i = 0; i < 20; i++)
            {
                buckets.Add(Bucket.Create($"Bucket for {faker.Commerce.ProductMaterial()}"));
            }
            
            dbContext.Buckets.AddRange(buckets);
            dbContext.SaveChanges();
        }
    }

    private static void SeedMaterialSettings(ApplicationDbContext dbContext)
    {
        if (!dbContext.MaterialSettings.Any())
        {
            // Получаем материалы из базы
            var steelSlag = dbContext.Materials.FirstOrDefault(m => m.Name == "Шлак стальной");
            var blastSlag = dbContext.Materials.FirstOrDefault(m => m.Name == "Шлак доменный");

            var settings = new List<MaterialSettings>();

            // Обработка Шлака стального
            if (steelSlag != null)
            {
                var timeRanges = new List<string> { "0-12ч" };
                foreach (var range in timeRanges)
                {
                    if (TryParseTimeRange(range, out decimal min, out decimal max))
                    {
                        var duration = (int)(max - min);
                        var setting = MaterialSettings.Create(
                            materialId: steelSlag.Id,
                            stageName: "Цветовой этап 1",
                            eventType: null,
                            duration: duration,
                            visualStateCode: "Active",
                            minHours: min,
                            maxHours: max
                        );

                        if (setting.IsSuccess)
                            settings.Add(setting.Value);
                    }
                }
            }

            // Обработка Шлака доменного
            if (blastSlag != null)
            {
                var timeRanges = new List<string>
                {
                    "0-12ч", "12-24ч", "24-36ч",
                    "38-45ч", "49ч", "24-36ч", "36ч"
                };

                int stageCounter = 1;
                foreach (var range in timeRanges)
                {
                    if (TryParseTimeRange(range, out decimal min, out decimal max))
                    {
                        var duration = (int)(max - min);
                        var setting = MaterialSettings.Create(
                            materialId: blastSlag.Id,
                            stageName: $"Цветовой этап {stageCounter}",
                            eventType: null,
                            duration: duration,
                            visualStateCode: "Active",
                            minHours: min,
                            maxHours: max
                        );

                        if (setting.IsSuccess)
                            settings.Add(setting.Value);

                        stageCounter++;
                    }
                }
            }

            dbContext.MaterialSettings.AddRange(settings);
            dbContext.SaveChanges();
        }
    }
    
    // Метод для парсинга временных интервалов
    private static bool TryParseTimeRange(string input, out decimal min, out decimal max)
    {
        min = 0;
        max = 0;
        input = input.Replace("ч", "").Trim();

        if (input.Contains("-"))
        {
            var parts = input.Split('-');
            if (decimal.TryParse(parts[0], out min) && decimal.TryParse(parts[1], out max))
                return true;
        }
        else if (decimal.TryParse(input, out min))
        {
            max = min; // Для формата "49ч"
            return true;
        }

        return false;
    }
}