﻿using Microsoft.EntityFrameworkCore;
using Veterinary.API.Helpers;
using Veterinary.Shared.Entities;
using Veterinary.Shared.Enums;

namespace Veterinary.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;

        }


        public async Task SeedAsync()
        {
            // Asegúrate de que la base de datos exista y esté migrada
            await _context.Database.EnsureCreatedAsync();

            await CheckPetTypeAsync();

            await CheckRolesAsync();

            await CheckUserAsync("123", "OAP", "OAP", "calle xxrsdf", "orlapez@gmail.com", UserType.Admin);


        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string direccion, string email, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {

                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Address=direccion,
                    Email = email,

                    UserName = email,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

            }

            return user;
        }





        private async Task CheckPetTypeAsync()
        {
            // Verificar si hay datos, si no, insertarlos
            if (!_context.PetTypes.Any())
            {
                _context.PetTypes.Add(new PetType { Name = "Dog" });
                _context.PetTypes.Add(new PetType { Name = "Cat" });
                _context.PetTypes.Add(new PetType { Name = "Hamster" });
                _context.PetTypes.Add(new PetType { Name = "Snake" });

                await _context.SaveChangesAsync();
            }

        }
    }
}
