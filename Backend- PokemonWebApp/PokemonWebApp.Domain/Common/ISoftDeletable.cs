namespace PokemonWebApp.Domain.Common
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        void BringBack();
        void Delete();
    }
}
