namespace L10N.API.SyncFunction.Model
{
    public class AppMetaData
    {
        public Guid id { get; set; }
        public string Language { get; set; }


        public List<MetaDataModel> MetaData { get; set; }
        public string AppName { get; set; }
    }
}
