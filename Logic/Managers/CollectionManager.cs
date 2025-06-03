using Logic.Models;
using Logic.Interfaces;

namespace Logic.Managers
{
    public class CollectionManager
    {
        private readonly ICollectionRepository _repo;

        public CollectionManager(ICollectionRepository repo)
        {
            _repo = repo;
        }

        public void CreateCollection(Collection collection) => _repo.CreateCollection(collection);

        public List<Collection> GetCollectionsByOwner(string owner) => _repo.GetCollectionsByOwner(owner);

        public void AddArtworkToCollection(int collectionId, int artworkId)
        {
            _repo.AddArtworkToCollectionIfNotExists(collectionId, artworkId);
        }
        public void DeleteCollection(int collectionId, string owner)
        {
            _repo.DeleteCollection(collectionId, owner);
        }


        public Collection GetCollectionById(int id) => _repo.GetCollectionById(id);
    }
}
