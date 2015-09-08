using Microsoft.AspNet.Identity.EntityFramework;
using NewsAggregator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Article> Articles { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Preference> Preferences { get; set; }
        public IDbSet<TagPreference> TagPrefs { get; set; }
        public IDbSet<CategoryPreference> CatPrefs { get; set; }
        public IDbSet<Friend> Friends { get; set; }
        public IDbSet<SharedArticleUser> SharedArticleUsers { get; set; }
        public IDbSet<SharedArticle> SharedArticles { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<PushedArticle> PushedArticles { get; set; }
        public IDbSet<FriendRequest> FriendRequests { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
