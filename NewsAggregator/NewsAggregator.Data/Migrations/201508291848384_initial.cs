namespace NewsAggregator.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Popularity = c.Int(nullable: false),
                        Like = c.Int(nullable: false),
                        Dislike = c.Int(nullable: false),
                        CategoryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        PreferenceId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Preferences", t => t.PreferenceId)
                .Index(t => t.PreferenceId);
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CategoryPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Display = c.Boolean(nullable: false),
                        PreferenceId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Preferences", t => t.PreferenceId)
                .Index(t => t.PreferenceId);
            
            CreateTable(
                "dbo.TagPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagId = c.String(maxLength: 128),
                        Rating = c.Int(nullable: false),
                        PreferenceId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Preferences", t => t.PreferenceId)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .Index(t => t.TagId)
                .Index(t => t.PreferenceId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comm = c.String(),
                        SharedArticleId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        Article_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SharedArticles", t => t.SharedArticleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Articles", t => t.Article_Id)
                .Index(t => t.SharedArticleId)
                .Index(t => t.UserId)
                .Index(t => t.Article_Id);
            
            CreateTable(
                "dbo.SharedArticles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.FriendRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequesterId = c.String(),
                        TargetId = c.String(),
                        TargetName = c.String(),
                        RequesterName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PushedArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.String(),
                        FromId = c.String(maxLength: 128),
                        ToId = c.String(maxLength: 128),
                        IsNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FromId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToId)
                .Index(t => t.FromId)
                .Index(t => t.ToId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SharedArticleUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MadeRating = c.Int(nullable: false),
                        SharedArticleId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SharedArticles", t => t.SharedArticleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SharedArticleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SharedArticleApplicationUsers",
                c => new
                    {
                        SharedArticle_Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SharedArticle_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.SharedArticles", t => t.SharedArticle_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.SharedArticle_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SharedArticleUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SharedArticleUsers", "SharedArticleId", "dbo.SharedArticles");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PushedArticles", "ToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PushedArticles", "FromId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "PreferenceId", "dbo.Preferences");
            DropForeignKey("dbo.Preferences", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "SharedArticleId", "dbo.SharedArticles");
            DropForeignKey("dbo.SharedArticleApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SharedArticleApplicationUsers", "SharedArticle_Id", "dbo.SharedArticles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagPreferences", "TagId", "dbo.Tags");
            DropForeignKey("dbo.Tags", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.TagPreferences", "PreferenceId", "dbo.Preferences");
            DropForeignKey("dbo.CategoryPreferences", "PreferenceId", "dbo.Preferences");
            DropIndex("dbo.SharedArticleApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SharedArticleApplicationUsers", new[] { "SharedArticle_Id" });
            DropIndex("dbo.SharedArticleUsers", new[] { "UserId" });
            DropIndex("dbo.SharedArticleUsers", new[] { "SharedArticleId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PushedArticles", new[] { "ToId" });
            DropIndex("dbo.PushedArticles", new[] { "FromId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Friends", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "Article_Id" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "SharedArticleId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Tags", new[] { "CategoryId" });
            DropIndex("dbo.TagPreferences", new[] { "PreferenceId" });
            DropIndex("dbo.TagPreferences", new[] { "TagId" });
            DropIndex("dbo.CategoryPreferences", new[] { "PreferenceId" });
            DropIndex("dbo.Preferences", new[] { "UserId" });
            DropIndex("dbo.Categories", new[] { "PreferenceId" });
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropTable("dbo.SharedArticleApplicationUsers");
            DropTable("dbo.SharedArticleUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PushedArticles");
            DropTable("dbo.FriendRequests");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Friends");
            DropTable("dbo.SharedArticles");
            DropTable("dbo.Comments");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tags");
            DropTable("dbo.TagPreferences");
            DropTable("dbo.CategoryPreferences");
            DropTable("dbo.Preferences");
            DropTable("dbo.Categories");
            DropTable("dbo.Articles");
        }
    }
}
