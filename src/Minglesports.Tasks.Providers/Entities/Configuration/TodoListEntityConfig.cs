using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minglesports.Tasks.Core.Domain;

namespace Minglesports.Tasks.Providers.Entities.Configuration
{
    public class TodoListEntityConfig : IEntityTypeConfiguration<TodoListAggregate>
    {
        public void Configure(EntityTypeBuilder<TodoListAggregate> builder)
        {
            builder.ToTable("TodoLists");

            builder.Property<long>("Id");
            builder.HasKey("Id");

            builder.OwnsOne(p => p.EntityId)
                .Property(p => p.Value)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("EntityId")
                .HasMaxLength(100)
                .IsRequired();

            builder.OwnsOne(p => p.EntityId)
                .Ignore(s => s.UserId);

            builder.OwnsOne(p => p.EntityId)
                .HasIndex(p => p.Value)
                .IsUnique();

            builder
                .OwnsMany(a => a.Tasks, r =>
                {
                    r.ToTable("Tasks");
                    r.WithOwner().HasForeignKey("TodoListId");
                    r.Property<long>("Id");
                    r.HasKey("Id");

                    r.OwnsOne(p => p.Name)
                        .Property(p => p.Value)
                        .HasColumnName("Name")
                        .HasMaxLength(100)
                        .IsRequired();

                    r.OwnsOne(p => p.EntityId)
                        .Property(p => p.Value)
                        .HasColumnName("EntityId")
                        .HasMaxLength(100)
                        .IsRequired();

                    r.Property(p => p.Status)
                        .HasConversion(
                            enumValue => enumValue.ToString(),
                            stringValue => Enum.Parse<TaskStatus>(stringValue))
                        .HasMaxLength(50)
                        .IsRequired();
                });
        }
    }
}