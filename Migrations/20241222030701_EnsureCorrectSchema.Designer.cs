﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WikiSistemaASP.NET.Data;

#nullable disable

namespace WikiSistemaASP.NET.Migrations
{
    [DbContext(typeof(WikiDbContext))]
    [Migration("20241222030701_EnsureCorrectSchema")]
    partial class EnsureCorrectSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WikiSistemaASP.NET.Models.Modulo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("Modulos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descricao = "Módulo básico de introdução ao ASP.NET.",
                            Nome = "Introdução ao ASP.NET"
                        },
                        new
                        {
                            Id = 2,
                            Descricao = "Módulo sobre EF Core.",
                            Nome = "Entity Framework Core"
                        });
                });

            modelBuilder.Entity("WikiSistemaASP.NET.Models.Topico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<int>("ModuloId")
                        .HasColumnType("integer");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ModuloId");

                    b.ToTable("Topicos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Conteudo = "Introdução ao ASP.NET.",
                            ModuloId = 1,
                            Titulo = "O que é ASP.NET?"
                        },
                        new
                        {
                            Id = 2,
                            Conteudo = "Passos para instalação.",
                            ModuloId = 2,
                            Titulo = "Instalando o Entity Framework Core"
                        });
                });

            modelBuilder.Entity("WikiSistemaASP.NET.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsAdmin = true,
                            Nome = "Administrador do Sistema",
                            Senha = "admin123",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("WikiSistemaASP.NET.Models.Topico", b =>
                {
                    b.HasOne("WikiSistemaASP.NET.Models.Modulo", "Modulo")
                        .WithMany("Topicos")
                        .HasForeignKey("ModuloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Modulo");
                });

            modelBuilder.Entity("WikiSistemaASP.NET.Models.Modulo", b =>
                {
                    b.Navigation("Topicos");
                });
#pragma warning restore 612, 618
        }
    }
}
