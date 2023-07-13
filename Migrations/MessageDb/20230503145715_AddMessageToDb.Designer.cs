﻿// <auto-generated />
using System;
using Job_Post_Website.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Job_Post_Website.Migrations.MessageDb
{
    [DbContext(typeof(MessageDbContext))]
    [Migration("20230503145715_AddMessageToDb")]
    partial class AddMessageToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Job_Post_Website.Model.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeSent")
                        .HasColumnType("datetime2");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageRecipient")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageSender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Message");
                });
#pragma warning restore 612, 618
        }
    }
}
