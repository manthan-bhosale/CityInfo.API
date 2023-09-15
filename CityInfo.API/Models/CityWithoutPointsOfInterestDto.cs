namespace CityInfo.API.Models
{   
    /// <summary>
    /// A DTO for city without points of interest
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {   

        /// <summary>
        /// The id of city
        /// </summary>
         public int Id {  get; set; }

        /// <summary>
        /// Name of city
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of city
        /// </summary>
        public string? Description { get; set; }
    }
}
