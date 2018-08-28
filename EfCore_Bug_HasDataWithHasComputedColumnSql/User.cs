using System.ComponentModel.DataAnnotations;

namespace EfCore_Bug_HasDataWithHasComputedColumnSql
{
    public class User
    {
        private string fullName = default;

        [Key]
        public long Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName {
            get => fullName ?? FirstName + " " + LastName;
            set => fullName = value;
        }
    }
}
