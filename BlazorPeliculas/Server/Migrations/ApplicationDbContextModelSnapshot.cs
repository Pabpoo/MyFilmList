﻿// <auto-generated />
using System;
using BlazorPeliculas.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlazorPeliculas.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationUserPelicula", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.Property<int>("FavoritasId")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationUserId", "FavoritasId");

                    b.HasIndex("FavoritasId");

                    b.ToTable("ApplicationUserPelicula");
                });

            modelBuilder.Entity("ApplicationUserPelicula1", b =>
                {
                    b.Property<string>("ApplicationUser1Id")
                        .HasColumnType("text");

                    b.Property<int>("PorVerId")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationUser1Id", "PorVerId");

                    b.HasIndex("PorVerId");

                    b.ToTable("ApplicationUserPelicula1");
                });

            modelBuilder.Entity("ApplicationUserPelicula2", b =>
                {
                    b.Property<string>("ApplicationUser2Id")
                        .HasColumnType("text");

                    b.Property<int>("VistasId")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationUser2Id", "VistasId");

                    b.HasIndex("VistasId");

                    b.ToTable("ApplicationUserPelicula2");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Biografia")
                        .HasColumnType("text");

                    b.Property<DateTime?>("FechaNacimiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Foto")
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Actores");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.Genero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.GeneroPelicula", b =>
                {
                    b.Property<int>("PeliculaId")
                        .HasColumnType("integer");

                    b.Property<int>("GeneroId")
                        .HasColumnType("integer");

                    b.HasKey("PeliculaId", "GeneroId");

                    b.HasIndex("GeneroId");

                    b.ToTable("GenerosPeliculas");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.Pelicula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("EnCartelera")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaLanzamiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Poster")
                        .HasColumnType("text");

                    b.Property<string>("Resumen")
                        .HasColumnType("text");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Trailer")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Peliculas");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.PeliculaActor", b =>
                {
                    b.Property<int>("PeliculaId")
                        .HasColumnType("integer");

                    b.Property<int>("ActorId")
                        .HasColumnType("integer");

                    b.Property<int>("Orden")
                        .HasColumnType("integer");

                    b.Property<string>("Personaje")
                        .HasColumnType("text");

                    b.HasKey("PeliculaId", "ActorId");

                    b.HasIndex("ActorId");

                    b.ToTable("PeliculasActores");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.VotoPelicula", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaVoto")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PeliculaId")
                        .HasColumnType("integer");

                    b.Property<string>("UsuarioId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Voto")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PeliculaId");

                    b.ToTable("VotosPeliculas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ApplicationUserPelicula", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorPeliculas.Shared.Entity.Pelicula", null)
                        .WithMany()
                        .HasForeignKey("FavoritasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationUserPelicula1", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("ApplicationUser1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorPeliculas.Shared.Entity.Pelicula", null)
                        .WithMany()
                        .HasForeignKey("PorVerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationUserPelicula2", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("ApplicationUser2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorPeliculas.Shared.Entity.Pelicula", null)
                        .WithMany()
                        .HasForeignKey("VistasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.GeneroPelicula", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.Genero", "Genero")
                        .WithMany("GenerosPelicula")
                        .HasForeignKey("GeneroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorPeliculas.Shared.Entity.Pelicula", "Pelicula")
                        .WithMany("GenerosPelicula")
                        .HasForeignKey("PeliculaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genero");

                    b.Navigation("Pelicula");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.PeliculaActor", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.Actor", "Actor")
                        .WithMany("PeliculasActor")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorPeliculas.Shared.Entity.Pelicula", "Pelicula")
                        .WithMany("PeliculasActor")
                        .HasForeignKey("PeliculaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Pelicula");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.VotoPelicula", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.Pelicula", "Pelicula")
                        .WithMany("VotosPeliculas")
                        .HasForeignKey("PeliculaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pelicula");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BlazorPeliculas.Shared.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.Actor", b =>
                {
                    b.Navigation("PeliculasActor");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.Genero", b =>
                {
                    b.Navigation("GenerosPelicula");
                });

            modelBuilder.Entity("BlazorPeliculas.Shared.Entity.Pelicula", b =>
                {
                    b.Navigation("GenerosPelicula");

                    b.Navigation("PeliculasActor");

                    b.Navigation("VotosPeliculas");
                });
#pragma warning restore 612, 618
        }
    }
}
