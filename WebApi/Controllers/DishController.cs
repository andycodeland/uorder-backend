﻿using Application.Dishes;
using Microsoft.AspNetCore.Mvc;
using Models.Dishes;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// Gets the list of all dishes.
        /// </summary>
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var dish = _dishService.GetAllDish();
            return Ok(dish);
        }

        /// <summary>
        /// Get the dish specified by Id
        /// </summary>
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _dishService.GetDishById(id);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new dish.
        /// </summary>
        [HttpPost("post")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] DishCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _dishService.Create(req);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Delete the dish specified by Id
        /// </summary>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _dishService.Delete(id);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        /// <summary>
        /// Update the dish specified by Id
        /// </summary>
        [HttpPut("put/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] DishUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dishService.Update(req);
            if (result == 0)
                return BadRequest();
            return Ok();
        }
    }
}