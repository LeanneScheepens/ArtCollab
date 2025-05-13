using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Logic.Managers;
using Logic.Interfaces;
using Logic.Models;
using Data;

namespace ArtCollab.Pages
{
    public class ArtistOverviewModel(ArtistManager artistManager) : PageModel
    {

        private readonly ArtistManager _artistManager = artistManager;

        public List<Artist> Artists { get; set; } = [];

        public void OnGet()
        {
           Artists = _artistManager.GetArtists();
        }
    }
}

