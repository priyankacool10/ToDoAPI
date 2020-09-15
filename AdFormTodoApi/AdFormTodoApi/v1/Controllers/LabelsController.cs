using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Services;
using AdFormTodoApi.v1.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AdFormTodoApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IMapper _mapper;

        public LabelsController(ILabelService labelService, IMapper mapper)
        {
            _labelService = labelService;
            _mapper = mapper;
        }

        // GET: api/Labels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Label>>> GetLabels()
        {
            var labels = await _labelService.GetAllLabel();
            var labelDTO = _mapper.Map<IEnumerable<Label>, IEnumerable<LabelDTO>>(labels);
            if (labels == null)
            {
                return NotFound(new { message = "Labels does not exist" });
            }
            return Ok(labelDTO);
        }

        // GET: api/Labels/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Label>> GetLabel(long id)
        {
            var label = await _labelService.GetLabelById(id);
            var labelDTO = _mapper.Map<Label, LabelDTO>(label);
            if (label == null)
            {
                return NotFound(new { message = "Label with id : {0} does not exist", id });
            }
            return Ok(labelDTO);
        }

        // PUT: api/Labels/5
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutLabel(long id, LabelDTO labelDTO)
        {
            var label = _mapper.Map<LabelDTO, Label>(labelDTO);
            if (id != label.Id)
            {
                return BadRequest();
            }

            await _labelService.UpdateLabel(id, label);
            return NoContent();
        }

        // POST: api/Labels
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Label>> PostLabel([FromBody]LabelDTO labelDTO)
        {
            if (labelDTO.Name == null)
                return BadRequest(new { message = "Label Name mandatory" });
            var label = _mapper.Map < LabelDTO, Label>(labelDTO);
            await _labelService.CreateLabel(label);
            return CreatedAtAction("GetLabel", new { id = label.Id }, label);
        }

        // DELETE: api/Labels/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Label>> DeleteLabel(long id)
        {
            await _labelService.DeleteLabel(id);
            return NoContent();
        }

    }
}
