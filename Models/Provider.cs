using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgilerapProcessSystems.Models
{
    public partial class Provider
    {
        public int ID { get; set; }
        public int? WorkID { get; set; }
        public int? UserID { get; set; }
        public int? CreateByID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CreateDate { get; set; }
        public int? UpdateByID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

        [ForeignKey("CreateByID")]
        public virtual User CreateBy { get; set; }
        [ForeignKey("UpdateByID")]
        public virtual User UpdateBy { get; set; }
        public virtual Work Work { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

    }
}
