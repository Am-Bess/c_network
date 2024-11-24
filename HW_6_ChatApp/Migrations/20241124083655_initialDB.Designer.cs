﻿// <auto-generated />
using System;
using HW_6_ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HW_6_ChatApp.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20241124083655_initialDB")]
    partial class initialDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HW_6_ChatApp.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateMessage")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FromUserId")
                        .HasColumnType("integer")
                        .HasColumnName("from_user_id");

                    b.Property<bool>("IsReceived")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<int?>("ToUserId")
                        .HasColumnType("integer")
                        .HasColumnName("to_user_id");

                    b.HasKey("Id")
                        .HasName("massages_pkey");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("HW_6_ChatApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("HW_6_ChatApp.Models.Message", b =>
                {
                    b.HasOne("HW_6_ChatApp.Models.User", "FromUser")
                        .WithMany("FromMessages")
                        .HasForeignKey("FromUserId")
                        .HasConstraintName("messages_from_user_id_fkey");

                    b.HasOne("HW_6_ChatApp.Models.User", "ToUser")
                        .WithMany("ToMessage")
                        .HasForeignKey("ToUserId")
                        .HasConstraintName("messages_to_user_id_fkey");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("HW_6_ChatApp.Models.User", b =>
                {
                    b.Navigation("FromMessages");

                    b.Navigation("ToMessage");
                });
#pragma warning restore 612, 618
        }
    }
}