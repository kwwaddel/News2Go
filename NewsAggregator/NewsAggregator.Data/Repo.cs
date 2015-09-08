using NewsAggregator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data
{
    public class Repo
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public void AddArticle(String articleId, Category c, HashSet<Tag> tags)
        {
            
            Article a = _db.Articles.Find(articleId);

            if (a != null)
                a.Popularity++;
            else
            {
                if (_db.Categories.Find(c.Name) == null)
                    _db.Categories.Add(c);

                foreach (Tag t in tags)
                {
                    t.CategoryId = c.Name;

                    if (_db.Tags.Find(t.Name) == null)
                        _db.Tags.Add(t);
                }   

                _db.Articles.Add(new Article { Id = articleId, CategoryId = c.Name, Popularity = 0});
            }

            try
            {
                _db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {
                Exception raise = dbEx;
                throw dbEx.InnerException;
            }
        }

        public void AddComment(Comment c, string userName)
        {
            c.UserId = GetUserIdFromName(userName);

            if (_db.SharedArticles.Find(c.SharedArticleId) == null)
            {
                _db.SharedArticles.Add(new SharedArticle { Id = c.SharedArticleId });
                _db.SharedArticleUsers.Add(new SharedArticleUser { SharedArticleId = c.SharedArticleId, UserId = c.UserId, MadeRating = 0});
            }   
            
            _db.Comments.Add(c);
            _db.SaveChanges();
        }

        public void Like(String userName, String articleId, String category, HashSet<Tag> tags, int like)
        {
            String userId = GetUserIdFromName(userName);

            if (_db.Categories.Find(category) == null)
                _db.Categories.Add(new Category { Name = category });

            if (_db.Preferences.Find(userId) == null)
                _db.Preferences.Add(new Preference { UserId = userId });
            //foreach (TagPreference tp in _db.TagPrefs)
            //{
            //    if (tp.PreferenceId == userId)
            //        tp.Rating += like;
            //}
            foreach (Tag t in tags)
            {
                t.CategoryId = category;

                if (_db.Tags.Find(t.Name) == null)
                    _db.Tags.Add(t);

                //List<TagPreference> tps = new List<TagPreference>(_db.TagPrefs.Where(x => x.PreferenceId == userId));

                TagPreference tp = _db.TagPrefs.FirstOrDefault(x => x.TagId == t.Name && x.PreferenceId == userId);

                if (tp == null)
                    _db.TagPrefs.Add(new TagPreference { TagId = t.Name, PreferenceId = userId, Rating = 1 });
                else
                    tp.Rating += like;

            }

            Article a = _db.Articles.Find(articleId);
            Debug.WriteLine("Category: " + category);
            if (a == null)
            {
                if (like == 1)
                    _db.Articles.Add(new Article { Id = articleId, CategoryId = category, Popularity = 1, Like = 1, Dislike = 0});
                else
                    _db.Articles.Add(new Article { Id = articleId, CategoryId = category, Popularity = 1, Like = 0, Dislike = 1 });  
            }
            else
            {
                a.Popularity += like;

                if (like == 1)
                    a.Like++;
                else
                    a.Dislike++;
            }

            if (_db.SharedArticleUsers.FirstOrDefault(x => x.UserId == userId && x.SharedArticleId == articleId) == null)
            {
                if (_db.SharedArticles.Find(articleId) == null)
                    _db.SharedArticles.Add(new SharedArticle { Id = articleId });

                _db.SharedArticleUsers.Add(new SharedArticleUser { SharedArticleId = articleId, UserId = userId, MadeRating = 1 });
            }

            _db.SaveChanges();
        }

        public void AddPreference(String userId, List<Category> categories)
        {
            //if (_db.Users.Find(userId).Preference == null)
            //    _db.Users.Find(userId).Preference = new Preference();

            //foreach (Category c in categories)
            //{
            //    if (_db.Categories.Find(c.Id) == null)
            //        _db.Users.Find(userId).Preference.Categories.Add(c);
            //    else
            //        _db.Users.Find(userId).Preference.Categories.Add(_db.Categories.Find(c.Id));
            //}

            //_db.SaveChanges();
            //Debug.WriteLine("cat name: " + categories.ElementAt(0).Name);
            //if (_db.Users.Find(userId) != null)
            //{
            //    _db.Preferences.Add(new Preference { UserId = userId });
            //    foreach (Category c in categories)
            //    {
            //        Debug.WriteLine("here: " + categories.ElementAt(0).Name);
            //        _db.CatPrefs.Add(new CategoryPreference { CategoryName = c.Name, Display = true, PreferenceId = userId });
            //    }
            //}
            //_db.SaveChanges();
        }

        //public List<Article> GetArticles(String userId)
        //{
        //    return new List<Article>(_db.Articles.Where(x => x.UserId == userId));
        //}

        public String[][] GetComments(String articleId)
        {
            List<Comment> comments = new List<Comment>(_db.Comments.Where(x => x.SharedArticleId == articleId));

            String[][] comms = new String[comments.Count][];

            if (comments.Count != 0)
            {
                for (int i = 0; i < comments.Count; i++)
                {
                    Comment c = comments.ElementAt(i);
                    comms[i] = new string[] { GetNameFromUserId(c.UserId), c.Comm };
                }
            }

            return comms;
        }

        public List<String> GetUserCommentedArticles(String userName)
        {
            String userId = GetUserIdFromName(userName);
            List<Comment> cs = new List<Comment>(_db.Comments.Where(x => x.UserId == userId));
            List<String> articles = new List<String>();

            foreach (Comment c in cs)
                articles.Add(c.SharedArticleId);

            return articles;
        }

        public List<Article> GetPopularByCategory(Category c, int num)
        {
            return new List<Article>(_db.Articles.Where(x => x.Category.Name == c.Name).OrderByDescending(x => x.Popularity).Take(num));
        }

        public Preference GetPreference(String userName)
        {
            String userId = GetUserIdFromName(userName);
            Debug.WriteLine("userId: " + userId);
            return _db.Preferences.Find(userId);
        }

        public List<CategoryPreference> GetCategoryPreference(String userName)
        {
            String userId = GetUserIdFromName(userName);

            Debug.WriteLine("userId: " + userId);
            return new List<CategoryPreference>(_db.CatPrefs.Where(x => x.PreferenceId == userId));
        }

        public ApplicationUser GetUser(String userId)
        {
            return _db.Users.Find(userId);
        }

        //public void EditComment(Comment c)
        //{
        //    _db.Articles.Find(c.ArticleId).Comments.First(x => x.Id == c.Id).Comm = c.Comm;
        //    _db.SaveChanges();
        //}

        //public List<ApplicationUser> GetFriends(String userId)
        //{
        //    return _db.Users.Find(userId).Friends;
        //}

        public void AddFriends(String userName, String []friendNames)
        {
            String userId = GetUserIdFromName(userName);

            for (int i = 0; i < friendNames.Length; i++)
            {
                String friendName = friendNames[i];
                String friendId = GetUserIdFromName(friendName);

                if (_db.Friends.FirstOrDefault((x => x.UserId == userId && x.Username == friendName)) == null)
                {
                    _db.Friends.Add(new Friend { UserId = userId, Username = friendName });
                    _db.Friends.Add(new Friend { UserId = friendId, Username = userName });
                }
            }

            _db.SaveChanges();
        }

        public void AddFriendRequest(String userName, String friendName)
        {
            String userId = GetUserIdFromName(userName);

            String friendId = GetUserIdFromName(friendName);

            if (_db.FriendRequests.FirstOrDefault(x => x.RequesterId == userId && x.TargetId == friendId) == null && _db.Friends.FirstOrDefault(y => y.UserId == userId && y.Username == friendName) == null && _db.FriendRequests.FirstOrDefault(x => x.RequesterId == friendId && x.TargetId == userId) == null)
            {
                if (_db.Friends.FirstOrDefault((x => x.UserId == userId && x.Username == friendName)) == null)
                {
                    _db.FriendRequests.Add(new FriendRequest { RequesterName = userName, TargetName = friendName, RequesterId = userId, TargetId = friendId, IsActive = true });
                }
            }

            _db.SaveChanges();
        }

        public List<FriendRequest> HasFriendRequests(String userName)
        {
            String userId = GetUserIdFromName(userName);
            List<FriendRequest> fRequests = new List<FriendRequest>(_db.FriendRequests.Where(x => x.TargetId == userId && x.IsActive));

            if (fRequests.Count == 0)
                return null;

            //String[][] requests = new String[fRequests.Count][];

            for (int i = 0; i < fRequests.Count; i++)
            {
                //requests[i] = new String[] { GetNameFromUserId(fRequests.ElementAt(i).RequesterId), fRequests.ElementAt(i).RequesterId };
                fRequests.ElementAt(i).IsActive = false;
                //Debug.WriteLine("Made active false");
            }

            _db.SaveChanges();

            return fRequests;
        }

        public List<Friend> GetFriends(String userName)
        {
            String userId = GetUserIdFromName(userName);

            List<Friend> friends = null;

            if (_db.Friends.Where(x => x.UserId == userId) != null)
                friends = new List<Friend>(_db.Friends.Where(x => x.UserId == userId));

            return friends;
        }

        public void UpdatePreference(String userId, Category c, bool b)
        {
            //int num = 0;

            //if (b)
            //    num = 1;
            //else
            //    num = -1;

            //foreach (Tag t in c.Tags)
            //{
            //    if (_db.Users.Find(userId).Preference.TagRatings.ContainsKey(t))
            //        _db.Users.Find(userId).Preference.TagRatings[t] += num;
            //    else
            //        _db.Users.Find(userId).Preference.TagRatings.Add(t, num);
            //}

            //_db.SaveChanges();
        }

        public void UpdatePreferences(String userId, List<Category> add, List<Category> remove)
        {
            //_db.Users.Find(userId).Preference.Categories.AddRange(add);
            //_db.Users.Find(userId).Preference.Categories.RemoveAll(x => remove.Contains(x));
            //_db.SaveChanges();
        }

        public String GetUserIdFromName(String s)
        {
            String result = null;

            ApplicationUser user = _db.Users.FirstOrDefault(x => x.UserName == s);

            if (user != null)
                result = user.Id;

            return result;
        }

        public String GetNameFromUserId(String s)
        {
            String result = null;

            ApplicationUser user = _db.Users.Find(s);

            if (user != null)
                result = user.UserName;

            return result.Substring(0, result.IndexOf("@"));
        }

        public String[][] GetPushedArticles(String userName)
        {
            String userId = GetUserIdFromName(userName);
            String[][] articles = null;
            if (_db.PushedArticles.Where(x => x.ToId == userId) != null)
            {
                List<PushedArticle> a = new List<PushedArticle>(_db.PushedArticles.Where(x => x.ToId == userId));
                articles = new String[a.Count][];

                for (int i = 0; i < a.Count; i++)
                {
                    PushedArticle pa = a.ElementAt(i);
                    pa.IsNew = false;
                    articles[i] = new String[] { GetNameFromUserId(pa.FromId), pa.ArticleId };
                }
            }
            _db.SaveChanges();

            return articles;
        }

        public String[][] GetNewPushedArticles(string userName)
        {
            
            String userId = GetUserIdFromName(userName);
            String[][] articles = null;
            if (_db.PushedArticles.Where(x => x.ToId == userId) != null)
            {
                List<PushedArticle> a = new List<PushedArticle>(_db.PushedArticles.Where(x => x.ToId == userId && x.IsNew));
                articles = new String[a.Count][];

                for (int i = 0; i < a.Count; i++)
                {
                    PushedArticle pa = a.ElementAt(i);
                    if (pa.IsNew)
                    {
                        articles[i] = new String[] { GetNameFromUserId(pa.FromId), pa.ArticleId };
                        pa.IsNew = false;
                    }
                }
            }
            _db.SaveChanges();

            return articles;
        }

        //doesn't currently check for existing shared article
        public void PushArticle(String userName, String [] friendNames, String articleId)
        {
            String userId = GetUserIdFromName(userName);
            for (int i = 0; i < friendNames.Length; i++)
            {
                String friendId = GetUserIdFromName(friendNames[i]);
                _db.PushedArticles.Add(new PushedArticle { ArticleId = articleId, IsNew = true, FromId = userId, ToId = friendId });
            }

            _db.SaveChanges();
        }

        public List<Tag> GetLikedTags(String userName)
        {
            string userId = GetUserIdFromName(userName);
            List<TagPreference> tagPrefs = new List<TagPreference>(_db.TagPrefs.Where(x => x.Rating > 0 && x.PreferenceId == userId));
            List<Tag> tags = new List<Tag>();

            foreach(TagPreference tag in tagPrefs)
            {
                tags.Add(_db.Tags.Find(tag.TagId));
            }

            return tags;
        }

        public List<Article> GetMostPopularArticlesByCategory(String category)
        {
            List<Article> articles = null;

            articles = new List<Article>(_db.Articles.Where(x => x.CategoryId == category).OrderByDescending(y => y.Popularity).Take(10));

            return articles;
        }

        public List<int> GetPopularity(String articleId)
        {
            List<int> ratings = new List<int>();

            Article a = _db.Articles.Find(articleId);
            Debug.WriteLine("articleid getpop: " + articleId + (a == null));
            if (a == null)
                return null;

            ratings.Add(a.Like);
            ratings.Add(a.Dislike);
            ratings.Add(a.Popularity);

            return ratings;
        }

        public int[][] GetPopularities(String [] articleIds)
        {
            int[][] ratings = new int[articleIds.Length][];

            for(int i = 0; i < articleIds.Length; i++)
            {
                String a = articleIds[i];
                if (GetPopularity(a) == null)
                    ratings[i] = new int[] { 0, 0, 0 };
                else
                    ratings[i] = GetPopularity(a).ToArray();
            }

            return ratings;
        }

        public bool MadeRating(String userName, String articleId)
        {
            bool result = false;
            String userId = GetUserIdFromName(userName);

            if (_db.SharedArticleUsers.FirstOrDefault(x => x.UserId == userId && x.SharedArticleId == articleId && x.MadeRating == 1) != null)
                result = true;

            return result;
        }
    }
}
