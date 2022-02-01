﻿// <auto-generated />
using System;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Accounts.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalUserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("AccountId");

                    b.ToTable("Account", (string)null);

                    b.HasData(
                        new
                        {
                            AccountId = new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"),
                            Currency = "USD",
                            ExternalUserId = "df1525ce-1f1b-4e22-81fd-1065a35e4d5c"
                        });
                });

            modelBuilder.Entity("Domain.Credits.Credit", b =>
                {
                    b.Property<Guid>("CreditId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("CreditId");

                    b.HasIndex("AccountId");

                    b.ToTable("Credit", (string)null);

                    b.HasData(
                        new
                        {
                            CreditId = new Guid("a86f8863-099f-49b2-acec-274476cb559d"),
                            AccountId = new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"),
                            Currency = "USD",
                            TransactionDate = new DateTime(2022, 2, 1, 14, 2, 18, 751, DateTimeKind.Utc).AddTicks(8011),
                            Value = 400m
                        });
                });

            modelBuilder.Entity("Domain.Debits.Debit", b =>
                {
                    b.Property<Guid>("DebitId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("DebitId");

                    b.HasIndex("AccountId");

                    b.ToTable("Debit", (string)null);

                    b.HasData(
                        new
                        {
                            DebitId = new Guid("3b31a10f-a9fe-49ad-94cb-ad32c07d13cb"),
                            AccountId = new Guid("352e98c4-f68b-4175-a943-08ab46b9c01b"),
                            Currency = "USD",
                            TransactionDate = new DateTime(2022, 2, 1, 14, 2, 18, 751, DateTimeKind.Utc).AddTicks(8029),
                            Value = 50m
                        });
                });

            modelBuilder.Entity("Domain.Credits.Credit", b =>
                {
                    b.HasOne("Domain.Accounts.Account", "Account")
                        .WithMany("CreditsCollection")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Domain.Debits.Debit", b =>
                {
                    b.HasOne("Domain.Accounts.Account", "Account")
                        .WithMany("DebitsCollection")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Domain.Accounts.Account", b =>
                {
                    b.Navigation("CreditsCollection");

                    b.Navigation("DebitsCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
