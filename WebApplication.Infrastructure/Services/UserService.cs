﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Infrastructure.Contexts;
using WebApplication.Infrastructure.Entities;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly InMemoryContext _dbContext;

        public UserService(InMemoryContext dbContext)
        {
            _dbContext = dbContext;

            // this is a hack to seed data into the in memory database. Do not use this in production.
            _dbContext.Database.EnsureCreated();
        }

        /// <inheritdoc />
        public async Task<User?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            User? user = await _dbContext.Users.Where(user => user.Id == id)
                                         .Include(x => x.ContactDetail)
                                         .FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> FindAsync(string? givenNames, string? lastName, CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users.Where(user => user.GivenNames == givenNames || user.LastName == lastName)
                                        .Include(x => x.ContactDetail)
                                        .ToListAsync(cancellationToken);

            return users;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetPaginatedAsync(int page, int count, CancellationToken cancellationToken = default)
        {
            var users = await _dbContext.Users.Skip((page - 1) * count)
                                            .Include(x => x.ContactDetail)
                                            .Take(count)
                                            .ToListAsync(cancellationToken);

            return users;
        }

        /// <inheritdoc />
        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        /// <inheritdoc />
        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var userToUpdate = await _dbContext.Users.Where(x => x.Id == user.Id)
                                         .Include(x => x.ContactDetail)
                                         .FirstAsync(cancellationToken);

            userToUpdate.GivenNames = user.GivenNames;
            userToUpdate.LastName = user.LastName;
            if (user.ContactDetail != null)
            {
                userToUpdate.ContactDetail ??= new ContactDetail();
                userToUpdate.ContactDetail.EmailAddress = user.ContactDetail.EmailAddress;
                userToUpdate.ContactDetail.MobileNumber = user.ContactDetail.MobileNumber;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return userToUpdate;
        }

        /// <inheritdoc />
        public async Task<User?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var userToDelete = await _dbContext.Users.Where(x => x.Id == id)
                                             .Include(x => x.ContactDetail)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (userToDelete != null)
            {
                _dbContext.Users.Remove(userToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return userToDelete;
        }

        /// <inheritdoc />
        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Implement a way to count the number of users in the database.");
        }
    }
}
