namespace EasyAutoPartsHub.Lib
{
    public static class Body
    {
        private static readonly string _pathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateOrcamento");
        private static readonly string _pathImages = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public static string Template(string template)
        {
            string path = Path.Combine(_pathTemplate, template);
            return File.ReadAllText(path);
        }

        public static byte[] Logo(string image)
        {
            string path = Path.Combine(_pathImages, image);
            return File.ReadAllBytes(path);
        }
    }
}
