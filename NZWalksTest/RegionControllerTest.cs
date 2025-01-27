using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NZWalks.API.Controllers;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using NZWalksTest.Helper;

namespace NZWalksTest
{
    public class RegionControllerTest
    {
		private readonly Mock<IRegionRepository> _mockRegionRepository;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<ILogger<RegionsController>> _mockLogger;
		private readonly RegionsController _controller;
		public RegionControllerTest()
        {
			// Initialize mocks
			_mockRegionRepository = new Mock<IRegionRepository>();
			_mockMapper = new Mock<IMapper>();
			_mockLogger = new Mock<ILogger<RegionsController>>();
		}

        [Fact]
		public async Task GetAll_ReturnsOkResult_WithRegionsDto()
		{
			// Arrange

			// Sample test data
			var regionsDomain = new List<Region>
			{
				new Region { Id = Guid.NewGuid(), Name = "Region1", Code = "R1", RegionImageUrl = "url1" },
				new Region { Id = Guid.NewGuid(), Name = "Region2", Code = "R2", RegionImageUrl = "url2" }
			};

			var regionsDto = new List<RegionDto>
			{
				new RegionDto { Id = regionsDomain[0].Id, Name = "Region1", Code = "R1", RegionImageUrl = "url1" },
				new RegionDto { Id = regionsDomain[1].Id, Name = "Region2", Code = "R2", RegionImageUrl = "url2" }
			};

			// Mock repository and mapper methods
			_mockRegionRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(regionsDomain);
			_mockMapper.Setup(mapper => mapper.Map<List<RegionDto>>(regionsDomain)).Returns(regionsDto);

			// Create controller with mocked dependencies
			var controller = new RegionsController(_mockRegionRepository.Object, _mockMapper.Object, _mockLogger.Object);

			// Act
			var result = await controller.GetAll();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnValue = Assert.IsType<List<RegionDto>>(okResult.Value);

			Assert.Equal(2, returnValue.Count);
			Assert.Equal("Region1", returnValue[0].Name);
			Assert.Equal("Region2", returnValue[1].Name);

            // Verify logging
            _mockLogger.VerifyLog(LogLevel.Information, Moq.Times.Once, "Finished GetALLRegion request");
		}

		[Fact]

		public async Task GetById_ShouldReturnOkResult_WhenRegionExists() 
		{
			//Arrange
			var regionById = new Region
			{
				Id = Guid.NewGuid(),
				Name = "Test Region Area",
				Code = "TRA",
			};

			_mockRegionRepository.Setup(region => region.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(regionById);
			var exceptedRegioDto = new RegionDto
			{
				Id = regionById.Id,
				Name = regionById.Name,
				Code = regionById.Code,
			};
			_mockMapper.Setup(mapper => mapper.Map<RegionDto>(regionById)).Returns(exceptedRegioDto);

			// Create controller with mocked dependencies
			var controller = new RegionsController(_mockRegionRepository.Object, _mockMapper.Object, _mockLogger.Object);

			//Act
			var result = await controller.GetById(regionById.Id);

			//Assert

			var okresult = Assert.IsType<OkObjectResult>(result);
			var returnedDto = Assert.IsType<RegionDto>(okresult.Value);
			Assert.Equal(exceptedRegioDto.Id, regionById.Id);
			Assert.Equal(exceptedRegioDto.Name, regionById.Name);
		}

		[Fact]

		public async Task GetById_ShouldReturnNotFound_WhenRegionDoesNotExist() 
		{
            //Arrange
			var regionID = Guid.NewGuid();

            _mockRegionRepository.Setup(region => region.GetByIdAsync(regionID)).ReturnsAsync((Region)null);
            var controller = new RegionsController(_mockRegionRepository.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
			var result = await controller.GetById(regionID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task Create_Region_ShouldReturnCreatedResult_WithValidInput()
		{
			//Arrange
			var regionDto = new AddRegionRequestDto
			{
				Name = "Test region area",
				Code = "TRA"
			};
			var reginDomainModel = new Region
			{
				Id= Guid.NewGuid(),
				Name = regionDto.Name,
				Code = regionDto.Code,
			};
			_mockMapper.Setup(mapper=> mapper.Map<Region>(regionDto)).Returns(reginDomainModel);
			_mockRegionRepository.Setup(region => region.CreateRegionAsync(It.IsAny<Region>())).ReturnsAsync(reginDomainModel);

			var expectedRegion = new RegionDto
			{
				Id = reginDomainModel.Id,
				Name = regionDto.Name,
				Code = regionDto.Code,
			};

			_mockMapper.Setup(mapper=>mapper.Map<RegionDto>(reginDomainModel)).Returns(expectedRegion);

			var controller = new RegionsController(_mockRegionRepository.Object, _mockMapper.Object, _mockLogger.Object);

			//Act
			var result = await controller.CreateRegion(regionDto);

			//Assert 
			var createdResult = Assert.IsType<CreatedAtActionResult>(result);
			var returnedDto = Assert.IsType<RegionDto>(createdResult.Value);
			Assert.Equal(expectedRegion.Id, returnedDto.Id);

		}

		[Fact]

		public async Task UpdateRegion_ShouldReturnOkResult_WhenUpdateIsSuccessful()
		{
			//Arrange
			var regionID = Guid.NewGuid();
			var mockUpdateRegionRequestDto = new UpdateRegionRequestDto
			{
				Code = "TRB",
				Name = "Test Region B"
			};
			var mockUpdateRedionDomain = new Region
			{
				Id = regionID,
				Code = mockUpdateRegionRequestDto.Code,
				Name = mockUpdateRegionRequestDto.Name,
			};

			_mockMapper.Setup(mapper=>mapper.Map<Region>(mockUpdateRegionRequestDto)).Returns(mockUpdateRedionDomain);
			_mockRegionRepository.Setup(region => region.UpdateRegionAsync(It.IsAny<Guid>(),It.IsAny<Region>())).ReturnsAsync(mockUpdateRedionDomain);

			var mockexpectedRegionDto = new RegionDto
			{
				Id = mockUpdateRedionDomain.Id,
				Name = mockUpdateRedionDomain.Name,
				Code = mockUpdateRedionDomain.Code,
			};

			_mockMapper.Setup(mapper=>mapper.Map<RegionDto>(mockUpdateRedionDomain)).Returns(mockexpectedRegionDto);

			var controller = new RegionsController(_mockRegionRepository.Object, _mockMapper.Object,_mockLogger.Object);

			//Act
			var updatedResult = await controller.UpdateRegion(regionID, mockUpdateRegionRequestDto);

			Assert.NotNull(updatedResult);
			var okResult = Assert.IsType<OkObjectResult>(updatedResult);
			var returnRegionType = Assert.IsType<RegionDto>(okResult.Value);
			Assert.Equal(mockexpectedRegionDto.Id, returnRegionType.Id);
		}

		[Fact]
		public async Task UpdateRegion_ShouldReturnNotFoundResult_WhenRegionIsNotExist()
		{
			//Arrange
			var regionID = Guid.NewGuid();
			_mockRegionRepository.Setup(region => region.UpdateRegionAsync(It.IsAny<Guid>(), It.IsAny<Region>())).ReturnsAsync((Region)null);

			var controller = new RegionsController(_mockRegionRepository.Object, _mockMapper.Object, _mockLogger.Object);

			//Act

			var result = await controller.UpdateRegion(regionID,new UpdateRegionRequestDto() {Code="xyz",Name="Vijay" });

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task DeleteRegion_ShouldReturnOkResult_WhenDeleteIsSuccessful()
		{
			//Arrange
			var regionID = Guid.NewGuid();
			var mockDeleteRegionDomain = new Region
			{
				Id = regionID,
				Name = "Name",
				Code = "Code",
			};
			
			_mockRegionRepository.Setup(region=>region.DeleteRegionAsync(regionID)).ReturnsAsync(mockDeleteRegionDomain);

			var mockDeleteRegionDto = new RegionDto
			{
				Id = mockDeleteRegionDomain.Id,
				Name = mockDeleteRegionDomain.Name,
				Code = mockDeleteRegionDomain.Code,
			};

			_mockMapper.Setup(mapper=>mapper.Map<RegionDto>(mockDeleteRegionDomain)).Returns(mockDeleteRegionDto);

			var controller = new RegionsController(_mockRegionRepository.Object,_mockMapper.Object, _mockLogger.Object);

			//Act 

			var result = await controller.DeleteRegion(regionID);

			var deletedOkResult = Assert.IsType<OkObjectResult>(result);
			var returnedType = Assert.IsType<RegionDto>(deletedOkResult.Value);
			Assert.Equal(mockDeleteRegionDto.Id, returnedType.Id);
		}

		[Fact]
		public async Task DeleteRegion_ShouldReturnNotFound_WhenRegionIsNotExist()
		{
			//Arrange
			var regionID = Guid.NewGuid();
			_mockRegionRepository.Setup(region => region.DeleteRegionAsync(regionID)).ReturnsAsync((Region)null);
			var controller = new RegionsController(_mockRegionRepository.Object,_mockMapper.Object,_mockLogger.Object);

			//Act
			var result = await controller.DeleteRegion(regionID);

			var resultNotFound = Assert.IsType<NotFoundResult>(result);
		}


    }
}