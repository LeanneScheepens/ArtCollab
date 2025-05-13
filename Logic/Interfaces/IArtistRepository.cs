using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface IArtistRepository
    {
        List<Artist> GetArtists();
        Artist GetArtistById(int id);
        void CreateArtist(Artist artist);    
        void DeleteArtist(int id);
    }
}
