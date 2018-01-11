using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogSurveyDEV.Startup))]
namespace BlogSurveyDEV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
