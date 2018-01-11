using BlogSurveyDEV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace BlogSurveyDEV.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            List<Blog> model = db.Blogs.ToList();

            return View("Index", model);
        }

        public ActionResult Detail(int id)
        {
            Blog blog = db.Blogs.FirstOrDefault(b => b.Id == id);

            Question[] ques = db.Questions.Where(q => q.BlogId == id).ToArray();

            BlogViewModel model = new BlogViewModel()
            {
                blog = blog,
                Question1 = ques[0],
                Question2 = ques[1],
                Question3 = ques[2],
                Question4 = ques[3],
            };

            return View("Detail", model);
        }

        public ActionResult AddBlog()
        {
            
            


           // return View(model1);



            return View();
        }


        public ActionResult GetTitles()
        {
            XmlModel model1 = new XmlModel();
            XmlTextReader okuyucu = new XmlTextReader("https://www.wired.com/feed/rss");
            XmlDocument dokuman = new XmlDocument();
            dokuman.Load(okuyucu);
            XmlNode rss = dokuman.SelectSingleNode("/rss");
            XmlNodeList title = dokuman.SelectNodes("/rss/channel/item/title");

            XmlNodeList link = dokuman.SelectNodes("/rss/channel/item/link");

            List<SourceListItem> sources = new List<SourceListItem>();

            for (int i = 0; i < 5; i++)
            {
                sources.Add(new SourceListItem()
                {
                    Title = title.Item(i).InnerText,
                    Url = link.Item(i).InnerText
                });
            }


            return Json(new { sources = sources }, JsonRequestBehavior.AllowGet);

        }
        
    }

    public class SourceListItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class XmlModel
    {
        public List<XmlModel> tlist { get; set; }
        public int Id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
    }
}