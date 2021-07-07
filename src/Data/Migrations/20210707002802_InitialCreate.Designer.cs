﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WhMgr.Data.Contexts;

namespace WhMgr.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210707002802_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.GymSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<ushort>("MaximumLevel")
                        .HasColumnType("smallint unsigned")
                        .HasColumnName("max_level");

                    b.Property<ushort>("MinimumLevel")
                        .HasColumnType("smallint unsigned")
                        .HasColumnName("min_level");

                    b.Property<string>("Name")
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("PokemonIDs")
                        .HasColumnType("longtext")
                        .HasColumnName("pokemon_ids");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("gyms");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.InvasionSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Areas")
                        .HasColumnType("longtext")
                        .HasColumnName("city");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<int>("InvasionType")
                        .HasColumnType("int")
                        .HasColumnName("grunt_type");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("PokestopName")
                        .HasColumnType("longtext")
                        .HasColumnName("pokestop_name");

                    b.Property<string>("RewardPokemonId")
                        .HasColumnType("longtext")
                        .HasColumnName("reward_pokemon_id");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("invasions");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.LocationSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("DistanceM")
                        .HasColumnType("int")
                        .HasColumnName("distance");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<double>("Latitude")
                        .HasColumnType("double")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasColumnType("double")
                        .HasColumnName("longitude");

                    b.Property<string>("Name")
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("locations");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.LureSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Areas")
                        .HasColumnType("longtext")
                        .HasColumnName("city");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("LureType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("lure_type");

                    b.Property<string>("PokestopName")
                        .HasColumnType("longtext")
                        .HasColumnName("pokestop_name");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("lures");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.Metadata", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .HasColumnType("longtext")
                        .HasColumnName("value");

                    b.HasKey("Key");

                    b.ToTable("metadata");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.PokemonSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Areas")
                        .HasColumnType("longtext")
                        .HasColumnName("city");

                    b.Property<string>("FormsString")
                        .HasColumnType("longtext")
                        .HasColumnName("form");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext")
                        .HasColumnName("gender");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("IVList")
                        .HasColumnType("longtext")
                        .HasColumnName("iv_list");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<int>("MaximumLevel")
                        .HasColumnType("int")
                        .HasColumnName("max_lvl");

                    b.Property<int>("MinimumCP")
                        .HasColumnType("int")
                        .HasColumnName("min_cp");

                    b.Property<int>("MinimumIV")
                        .HasColumnType("int")
                        .HasColumnName("min_iv");

                    b.Property<int>("MinimumLevel")
                        .HasColumnType("int")
                        .HasColumnName("min_lvl");

                    b.Property<string>("PokemonId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("pokemon_id");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.Property<uint>("_Size")
                        .HasColumnType("int unsigned")
                        .HasColumnName("size");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("pokemon");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.PvpSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Areas")
                        .HasColumnType("longtext")
                        .HasColumnName("city");

                    b.Property<string>("Form")
                        .HasColumnType("longtext")
                        .HasColumnName("form");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("League")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("league");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<double>("MinimumPercent")
                        .HasColumnType("double")
                        .HasColumnName("min_percent");

                    b.Property<int>("MinimumRank")
                        .HasColumnType("int")
                        .HasColumnName("min_rank");

                    b.Property<string>("PokemonId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("pokemon_id");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("pvp");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.QuestSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Areas")
                        .HasColumnType("longtext")
                        .HasColumnName("city");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("PokestopName")
                        .HasColumnType("longtext")
                        .HasColumnName("pokestop_name");

                    b.Property<string>("RewardKeyword")
                        .HasColumnType("longtext")
                        .HasColumnName("reward");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("quests");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.RaidSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Areas")
                        .HasColumnType("longtext")
                        .HasColumnName("city");

                    b.Property<string>("Form")
                        .HasColumnType("longtext")
                        .HasColumnName("form");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<uint>("PokemonId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("pokemon_id");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int")
                        .HasColumnName("subscription_id");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("raids");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("guild_id");

                    b.Property<string>("IconStyle")
                        .HasColumnType("longtext")
                        .HasColumnName("icon_style");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext")
                        .HasColumnName("phone_number");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint unsigned")
                        .HasColumnName("status");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("subscriptions");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.GymSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Gyms")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.InvasionSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Invasions")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.LocationSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Locations")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.LureSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Lures")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.PokemonSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Pokemon")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.PvpSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("PvP")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.QuestSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Quests")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.RaidSubscription", b =>
                {
                    b.HasOne("WhMgr.Services.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany("Raids")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WhMgr.Services.Subscriptions.Models.Subscription", b =>
                {
                    b.Navigation("Gyms");

                    b.Navigation("Invasions");

                    b.Navigation("Locations");

                    b.Navigation("Lures");

                    b.Navigation("Pokemon");

                    b.Navigation("PvP");

                    b.Navigation("Quests");

                    b.Navigation("Raids");
                });
#pragma warning restore 612, 618
        }
    }
}
