using CityInfo.API.Entities;

namespace CityInfo.API.Sevices
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>>GetCitiesAsync();

        Task<(IEnumerable<City>, PaginationMetadata)>GetCitiesAsync(
            string? name, string? searchQuery,  int pageNumber, int PageSize);

        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);

        Task<bool> CityExistsAsync(int cityId);

        Task<IEnumerable<PointOfInterest>>GetPointsOFInterestForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOFInterestForCityAsync(int cityId,
            int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

        Task<bool> SaveChangesAsync();      
    }
}
