namespace PokemonWebApp.Domain.Common
{
    public interface IHasTimestamps
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
