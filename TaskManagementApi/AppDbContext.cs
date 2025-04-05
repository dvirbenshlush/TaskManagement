﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagementApi.Models;

namespace TaskManagementApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}
