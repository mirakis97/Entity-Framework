﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using P01_HospitalDatabase.Data;

namespace P01_HospitalDatabase.Migrations
{
    [DbContext(typeof(HospitalContext))]
    [Migration("20211120164657_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.Diagnose", b =>
                {
                    b.Property<int>("DiagnoseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("DiagnoseId");

                    b.HasIndex("PatientId");

                    b.ToTable("Diagnoses");
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.Medicament", b =>
                {
                    b.Property<int>("MedicamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("MedicamentId");

                    b.ToTable("Medicaments");
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("HasInsurance")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("PatientId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.PatientMedicament", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "MedicamentId");

                    b.HasIndex("MedicamentId");

                    b.ToTable("PatientMedicaments");
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.Visitation", b =>
                {
                    b.Property<int>("VisitationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("VisitationId");

                    b.HasIndex("PatientId");

                    b.ToTable("Visitations");
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.Diagnose", b =>
                {
                    b.HasOne("P01_HospitalDatabase.Data.Models.Patient", "Patient")
                        .WithMany("Diagnoses")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.PatientMedicament", b =>
                {
                    b.HasOne("P01_HospitalDatabase.Data.Models.Medicament", "Medicament")
                        .WithMany("PatientMedicaments")
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("P01_HospitalDatabase.Data.Models.Patient", "Patient")
                        .WithMany("PatientMedicaments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("P01_HospitalDatabase.Data.Models.Visitation", b =>
                {
                    b.HasOne("P01_HospitalDatabase.Data.Models.Patient", "Patient")
                        .WithMany("Visitations")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
