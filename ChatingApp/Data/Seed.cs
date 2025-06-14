﻿using ChatingApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ChatingApp.Data
{
    public class Seed
    {
        public static async Task SeedUsers(Context context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
           
            if(users == null) return;

            foreach (var user in users) {
               using var hmac = new HMACSHA512();
               user.UserName=user.UserName.ToLower();
               user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word"));
               user.PasswordSalt = hmac.Key;
               context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}
