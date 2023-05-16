namespace L10N.API.Model
{
    public class AppMetaData
    {

        public Guid id { get; set; }
        public string Language { get; set; }


        public List<MetaData> MetaData { get; set; }
        public string AppName { get; set; }
    }
}
