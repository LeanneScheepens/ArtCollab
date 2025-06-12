using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface IArtworkRepository
    {
        void CreateArtwork(Artwork artwork);
        List<Artwork> GetArtworks();
        void DeleteArtwork(int id);
        Artwork GetArtworkById(int id);
        void UpdateArtwork(Artwork artwork);
    }
}
