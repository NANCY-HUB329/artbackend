using Arts.Models.Dtos;



namespace Arts.Services.IService
{




    public interface IArtService
    {

        Task<IEnumerable<Art>> GetAllArts();
        Task<Art> GetArtById(Guid Id);
        Task<List<Art>> GetOpenArts();
        Task<List<Art>> GetClosedArts();
        Task<string> AddArt(Art art);
        Task<string> UpdateArt();
        Task<string> DeleteArt( Art art);

    }
}




