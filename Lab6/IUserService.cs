using System;
using System.Collections.Generic;
using System.Linq;
using Lab6.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lab6
{
    // Interface
    public interface IUserService
    {
        Task<List<User>> GetUsers(List<User> user);
        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int userId);
    }

    // Service Implementation
    public class UserService : IUserService
    {
        private List<User> users = new List<User>();

        public async Task<List<User>> GetUsers(List<User> user)
        {
            // Ensure userList is not null
            user.Clear(); // Clear existing items
            user.AddRange(users); // Add users from the service
            return await Task.FromResult(user);
        }

        public void AddUser(User user)
        {
            int id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            user.Id = id;
            users.Add(user);
        }

        public void EditUser(User user)
        {
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public void DeleteUser(int userId)
        {
            var userToRemove = users.FirstOrDefault(u => u.Id == userId);
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }
    }
}

