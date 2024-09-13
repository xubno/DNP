﻿using Entities;

namespace RepositoryContracts;

public interface IUserRespository
{
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<User> GetSingleAsync(int id);
    IQueryable<User> GetMany();
}