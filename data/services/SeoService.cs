using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace depot {

    public interface ISeoService {
        RobotsTxt GetRobots ();
        bool UpdateRobots (RobotsTxt model);
    }

    public class SeoService : ISeoService {

        public readonly IHostingEnvironment _hostingEnvironment;

        public SeoService (IHostingEnvironment hostingEnvironment) {
            this._hostingEnvironment = hostingEnvironment;
        }

        public string PathToFile => _hostingEnvironment.WebRootPath + "/../files/robots.txt";

        public RobotsTxt GetRobots () {
            var model = new RobotsTxt {
                Text = File.ReadAllText (PathToFile)
            };

            return model;
        }

        public bool UpdateRobots (RobotsTxt model) {
            File.WriteAllText (PathToFile, model.Text);

            return true;
        }
    }

}