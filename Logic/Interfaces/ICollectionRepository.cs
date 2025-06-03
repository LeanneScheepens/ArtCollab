using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using ArtCollab.Models;


namespace Logic.Interfaces
{
    public interface ICollectionRepository
    {
        void CreateCollection(Collection collection);
        List<Collection> GetCollectionsByOwner(string owner);
        void AddArtworksToCollection(int collectionId, List<int> artworkIds);
        Collection GetCollectionById(int id);
        void AddArtworkToCollectionIfNotExists(int collectionId, int artworkId);
        void DeleteCollection(int collectionId, string owner);

    }
}
