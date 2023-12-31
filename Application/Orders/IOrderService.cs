﻿using Data.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Models.Analystic;
using Models.Orders;

namespace Application.Orders
{
    public interface IOrderService
    {
        Task<string> Create(OrderCreateRequest req);

        Task<string> PayOrder(string id);

        Task<int> Update(OrderUpdateRequest req);

        Task<int> Delete(string id);

        Task<List<OrderVm>> GetAllOrder();

        Task<List<OrderVm>> GetAllBooking();

        Task<List<OrderVm>> GetCurrentBooking();

        Task<OrderVm> GetById(string id);

        Task<OrderVm> GetReccentlyOrder(string id);

        Task<int> UpdateOrderStatus(string id, JsonPatchDocument<Order> patchDoc);

        Task<List<RevenueVm>> GetRevenue();

        Task<List<TopSellersVm>> GetTopSellers();

        Task<CountManagementVm> GetCountManagement();
    }
}