using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tomware.Microwf.Engine
{
  [Table("WorkItem")]
  public partial class WorkItem : EngineEntity
  {
    public static int WORKITEM_RETRIES = 3;

    [Required]
    public string TriggerName { get; set; }

    [Required]
    public int EntityId { get; set; }

    [Required]
    public string WorkflowType { get; set; }

    public int Retries { get; set; }

    public string Error { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public static WorkItem Create(
      string triggerName,
      int entityId,
      string workflowType,
      DateTime? dueDate = null
    )
    {
      return new WorkItem
      {
        TriggerName = triggerName,
        EntityId = entityId,
        WorkflowType = workflowType,
        Retries = 0,
        DueDate = dueDate ?? SystemTime.Now()
      };
    }
  }
}
