﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SpelAvondApp.Infrastructure.Migrations
{
    [DbContext(typeof(SpellenDbContext))]
    [Migration("20241014105521_SpellenMigration")]
    partial class SpellenMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BordspelBordspellenAvond", b =>
                {
                    b.Property<int>("BordspellenAvondenId")
                        .HasColumnType("int");

                    b.Property<int>("BordspellenId")
                        .HasColumnType("int");

                    b.HasKey("BordspellenAvondenId", "BordspellenId");

                    b.HasIndex("BordspellenId");

                    b.ToTable("BordspellenAvondBordspellen", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser");
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.Bordspel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Beschrijving")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<bool>("Is18Plus")
                        .HasColumnType("bit");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SoortSpel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Bordspellen");
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.BordspellenAvond", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("BiedtAlcoholvrijeOpties")
                        .HasColumnType("bit");

                    b.Property<bool>("BiedtLactosevrijeOpties")
                        .HasColumnType("bit");

                    b.Property<bool>("BiedtNotenvrijeOpties")
                        .HasColumnType("bit");

                    b.Property<bool>("BiedtVegetarischeOpties")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Is18Plus")
                        .HasColumnType("bit");

                    b.Property<int>("MaxAantalSpelers")
                        .HasColumnType("int");

                    b.Property<string>("OrganisatorId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BordspellenAvonden");
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.Inschrijving", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Aanwezig")
                        .HasColumnType("bit");

                    b.Property<int>("BordspellenAvondId")
                        .HasColumnType("int");

                    b.Property<string>("DieetWensen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpelerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BordspellenAvondId");

                    b.ToTable("Inschrijvingen");
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BordspellenAvondId")
                        .HasColumnType("int");

                    b.Property<string>("Opmerking")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("SpelerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BordspellenAvondId");

                    b.HasIndex("SpelerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("BordspelBordspellenAvond", b =>
                {
                    b.HasOne("SpelAvondApp.Domain.Models.BordspellenAvond", null)
                        .WithMany()
                        .HasForeignKey("BordspellenAvondenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpelAvondApp.Domain.Models.Bordspel", null)
                        .WithMany()
                        .HasForeignKey("BordspellenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.Inschrijving", b =>
                {
                    b.HasOne("SpelAvondApp.Domain.Models.BordspellenAvond", "BordspellenAvond")
                        .WithMany("Inschrijvingen")
                        .HasForeignKey("BordspellenAvondId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BordspellenAvond");
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.Review", b =>
                {
                    b.HasOne("SpelAvondApp.Domain.Models.BordspellenAvond", "BordspellenAvond")
                        .WithMany("Reviews")
                        .HasForeignKey("BordspellenAvondId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Speler")
                        .WithMany()
                        .HasForeignKey("SpelerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BordspellenAvond");

                    b.Navigation("Speler");
                });

            modelBuilder.Entity("SpelAvondApp.Domain.Models.BordspellenAvond", b =>
                {
                    b.Navigation("Inschrijvingen");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
