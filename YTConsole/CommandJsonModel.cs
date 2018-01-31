namespace YTConsole
{
  public class CommandJsonModelArray
  {
    public CommandJsonModel[] theArray;

    public override string ToString()
    {
      return $"{nameof(theArray)}: {theArray}";
    }
  }

  public class CommandJsonModel
  {
    public string CorrelationId { get; set; }
    public string PurchaseOrderId { get; set; }
    public string DataAreaId { get; set; }

    public override string ToString()
    {
      return $"{nameof(CorrelationId)}: {CorrelationId}, {nameof(PurchaseOrderId)}: {PurchaseOrderId}, {nameof(DataAreaId)}: {DataAreaId}";
    }
  }
  
}

/*
[
  {
    "CorrelationId": "c2cd3f56-1f3f-45ed-ba6a-d674f2ab26a4",
    "PurchaseOrderId": "12319482",
    "DataAreaId": "bgh"
  }
]
*/