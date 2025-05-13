using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtCollab.Models;

namespace Logic.Interfaces
{
    public class IArtistRepository
    {
        public void CreateArtist(Artist artist);
        public List<Artist> GetArtist();
        public void DeleteArtist(int id);

    }
}
