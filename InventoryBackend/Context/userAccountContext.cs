using InventoryBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InventoryBackend.Context
{
    public class userAccountContext : DbContext
    {
        public userAccountContext(DbContextOptions<userAccountContext> options) : base(options)
        { }
        public DbSet<userAccounts> userAccounts => Set<userAccounts>();
        //public DbSet<userAccounts> --> Represents a table in the database where each row corresponds to a SupportTicket entity.
        
        //userAccount => Set<userAccounts>(); --> This is a property expression that returns a
        //DbSet<SupportTicket> using the Set<T>() method of DbContext.
        //It allows querying and modifying SupportTicket records.
    }
}
