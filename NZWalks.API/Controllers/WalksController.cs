using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) 
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create Walk
        //Post: /api/walks
        [HttpPost]
        [VallidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
           
                //Map dto to domain model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);

                //Map Domain model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
           
        }

        //Get Walks
        //Get:/api/walks?filterOn=Name&filterQuery=Track&shortBy=Name&IsAscending=true&pageNumber=2&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? shortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize=1000)
        {

           
                var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, shortBy, isAscending ?? true, pageNumber, pageSize);

            //throw a exception (Middleware)
            throw new Exception("Somthing went wrong");

                return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
           
        }

        //Get Walks
        //Get:/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel= await walkRepository.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            //map to dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update Walk by ID
        // Put: /api/Walks/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        [VallidateModel]

        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            
                //map domain to main
                var walkDominModel = mapper.Map<Walk>(updateWalkDto);

                walkDominModel = await walkRepository.UpdateAsync(id, walkDominModel);

                if (walkDominModel == null)
                {
                    return NotFound();
                }

                //Map to Dto
                return Ok(mapper.Map<WalkDto>(walkDominModel));
            
        }

        //Delete a walk by id 
        //Delete: /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var deleteDomainModel = await walkRepository.DeleteAsync(id);
            if(deleteDomainModel == null)
            {
                return NotFound();
            }

            //map to Dto
            return Ok(mapper.Map<WalkDto>(deleteDomainModel));
        }


    }
}
