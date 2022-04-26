namespace Release_Tool
{
    public class file
    {
        public string path { get; set; }
        public string newfilename { get; set; }
        public file(string _path, string _newfilename)
        {
            this.path = _path;
            this.newfilename = _newfilename;
        }
    }
}