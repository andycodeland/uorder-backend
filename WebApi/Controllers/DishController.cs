﻿using Application.Dishes;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models.Dishes;

namespace WebApi.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "admin,creator,staff")]
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var dish = _dishService.GetAll();
            return Ok(dish);
        }

        /// <summary>
        /// Get the dish specified by Id
        /// </summary>
        [Authorize(Roles = "admin,creator")]
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _dishService.GetById(id);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new dish.
        /// </summary>
        [Authorize(Roles = "admin,creator")]
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
        [Authorize(Roles = "admin,creator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _dishService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// Undo delete action
        /// </summary>
        [Authorize(Roles = "admin,creator")]
        [HttpPost("undoDelete/{itemId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UndoDelete(string itemId)
        {
            var result = await _dishService.UndoDelete(itemId);
            return Ok(result);
        }

        /// <summary>
        /// Update the dish specified by Id
        /// </summary>
        [Authorize(Roles = "admin,creator")]
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

        /// <summary>
        /// Update the dish active status specified by Id
        /// </summary>
        [Authorize(Roles = "admin,creator")]
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] JsonPatchDocument<Dish> patchDoc)
        {
            var result = await _dishService.UpdateStatus(id, patchDoc);
            if (result == 0)
                return BadRequest();
            return Ok();
        }
    }
}