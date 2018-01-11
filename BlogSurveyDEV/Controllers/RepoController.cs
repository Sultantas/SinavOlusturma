using BlogSurveyDEV.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BlogSurveyDEV.Controllers
{
    public class RepoController : Controller
    {
        // public ApplicationDbContext db { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Repo
        [Authorize]
        [HttpPost]
        public ActionResult AddBlog(BlogViewModel model)
        {

            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

            

            if (model.blog == null)
            {

                return Json(new
                {
                    result = false,
                    message = "Blog null  "
                }, JsonRequestBehavior.AllowGet);
            }


        


            model.blog.ApplicationUserId = user.Id;
            db.Blogs.Add(model.blog);
            //db.Entry(blog).State = System.Data.Entity.EntityState.Deleted;
            try
            {

                db.SaveChanges();

                model.Question1.BlogId = model.blog.Id;
                db.Questions.Add(model.Question1);

                model.Question2.BlogId = model.blog.Id;
                db.Questions.Add(model.Question2);

                model.Question3.BlogId = model.blog.Id;
                db.Questions.Add(model.Question3);

                model.Question4.BlogId = model.blog.Id;
                db.Questions.Add(model.Question4);

                db.SaveChanges();


                return Json(new
                {
                    result = true,
                    message = "İşlem başarılı.",
                    data = model
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = false,
                    message = "Beklenmedik bir hata oluştu. Hata Mesejı: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteBlog(int _blogId)
        {
            Blog blog = db.Blogs.FirstOrDefault(b => b.Id == _blogId);

            if (blog == null)
            {

                return Json(new {
                    result = false,
                    message = "Kayıt bulunamadı."
                }, JsonRequestBehavior.AllowGet);                                                                                                       
            }

            db.Blogs.Remove(blog);
            //db.Entry(blog).State = System.Data.Entity.EntityState.Deleted;
            try
            {

                db.SaveChanges();

                return Json(new
                {
                    result = true,
                    message = "İşlem başarılı."
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = false,
                    message = "Beklenmedik bir hata oluştu. Hata Mesejı: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }


           

        }


        // Questions
        public bool AddQuestion(Question que)
        {
            

           
            db.Questions.Add(que);
            //db.Entry(blog).State = System.Data.Entity.EntityState.Deleted;
            try
            {

                db.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }


        // Questions
        public ActionResult GetBlogViewModel()
        {
            return Json(new { model = new BlogViewModel() }, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public ActionResult CheckOption(BlogExamViewModel model)
        {
            // Birinci Soru Değerlendirmesi
            Question que = db.Questions.FirstOrDefault(b => b.Id == model.Question1.QueId);
            model.Question1.Result = (model.Question1.UserAnswer == que.Answer).ToString().ToLower();

            // İkinci Soru Değerlendirmesi
            que = db.Questions.FirstOrDefault(b => b.Id == model.Question2.QueId);
            model.Question2.Result = (model.Question2.UserAnswer == que.Answer).ToString().ToLower();

            // Üçüncü Soru Değerlendirmesi
            que = db.Questions.FirstOrDefault(b => b.Id == model.Question3.QueId);
            model.Question3.Result = (model.Question3.UserAnswer == que.Answer).ToString().ToLower();

            // Dördüncü Soru Değerlendirmesi
            que = db.Questions.FirstOrDefault(b => b.Id == model.Question4.QueId);
            model.Question4.Result = (model.Question4.UserAnswer == que.Answer).ToString().ToLower();
            

            return Json(new
            {
                result = true,
                data = model
            }, JsonRequestBehavior.AllowGet);




        }

        
        public ActionResult GetBlogExamModel(int id)
        {
            Question[] questions = db.Questions.Where(q => q.BlogId == id).ToArray();

            BlogExamViewModel model = new BlogExamViewModel()
            {
                Question1 = new OptionControlModel()
                {
                    QueId = questions[0].Id
                },
                Question2 = new OptionControlModel()
                {
                    QueId = questions[1].Id
                },
                Question3 = new OptionControlModel()
                {
                    QueId = questions[2].Id
                },
                Question4 = new OptionControlModel()
                {
                    QueId = questions[3].Id
                }

                
            };

            return Json(new
            {
                result = true,
                data = model
            }, JsonRequestBehavior.AllowGet);




        }


    }


    public class BlogViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public Blog blog { get; set; }

        public Question Question1 { get; set; }
        public Question Question2 { get; set; }
        public Question Question3 { get; set; }
        public Question Question4 { get; set; }
     
         


        public BlogViewModel()
        {
            blog = new Blog();
            Question1 = new Question();
            Question2 = new Question();
            Question3 = new Question();
            Question4 = new Question();
        }

    }

    public class BlogExamViewModel
    {
        public OptionControlModel Question1 { get; set; }
        public OptionControlModel Question2 { get; set; }
        public OptionControlModel Question3 { get; set; }
        public OptionControlModel Question4 { get; set; }
    }

    public class OptionControlModel
    {
        public int QueId { get; set; }
        public int UserAnswer { get; set; }
        public string Result { get; set; }
    }

}