using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.DatabaseContext
{
    public class ApiContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Report>()
            .HasOne(e => e.User)
            .WithMany(c => c.Reports)
            .HasForeignKey(f => f.UserId);

            builder.Entity<PaymentGateway>()
            .HasOne(e => e.ModifiedByUser)
            .WithMany(c => c.ModifiedPaymentGateways)
            .HasForeignKey(f => f.ModifiedByUserId);

            builder.Entity<Movie>()
            .HasOne(e => e.AssetsCheckedByUser)
            .WithMany(c => c.AssetsCheckedMovies)
            .HasForeignKey(f => f.AssetsCheckedByUserId);

            builder.Entity<Scene>()
            .HasOne(e => e.CheckedByUser)
            .WithMany(c => c.CheckedScenes)
            .HasForeignKey(f => f.CheckedByUserId);

            builder.Entity<Scene>()
            .HasOne(e => e.CheckedByScene)
            .WithMany(c => c.CheckedScenes)
            .HasForeignKey(f => f.CheckedBySceneId);

            builder
            .Entity<UserDevice>()
            .Property(e => e.Id)
            .ValueGeneratedNever();

            builder.Entity<BannedKeyword>()
            .HasIndex(u => u.Keyword)
            .IsUnique();

            builder.Entity<Payment>()
            .HasOne(e => e.Discount)
            .WithMany(c => c.Payments)
            .HasForeignKey(f => f.DiscountCode);

            builder.Entity<AppRelease>().Ignore(m => m.Version);

            builder.Entity<Payment>()
            .HasOne(e => e.Gateway)
            .WithMany(c => c.Payments)
            .HasForeignKey(f => f.GatewayType);

            builder.Entity<FeatureItem>()
            .HasOne(e => e.CheckedByUser)
            .WithMany(c => c.ChangedFeatureItems)
            .HasForeignKey(f => f.ChangedByUserId);

            builder.Entity<UserDiscount>()
            .HasOne(e => e.Discount)
            .WithMany(c => c.UserDiscounts)
            .HasForeignKey(f => f.DiscountCode);

            base.OnModelCreating(builder);
        }

        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieLanguage> MovieLanguages { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<BannedKeyword> BannedKeywords { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<AppRelease> AppReleases { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureItem> FeatureItems { get; set; }
    }

    public class ApiContextFactory : IDesignTimeDbContextFactory<ApiContext>
    {
        public ApiContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            optionsBuilder.UseNpgsql(
                "CONNECTION_STRING",
                options => options.SetPostgresVersion(new Version(9, 6)));

            return new ApiContext(optionsBuilder.Options);
        }
    }
}

