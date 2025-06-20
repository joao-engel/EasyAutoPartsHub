namespace EasyAutoPartsHub.Lib
{
    public static class Body
    {
        private static readonly string _pathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateOrcamento");

        public static string Template(string template)
        {
            string path = Path.Combine(_pathTemplate, template);
            return File.ReadAllText(path);
        }
    }
}
