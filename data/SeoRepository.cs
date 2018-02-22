namespace depot {
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System;
    using HtmlAgilityPack;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;

    public interface ISeoRepository {
        Task<SeoParameter> Get (int id);
        // GlobalSeo GetSearchSeo ();
        void UpdateSearchSeo (GlobalSeo seo);
        Task SaveCatalogItemSeo (SeoParameter model, int catalogItemId);
    }

    public class SeoRepository : ISeoRepository {

        private DepotContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SeoRepository (DepotContext dbContext, IHostingEnvironment hostingEnvironment) {
            this._dbContext = dbContext;
            this._hostingEnvironment = hostingEnvironment;
        }

        public async Task<SeoParameter> Get (int id) {
            var seo = await this._dbContext.SeoParameters.FindAsync (id);
            return seo;
        }

        public async Task SaveCatalogItemSeo (SeoParameter model, int catalogItemId) {
            var item = this._dbContext.ProducerCatalogItems.Include (i => i.SeoParameter).First (x => x.ID == catalogItemId);

            if (model.ID == 0 && item.SeoParameter == null) {
                await this._dbContext.SeoParameters.AddAsync (model);
                await this._dbContext.SaveChangesAsync ();
            }

            item.SeoParameterId = model.ID;
            item.SeoParameter = model;

            this._dbContext.Update (item);

            await this._dbContext.SaveChangesAsync ();
        }

        // public string CUSTOM_APP_TAG_SEO = "custom-app-tag-seo";

        /*
                public GlobalSeo GetSearchSeo () {
                    var elements = this.GetElement ();
                    var htmlDocument = elements.Item1;
                    var element = elements.Item2;

                    var result = element.InnerText;

                    var seoJson = this.GetSeoString (result);

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<GlobalSeo> (seoJson, this.JsonSettings);;
                }
        */
        public void UpdateSearchSeo (GlobalSeo seo) {
            /*var elements = this.GetElement ();
            var htmlDocument = elements.Item1;
            var element = elements.Item2;
*/
            var seoResult = Newtonsoft.Json.JsonConvert.SerializeObject (seo, this.JsonSettings);

            var pathToFile = _hostingEnvironment.WebRootPath + "/../files/ceo/search.json";
            File.WriteAllText (pathToFile, seoResult);
        }

        private JsonSerializerSettings JsonSettings => new JsonSerializerSettings () {
            ContractResolver = new CamelCasePropertyNamesContractResolver ()
        };
        /*
        private string GetModifiedParams (string input, string newValue) {
            var indexes = this.GetIndexes (input);

            var aStringBuilder = new StringBuilder (input);
            aStringBuilder.Remove (indexes.Item1, indexes.Item2 - indexes.Item1 + 1);
            aStringBuilder.Insert (indexes.Item1, newValue);

            return aStringBuilder.ToString ();
        }
 */
        private string PathToFile => !_hostingEnvironment.IsDevelopment () ?
            _hostingEnvironment.WebRootPath + "./../wwwroot/search_module/index.html" :
            _hostingEnvironment.WebRootPath + "./../src/search/index.html";

        /*
                private Tuple<HtmlDocument, HtmlNode> GetElement () {
                    var htmlDocument = new HtmlDocument ();
                    var text = File.ReadAllText (PathToFile);
                    htmlDocument.LoadHtml (text);
                    var element = htmlDocument.GetElementbyId (CUSTOM_APP_TAG_SEO);

                    return Tuple.Create (htmlDocument, element);
                }
        private string GetSeoString (string result) {
            var indexes = this.GetIndexes (result);
            var seoJson = result.Substring (indexes.Item1, indexes.Item2 - indexes.Item1 + 1).Trim ();

            return seoJson; //Regex.Replace (seoJson, @"\s+", "");
        }

        private Tuple<int, int> GetIndexes (string input) {
            var jsonTagFirst = input.IndexOf ('{');
            var jsonTagLast = input.LastIndexOf ('}');

            return Tuple.Create (jsonTagFirst, jsonTagLast);
        }
         */
    }

}