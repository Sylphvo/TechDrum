namespace TechDrum.Core.Models.History
{
    public class HistoryTransactionModel
    {
          public string username { get; set; }
          public int pageIndex { get; set; }
          public int pageSize { get; set; }
          public string searchString { get; set; }
          public string type { get; set; }
    }
}
