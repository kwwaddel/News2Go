using NewsAggregator.Data;
using NewsAggregator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsAggregator.Controllers.api
{
    public class ArticlesController : ApiController
    {
        private Repo _repo;

        public ArticlesController()
        {
            _repo = new Repo();
        }

        //[HttpPost]
        //[Route("api/Articles/GetArticles")]
        //public IHttpActionResult GetArticles(String userName)
        //{
        //    return Ok(_repo.GetArticles(userName));
        //}

        [HttpPost]
        [Route("api/Articles/AddArticle")]
        public IHttpActionResult AddArticle(string articleId, String catName, [FromUri] string[] tags)
        {
            HashSet<string> thetags = new HashSet<string>(tags);
            HashSet<Tag> theTags = new HashSet<Tag>();
            foreach (string tag in thetags)
                theTags.Add(new Tag { Name = tag});

            //_repo.AddArticle(articleId, new Category { Name = catName, Tags = new HashSet<Tag>(theTags) });
            _repo.AddArticle(articleId, new Category { Name = catName}, theTags);

            return Ok();
        }

        [HttpPost]
        [Route("api/Articles/AddComment")]
        public IHttpActionResult AddComment(string articleId, string comm, string userName)
        {
            _repo.AddComment(new Comment { Comm = comm, SharedArticleId = articleId}, userName);

            return Ok();
        }

        [HttpGet]
        [Route("api/Articles/GetComments")]
        public IHttpActionResult GetComments(string articleId)
        {
            return Ok(_repo.GetComments(articleId));
        }

        [HttpPost]
        [Route("api/Articles/GetCategoryPreference")]
        public IHttpActionResult GetCategoryPreference(string userName)
        {
            List<CategoryPreference> p = _repo.GetCategoryPreference(userName);

            string[] categoryNames = new string[p.Count];

            for (int i = 0; i < p.Count; i++)
            {
                if (p.ElementAt(i).Display)
                    categoryNames[i] = p.ElementAt(i).CategoryName;
            }   
            
            return Ok(categoryNames);
        }

        [HttpPost]
        [Route("api/Articles/AddCategoryPreference")]
        public IHttpActionResult AddCategoryPreference(string userName, [FromUri]String[] categories)
        {
            Debug.WriteLine("AddCatPref");
            List<Category> c = new List<Category>();

            foreach (String s in categories)
                c.Add(new Category { Name = s });

            _repo.AddPreference(userName, c);

            return Ok();
        }

        [HttpPost]
        [Route("api/Articles/AddFriends")]
        public IHttpActionResult AddFriends(string userName, [FromUri]string[] friendNames)
        {
            _repo.AddFriends(userName, friendNames);

            return Ok();
        }

        [HttpPost]
        [Route("api/Articles/AddFriendRequest")]
        public IHttpActionResult AddFriendRequest(string userName, string friendName)
        {
            _repo.AddFriendRequest(userName, friendName);

            return Ok();
        }

        [HttpGet]
        [Route("api/Articles/GetUserCommentedArticles")]
        public IHttpActionResult GetUserCommentedArticles(string userName)
        {
            return Ok(_repo.GetUserCommentedArticles(userName));
        }

        [HttpGet]
        [Route("api/Articles/GetFriends")]
        public IHttpActionResult GetFriends(string userName)
        {
            List<Friend> fs = new List<Friend>(_repo.GetFriends(userName));
            
            if (fs.Count == 0)
                return Ok();

            String[] friends = new String[fs.Count];

            for (int i = 0; i < fs.Count; i++)
                friends[i] = fs.ElementAt(i).Username;

            List<FriendRequest> fr = new List<FriendRequest>();

            foreach (Friend f in fs)
            {
                fr.Add(new FriendRequest { TargetName = f.Username });
            }

            return Ok(fr);
        }

        [HttpGet]
        [Route("api/Articles/HasFriendRequests")]
        public IHttpActionResult HasFriendRequests(string userName)
        {
            return Ok(_repo.HasFriendRequests(userName));
        }

        [HttpPost]
        [Route("api/Articles/GetNewPushedArticles")]
        public IHttpActionResult GetNewPushedArticles(string userName)
        {
            return Ok(_repo.GetNewPushedArticles(userName));
        }

        [HttpPost]
        [Route("api/Articles/GetPushedArticles")]
        public IHttpActionResult GetPushedArticles(string userName)
        {
            return Ok(_repo.GetPushedArticles(userName));
        }

        [HttpPost]
        [Route("api/Articles/PushArticle")]
        public IHttpActionResult PushArticle(string userName, [FromUri]string [] friendNames, string articleId)
        {
            _repo.PushArticle(userName, friendNames, articleId);

            return Ok();
        }

        [HttpPost]
        [Route("api/Articles/Like")]
        public IHttpActionResult Like(string userName, string articleId, string category, [FromUri] string[] tags, int like)
        {
            HashSet<string> thetags = new HashSet<string>(tags);
            HashSet<Tag> theTags = new HashSet<Tag>();
            foreach (string tag in thetags)
                theTags.Add(new Tag { Name = tag });

            _repo.Like(userName, articleId, category, theTags, like);

            return Ok();
        }

        [HttpPost]
        [Route("api/Articles/GetPopularity")]
        public IHttpActionResult GetPopularity(string articleId)
        {
            int[] ratings = new int[3] { 0, 0, 0 };

            if (_repo.GetPopularity(articleId) == null)
                return Ok(ratings);
            
            return Ok(_repo.GetPopularity(articleId).ToArray());
        }

        [HttpPost]
        [Route("api/Articles/GetPopularities")]
        public IHttpActionResult GetPopularities(string [] articleIds)
        {
            return Ok(_repo.GetPopularities(articleIds));
        }

        [HttpPost]
        [Route("api/Articles/GetLikedTags")]
        public IHttpActionResult GetLikedTags(string userName)
        {
            List<Tag> t = _repo.GetLikedTags(userName);
            String[] tags = new String[t.Count];
            for (int i = 0; i < t.Count; i++)
                tags[i] = t.ElementAt(i).Name;

            return Ok(tags);
        }

        [HttpPost]
        [Route("api/Articles/GetMostPopularArticlesByCategory")]
        public IHttpActionResult GetMostPopularArticlesByCategory(string category)
        {
            List<Article> a = _repo.GetMostPopularArticlesByCategory(category);
            String[] articles = new String[a.Count];
            for (int i = 0; i < a.Count; i++)
                articles[i] = a.ElementAt(i).Id;

            return Ok(articles);
        }

        [HttpGet]
        [Route("api/Articles/MadeRating")]
        public IHttpActionResult MadeRating(String userName, String articleId)
        {
            return Ok(_repo.MadeRating(userName, articleId));
        }
    }
}
