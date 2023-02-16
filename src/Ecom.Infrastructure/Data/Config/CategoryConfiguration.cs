using Ecom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(30);

            //Seed
            builder.HasData(
                new Category { Id = 1, Name = "Category one", Description = "1" },
                new Category { Id = 2, Name = "Category two", Description = "2" },
                new Category { Id = 3, Name = "Category three", Description = "3" }
                );
        }
    }
}
