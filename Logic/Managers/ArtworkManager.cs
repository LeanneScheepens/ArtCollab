
using System.ComponentModel.DataAnnotations;
using Logic.Interfaces;
using Logic.Models;
using Logic.ViewModels;

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
        public List<Artwork> GetArtworksByOwner(string owner)
        {
            return GetArtworks().Where(a => a.Owner == owner).ToList();
        }
  
        public void UpdateArtwork(EditArtworkViewModel viewModel)
        {

            var validationContext = new ValidationContext(viewModel);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(viewModel, validationContext, validationResults, true);

            if (!isValid)
            {
                string errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException($"Validatie mislukt: {errorMessages}");
            }

            // Ophalen bestaand artwork
            var artwork = _artworkRepository.GetArtworkById(viewModel.ArtworkId);

            if (artwork == null)
                throw new ArgumentNullException($"Artwork met id {viewModel.ArtworkId} niet gevonden.");

            // Mapping van ViewModel naar Entity
            artwork.Title = viewModel.Title;
            artwork.ImageUrl = viewModel.ImageUrl;
            artwork.Owner = viewModel.Owner;
            artwork.Description = viewModel.Description;
            artwork.UploadDate = viewModel.UploadDate;

            // Opslaan via repository
            _artworkRepository.UpdateArtwork(artwork);
        }
    }
}
