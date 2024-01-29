using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgilerapProcessSystems.Models
{
    public class WorkLogMetadata
    {
        [DisplayName("Update By")]
        public virtual User UpdateBy { get; set; }
        public int? No { get; set; }
    }
    [ModelMetadataType(typeof(WorkLogMetadata))]
    public partial class WorkLog
    {
        [NotMapped]
       public WorkLog? WorkLogChangeNext {  get; set; } 
    }
}
