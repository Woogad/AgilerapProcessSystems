using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgilerapProcessSystems.Models
{
    public partial class Work
    {
        public int ID { get; set; }
        public int? HeaderID { get; set; }
        public string? Project { get; set; }
        public string? Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public int? StatusID { get; set; }
        public string? Remark { get; set; }
        public int? CreateByID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }
        public int? UpdateByID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CreateByID")]
        public virtual User CreateBy { get; set; }
        [ForeignKey("UpdateByID")]
        public virtual User UpdateBy { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<WorkLog> WorkLog { get; set; }
        public virtual ICollection<Provider> Provider { get; set; }

    }
}
