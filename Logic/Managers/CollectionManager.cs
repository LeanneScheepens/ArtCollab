using Logic.Models;
using Logic.Interfaces;

namespace Logic.Managers
{
    public class CollectionManager
    {
        private readonly ICollectionRepository _collectionrepository;

        public CollectionManager(ICollectionRepository Collectionrepository)
        {
            _collectionrepository = Collectionrepository;
        }

        public void CreateCollection(Collection collection) => _collectionrepository.CreateCollection(collection);

        public List<Collection> GetCollectionsByOwner(string owner) => _collectionrepository.GetCollectionsByOwner(owner);

        public void AddArtworkToCollection(int collectionId, int artworkId)
        {
            _collectionrepository.AddArtworkToCollectionIfNotExists(collectionId, artworkId);
        }
        public void DeleteCollection(int collectionId, string owner)
        {
            _collectionrepository.DeleteCollection(collectionId, owner);
        }
        public bool ArtworkExistsInCollection(int collectionId, int artworkId)
        {
            var collection = GetCollectionById(collectionId);
            return collection?.Artworks?.Any(a => a.Id == artworkId) == true;
        }


        public Collection GetCollectionById(int id) => _collectionrepository.GetCollectionById(id);
    }
}
