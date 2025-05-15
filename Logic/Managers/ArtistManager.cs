using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Interfaces;

namespace Logic.Managers
{
    public class ArtistManager
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistManager(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public List<Artist> GetArtists()
        {
            return _artistRepository.GetArtists();
        }

        public void DeleteArtist(List<int> ids)
        {
            foreach (int id in ids)
            {
                _artistRepository.DeleteArtist(id);
            }
        }

        public void CreateArtist(Artist artist)
        {
            _artistRepository.CreateArtist(artist);
        }
    }
}


