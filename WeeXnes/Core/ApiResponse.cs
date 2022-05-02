namespace WeeXnes.Core
{
    public class ApiResponse
    {
        public string download_url { get; set; }
        public string file_name { get; set; }
        public string tag_name { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public ApiResponse(string _download_url, string _file_name, string _tag_name, string _name, string _description)
        {
            this.download_url = _download_url;
            this.file_name = _file_name;
            this.tag_name = _tag_name;
            this.name = _name;
            this.description = _description;
        }

        public override string ToString()
        {
            string returnval =
                "download_url: " + this.download_url + "\n" +
                "file_name: " + this.file_name + "\n" +
                "tag_name: " + this.tag_name + "\n" +
                "name: " + this.name + "\n" +
                "description: " + this.description;
            return returnval;
        }
    }
}