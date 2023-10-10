﻿// <auto-generated />
using System;
using Examen_UII.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Examen_UII.Migrations
{
    [DbContext(typeof(AguaDBContext))]
    [Migration("20231009193748_CrearBaseDatos")]
    partial class CrearBaseDatos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Examen_UII.Models.Dispositivos", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DireccionMAC")
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)");

                    b.Property<string>("Estatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdUsuarios")
                        .HasColumnType("int");

                    b.Property<string>("Identificador")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Latitud")
                        .HasColumnType("float");

                    b.Property<double>("Longitud")
                        .HasColumnType("float");

                    b.Property<int>("Prioridad")
                        .HasColumnType("int");

                    b.Property<int?>("UsuariosID")
                        .HasColumnType("int");

                    b.Property<int>("ZonasID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UsuariosID");

                    b.HasIndex("ZonasID");

                    b.ToTable("Dispositivos");
                });

            modelBuilder.Entity("Examen_UII.Models.RegistrosConsumo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<double>("CantidadLitrosRegistrados")
                        .HasColumnType("float");

                    b.Property<int>("DispositivosId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Inicio")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DispositivosId");

                    b.ToTable("RegistrosConsumo");
                });

            modelBuilder.Entity("Examen_UII.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Rol")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Examen_UII.Models.Usuarios", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<string>("Usuario")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("RolesId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Examen_UII.Models.Zonas", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Zonas");
                });

            modelBuilder.Entity("Examen_UII.Models.Dispositivos", b =>
                {
                    b.HasOne("Examen_UII.Models.Usuarios", "Usuarios")
                        .WithMany()
                        .HasForeignKey("UsuariosID");

                    b.HasOne("Examen_UII.Models.Zonas", "Zonas")
                        .WithMany("Dispositivos")
                        .HasForeignKey("ZonasID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuarios");

                    b.Navigation("Zonas");
                });

            modelBuilder.Entity("Examen_UII.Models.RegistrosConsumo", b =>
                {
                    b.HasOne("Examen_UII.Models.Dispositivos", "Dispositivos")
                        .WithMany("RegistrosConsumo")
                        .HasForeignKey("DispositivosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dispositivos");
                });

            modelBuilder.Entity("Examen_UII.Models.Usuarios", b =>
                {
                    b.HasOne("Examen_UII.Models.Roles", "Roles")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Examen_UII.Models.Dispositivos", b =>
                {
                    b.Navigation("RegistrosConsumo");
                });

            modelBuilder.Entity("Examen_UII.Models.Roles", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Examen_UII.Models.Zonas", b =>
                {
                    b.Navigation("Dispositivos");
                });
#pragma warning restore 612, 618
        }
    }
}