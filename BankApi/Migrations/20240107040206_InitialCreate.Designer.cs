﻿// <auto-generated />
using System;
using BankApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankApi.Migrations
{
    [DbContext(typeof(BankDb))]
    [Migration("20240107040206_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("BankApi.Accounts.Account", b =>
                {
                    b.Property<Guid>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Balance")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("AccountID");

                    b.HasIndex("UserID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankApi.Commands.Command", b =>
                {
                    b.Property<Guid>("CommandID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("CommandStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommandType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FailureMessage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CommandID");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("BankApi.Transactions.Transaction", b =>
                {
                    b.Property<Guid>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountID")
                        .HasColumnType("TEXT");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.HasKey("TransactionID");

                    b.HasIndex("AccountID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BankApi.Users.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("CurrentAccountID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SurName")
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.HasIndex("CurrentAccountID")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankApi.Accounts.Account", b =>
                {
                    b.HasOne("BankApi.Users.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankApi.Transactions.Transaction", b =>
                {
                    b.HasOne("BankApi.Accounts.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BankApi.Users.User", b =>
                {
                    b.HasOne("BankApi.Accounts.Account", "CurrentAccount")
                        .WithOne()
                        .HasForeignKey("BankApi.Users.User", "CurrentAccountID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CurrentAccount");
                });

            modelBuilder.Entity("BankApi.Accounts.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("BankApi.Users.User", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
