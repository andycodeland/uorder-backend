﻿using Application.Orders;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.Orders;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderhService, IHubContext<OrderHub> hubContext)
        {
            _orderService = orderhService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Gets the list of all orders.
        /// </summary>
        [Authorize(Roles = "admin,staff")]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _orderService.GetAllOrder();
            return Ok(list.OrderBy(x => x.CreatedAt));
        }

        /// <summary>
        /// Get the order specified by Id
        /// </summary>
        [Authorize(Roles = "admin,staff")]
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _orderService.GetById(id);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [Authorize(Roles = "admin,staff")]
        [HttpPost("post")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] OrderCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _orderService.Create(req);
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "Có đơn hàng mới!");
            if (result == null)
                return Ok();
            return Ok(result);
        }

        /// <summary>
        /// Delete the order specified by Id
        /// </summary>
        [Authorize(Roles = "admin,staff")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _orderService.Delete(id);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        /// <summary>
        /// Update the order specified by Id
        /// </summary>
        [Authorize(Roles = "admin,staff")]
        [HttpPut("put/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromForm] OrderUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderService.Update(req);
            if (result == 0)
                return BadRequest();
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "Đơn hàng vừa được cập nhập!");
            return Ok();
        }

        /// <summary>
        /// Update the order status specified by Id
        /// </summary>
        [Authorize(Roles = "admin,staff")]
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] JsonPatchDocument<Order> patchDoc)
        {
            var result = await _orderService.UpdateOrderStatus(id, patchDoc);
            if (result == null)
                return BadRequest();
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "Đơn hàng vừa được cập nhập!");
            return Ok();
        }
    }
}