using ArtCollab.Models;
using Logic.Interfaces;
using Logic.Models;

namespace Logic.Managers
{
    public class ArtworkManager
    {
        private readonly IArtworkRepository _artworkRepository;
        public ArtworkManager(IArtworkRepository artworkRepository)
        {
            _artworkRepository = artworkRepository;
        }

        public List<Artwork> GetArtworks()
        {
            return _artworkRepository.GetArtworks();
        }
        public void CreateArtwork(Artwork artwork)
        {
            _artworkRepository.CreateArtwork(artwork);
        }
        public void DeleteArtwork(List<int> ids)
        {
            foreach (int id in ids)
            {
                _artworkRepository.DeleteArtwork(id);
            }
        }
        public Artwork GetArtworkById(int id)
        {
            return _artworkRepository.GetArtworkById(id);
        }
    }
}
