namespace L10N.API.SyncFunction.Model
{
    public class App10NKeysandValuescs
    {
        public Guid id { get; set; }
        public string AppName { get; set; }

        public string LocaleCode { get; set; }

        public string ResourcKey { get; set; }

        public string LocaleValue { get; set; }

        //public Dictionary<string, string> keyValues = new Dictionary<string, string>();

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public AllKeyValues AllKeyValues { get; set; }
    }
}
