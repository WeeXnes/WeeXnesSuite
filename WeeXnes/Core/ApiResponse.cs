using System.Collections;
using System.Collections.Generic;

namespace WeeXnes.Core
{
    public class GithubApiResponse
    {
        public string tag_name { get; set; }
        public IList<GithubAsset> assets { get; set; }

        public GithubApiResponse(string tag_name, IList<GithubAsset> assets)
        {
            this.tag_name = tag_name;
            this.assets = assets;
        }
        public override string ToString()
        {
            return this.tag_name + " with " + this.assets.Count;
        }
    }

    public class GithubAsset
    {
        public string browser_download_url { get; set; }
        public string name { get; set; }

        public GithubAsset(string browser_download_url, string name)
        {
            this.browser_download_url = browser_download_url;
            this.name = name;
        }
        public override string ToString()
        {
            return this.name;
        }
    }
}