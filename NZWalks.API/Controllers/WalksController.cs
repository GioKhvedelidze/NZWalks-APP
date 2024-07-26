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
[Authorize]
public class WalksController : ControllerBase
{
    private readonly IWalkRepository _walkRepo;
    private readonly IMapper _mapper;

    public WalksController(IWalkRepository walkRepo, IMapper mapper)
    {
        _walkRepo = walkRepo;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Authorize(Roles = "Writer")]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

        await _walkRepo.CreateWalkAsync(walkDomainModel);

        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 15)
    {
        var walksDomainModel = await _walkRepo.GetAllAsync(isAscending ?? true, sortBy, filterQuery,
            filterOn, pageNumber, pageSize);
        
        return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomainModel = await _walkRepo.GetByIdAsync(id);

        if (walkDomainModel == null)
            return NotFound();

        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

        walkDomainModel = await _walkRepo.UpdateWalkAsync(id, walkDomainModel);

        if (walkDomainModel == null)
            return NotFound();

        var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

        return Ok(walkDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var walkDomainModel = await _walkRepo.DeleteWalkAsync(id);

        if (walkDomainModel == null)
            return NotFound();

        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }
}