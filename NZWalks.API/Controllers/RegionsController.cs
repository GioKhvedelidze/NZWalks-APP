using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository _regionRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<RegionsController> _logger;

    public RegionsController(IRegionRepository regionRepo, IMapper mapper, ILogger<RegionsController> logger)
    {
        _regionRepo = regionRepo;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    //[Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var allRegions = await _regionRepo.GetAllAsync();
            
            _logger.LogInformation("Response{Serialize}",JsonSerializer.Serialize(allRegions));
        
            return Ok(allRegions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            throw;
        }
    }

    [HttpGet]
    [Route("{id:guid}")]
    //[Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var region = await _regionRepo.GetByIdAsync(id);

        if (region == null)
            return NotFound();

        return Ok(_mapper.Map<RegionDto>(region));
    }

    [HttpPost]
    [ValidateModel]
    //[Authorize(Roles = "Writer")]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

        await _regionRepo.CreateRegionAsync(regionDomainModel);
        
        return CreatedAtAction(nameof(GetById), new {id = regionDomainModel.Id}, regionDomainModel);
    }

    [HttpPut]
    [Route("{id:guid}")]
    //[Authorize(Roles = "Writer")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    { 
        var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            
       regionDomainModel =  await _regionRepo.UpdateRegionAsync(id, regionDomainModel);
        
        if (regionDomainModel == null)
            return NotFound();

        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
        
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    //[Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var regionDomainModel = await _regionRepo.DeleteRegion(id);

        if (regionDomainModel == null)
            return NotFound();

        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
        
        return Ok(regionDto);
    }
}