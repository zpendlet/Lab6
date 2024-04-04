using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab6.Models;
using Microsoft.Extensions.Logging;

namespace Lab6
{
    // Interface
    public interface IBookService
    {
        Task<List<Book>> GetBooks();
        void AddBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int bookId);
        Task BorrowBook(int bookId, string userEmail); 
    }

    // Service Implementation
    public class BookService : IBookService
    {
        private readonly List<Book> books = new List<Book>(); // Ensure books list is scoped properly
        private readonly Dictionary<User, List<Book>> borrowedBooks = new Dictionary<User, List<Book>>(); // Ensure borrowedBooks dictionary is scoped properly
        private readonly List<User> users = new List<User>(); // Ensure users list is scoped properly
        

        public async Task<List<Book>> GetBooks()
        {
            return await Task.FromResult(books);
        }

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public void EditBook(Book book)
        {
            var existingBook = books.Find(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
            }
            else
            {
                throw new ArgumentException("Book not found");
            }
        }

        public void DeleteBook(int bookId)
        {
            var bookToRemove = books.Find(b => b.Id == bookId);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
            }
            else
            {
                throw new ArgumentException("Book not found");
            }
        }

        public async Task BorrowBook(int bookId, string userEmail)
        {
            // Find the book to borrow
            Book book = books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found or no available copies.");
            }

            // Find the user who is borrowing the book by email
            User user = users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            // Borrow the book
            if (!borrowedBooks.ContainsKey(user))
            {
                borrowedBooks[user] = new List<Book>();
            }

            borrowedBooks[user].Add(book);
            books.Remove(book);
        }

    }
}

