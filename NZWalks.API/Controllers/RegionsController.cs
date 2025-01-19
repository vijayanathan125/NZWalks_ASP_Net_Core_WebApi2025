using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https://loaclhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDBContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //Get All Region

        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("The is the custom exception");
                // Get Data from Database - Domain models
                var regionsDomain = await regionRepository.GetAllAsync();
                //Map Domain Models to DTO's using mapper
                var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
                logger.LogInformation($"Finished GetALLRegion request with data:{JsonSerializer.Serialize(regionsDto)}");
                //Return DTO's
                return Ok(regionsDto);
            }
            catch(Exception ex) 
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            //logger.LogInformation("GetAll Action Method was invoked");


            //Map Domain Models to DTO's
            //var regionsDto=new List<RegionDto>();
            //foreach (var region in regionsDomain) {
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl,
            //    });
            //}

          
        }

        //Get Region by ID

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            
            ////var region=dbContext.Regions.Find(id);
            ////Get Region Domain model from the database
            var regionDomain=await regionRepository.GetByIdAsync(id);
            if (regionDomain == null) { return NotFound(); }
            ////Map/Convert to DTO
            //var regionDto = new RegionDto
            //{
            //    Id=regionDomain.Id,
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl,

            //};

            //Mapping Domain model to dto
            var regionDto=mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        //Post to create new region
        //Post: https://localhost:1234/api/regions

        [HttpPost]
        [VallidateModel]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //Map or Convert DTO to Domain Model

                //var regionDomainModel = new Region
                //{
                //    Code = addRegionRequestDto.Code,
                //    Name = addRegionRequestDto.Name,
                //    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                //};

                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                //Use Domain model to  create Region

                //await dbContext.Regions.AddAsync(regionDomainModel);
                //await dbContext.SaveChangesAsync();

                //Repository Pattern
                regionDomainModel = await regionRepository.CreateRegionAsync(regionDomainModel);

                //map Mdomain model to dto
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
           
        }

        //Update Region
        //Put: https://localhost:1234/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [VallidateModel]
        [Authorize(Roles ="Writer")]
        public async  Task<IActionResult>UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           
                //Dto to domain

                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionRequestDto.Code,
                //    Name = updateRegionRequestDto.Name,
                //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
                //};

                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await regionRepository.UpdateRegionAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Conver domain model to dto 
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
           

        }

        //Delete Region
        //Delete: https://localhost:1234/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Writer, Reader")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel=await regionRepository.DeleteRegionAsync(id);
            if(regionDomainModel==null)
            {
                return NotFound();
            }

            ////Delete a region
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            // return deleted region back
            //map domain to dto
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};

            var regionDto = mapper.Map<RegionDto?>(regionDomainModel);
            return Ok(regionDto);
        } 

    }
}
