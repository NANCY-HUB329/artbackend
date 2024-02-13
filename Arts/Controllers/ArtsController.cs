using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Arts.Models.Dtos;
using Arts.Services.IService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Arts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        private readonly IArtService _artService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public ArtsController(IArtService artService , IMapper mapper)
        {
            _artService = artService;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtDto>>> GetArts()
        {
            var arts = await _artService.GetAllArts();
            return Ok(arts);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtDto>> GetArtById(Guid id)
        {
            var art = await _artService.GetArtById(id);

            if (art == null)
            {
                return NotFound();
            }

            return Ok(art);
        }

        [HttpGet("Open")]
        public async Task<ActionResult<ResponseDto>> GetOpenArt()
        {
            var res = await _artService.GetOpenArts();
            _response.Result = res;
            return Ok(_response);
        }
        
        
           

        [HttpGet("Closed")]
        public async Task<ActionResult<ResponseDto>> GetClosedArt()
        {
            var res = await _artService.GetClosedArts();
            _response.Result = res;
            return Ok(_response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateArt(ArtDto artDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                _response.Errormessage = "Please login to add art";
                return Unauthorized(_response);
             }

            var art = _mapper.Map<Art>(artDto);
            art.SellerId = Guid.Parse(UserId);
            var res = await _artService.AddArt(art);
            _response.Result = res;
            return Created("", _response);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto>> UpdateArt(Guid id, ArtDto artDto)
        {
            var art = await _artService.GetArtById(id);

            if (art == null)
            {
                return NotFound();
            }

            _mapper.Map(artDto, art);

            var res = await _artService.UpdateArt();
            _response.Result = res;
            return Created("", _response);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto>> DeleteArt(Guid id)
        {
            var art = await _artService.GetArtById(id);
            if (art.Description == null)
            {
                _response.Errormessage = "Art Not Found";
            }

            var res = await _artService.DeleteArt(art);
            _response.Result = res;
            return Ok(_response);
        }
    }
}
