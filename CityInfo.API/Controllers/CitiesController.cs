using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        
        private readonly ICityInfoRepository _cityinforepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityinforepository,
            IMapper mapper) 
        {
            _cityinforepository = cityinforepository ?? throw new ArgumentNullException(nameof(cityinforepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public  async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string ? name, string? searchQuery, int pageNumber = 1, int PageSize = 10)
        {   
            if(PageSize > maxCitiesPageSize)
            {
                PageSize = maxCitiesPageSize;
            }
            var (cityEntities, paginationMetadata) = await  _cityinforepository
                .GetCitiesAsync(name, searchQuery, pageNumber, PageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }
        /// <summary>
        /// Get City by id
        /// </summary>
        /// <param name="id">The Id of city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include pointsofInterest</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task <IActionResult> GetCity(
            int id, bool includePointsOfInterest = false)
        {   
            var city = await _cityinforepository.GetCityAsync(id, includePointsOfInterest);
            if(city == null)
            {
                return NotFound();
            }
            if(includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
