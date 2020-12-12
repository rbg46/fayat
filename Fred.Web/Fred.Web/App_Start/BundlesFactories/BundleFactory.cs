using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace Fred.Web
{
    public abstract class BundleFactory
    {
        protected List<string> jsFiles = new List<string>();
        protected List<string> cssFiles = new List<string>();
        private readonly BundleCollection bundles;
        private string jsBundleName;
        private string cssBundleName;

        protected BundleFactory(BundleCollection bundles)
        {
            if (bundles == null)
            {
                throw new System.ArgumentNullException(nameof(bundles));
            }

            this.bundles = bundles;
        }

        protected void JsBundleName(string jsBundleName)
        {
            this.jsBundleName = jsBundleName;
        }

        protected void Js(params string[] jsFiles)
        {
            this.jsFiles = jsFiles.ToList();
        }

        protected void AddJs(string jsFile)
        {
            jsFiles.Add(jsFile);
        }

        protected void CssBundleName(string cssBundleName)
        {
            this.cssBundleName = cssBundleName;
        }

        protected void Css(params string[] cssFiles)
        {
            this.cssFiles = cssFiles.ToList();
        }

        protected void AddCss(string cssFile)
        {
            cssFiles.Add(cssFile);
        }

        /// <summary>
        /// Crée le bundle.
        /// </summary>
        /// <param name="minifyJs">True pour minifier le Javascript, sinon false.</param>
        public void CreateBundle(bool minifyJs = true)
        {
            if (!string.IsNullOrEmpty(jsBundleName) && jsFiles.Count > 0)
            {
                var jsBundle = new ScriptBundle(jsBundleName).Include(jsFiles.ToArray());
                bundles.Add(jsBundle);
                if (!minifyJs)
                {
                    // Using the ASP.Net bundler without minification : https://www.gembalabs.com/2015/04/20/using-the-asp-net-bundler-without-minification/
                    jsBundle.Transforms.Clear();
                }
            }
            if (!string.IsNullOrEmpty(cssBundleName) && cssFiles.Count > 0)
            {
                bundles.Add(new StyleBundle(cssBundleName).Include(cssFiles.ToArray()));
            }
        }
    }
}
