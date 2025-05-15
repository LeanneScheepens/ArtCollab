using ArtCollab.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtCollab.Pages
{
    public class ArtworkOverviewModel : PageModel
    {


              private readonly ArtworkManager _artworkManager;

            public List<Artwork> Artworks { get; set; }

            public ArtworkOverviewModel(ArtworkManager artworkManager)
            {
                _artworkManager = artworkManager;
            }

            public void OnGet()
            {
                Artworks = _artworkManager.GetArtworks();
            }
        }
    }

