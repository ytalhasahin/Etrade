using Etrade.DAL.Abstract;
using Etrade.Entity.Models.Entities;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderDAL _orderDal;
        private readonly IOrderlineDAL _orderlineDal;
        private readonly IProductDAL _productDal;
        public OrderController(IOrderDAL orderDal, IOrderlineDAL orderlineDal, IProductDAL productDal)
        {
            _orderDal = orderDal;
            _orderlineDal = orderlineDal;
            _productDal = productDal;
        }

        public IActionResult Index()
        {
            return View(_orderDal.GetAll());
        }
        public IActionResult Details(int id)
        {
            var order = _orderDal.Get(id);
            //orderlinedal yazılmalı
            return View(order);
        }
        public IActionResult Delete(int id)
        {
            var order = _orderDal.Get(id);
            _orderDal.Delete(order);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var order = _orderDal.Get(id);
            var model = new OrderStateViewModel()
            {
                OrderId = order.Id,
                IsCompleted = false
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(OrderStateViewModel model)
        {
            var order = _orderDal.Get(model.OrderId);
            if(model.IsCompleted)
                order.OrderState = OrderState.Completed;
            else
                order.OrderState = OrderState.Waiting;
            return RedirectToAction("Index");

        }
    }
}
