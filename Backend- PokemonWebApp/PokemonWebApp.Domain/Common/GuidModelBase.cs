using System.ComponentModel.DataAnnotations;

namespace PokemonWebApp.Domain.Common
{
    public abstract class GuidModelBase
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        protected GuidModelBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
