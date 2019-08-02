﻿// <auto-generated />
using System;
using AMI.Persistence.EntityFramework.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AMI.Persistence.EntityFramework.SQLite.Migrations
{
    [DbContext(typeof(SqliteDbContext))]
    partial class SqliteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("AMI.Domain.Entities.ObjectEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("DataType");

                    b.Property<string>("ExtractedPath");

                    b.Property<int>("FileFormat");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("OriginalFilename")
                        .IsRequired();

                    b.Property<string>("SourcePath")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedDate");

                    b.HasIndex("UserId");

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("AMI.Domain.Entities.ResultEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("BasePath");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("JsonFilename");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("ResultSerialized");

                    b.Property<int>("ResultType");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("AMI.Domain.Entities.RoleEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AMI.Domain.Entities.TaskEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("CommandSerialized");

                    b.Property<int>("CommandType");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Message");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<Guid?>("ObjectId");

                    b.Property<int>("Position");

                    b.Property<int>("Progress");

                    b.Property<Guid?>("ResultId");

                    b.Property<int>("Status");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedDate");

                    b.HasIndex("ResultId")
                        .IsUnique();

                    b.HasIndex("Status");

                    b.HasIndex("UserId");

                    b.HasIndex("ObjectId", "Status");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("AMI.Domain.Entities.TokenEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUsedDate");

                    b.Property<string>("TokenValue")
                        .IsRequired();

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedDate");

                    b.HasIndex("LastUsedDate");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("AMI.Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTime?>("LockoutEndDateUtc");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedUsername")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Roles");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatedDate");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .IsUnique();

                    b.HasIndex("NormalizedUsername")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AMI.Domain.Entities.TaskEntity", b =>
                {
                    b.HasOne("AMI.Domain.Entities.ObjectEntity", "Object")
                        .WithMany("Tasks")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AMI.Domain.Entities.ResultEntity", "Result")
                        .WithOne("Task")
                        .HasForeignKey("AMI.Domain.Entities.TaskEntity", "ResultId");
                });

            modelBuilder.Entity("AMI.Domain.Entities.TokenEntity", b =>
                {
                    b.HasOne("AMI.Domain.Entities.UserEntity", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
