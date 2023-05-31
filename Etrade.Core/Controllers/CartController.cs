using Etrade.Core.Models.Helper;
using Etrade.DAL.Abstract;
using Etrade.Entity.Models.Entities;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductDAL _productDal;
        private readonly IOrderDAL _orderDal;

        public CartController(IProductDAL productDal, IOrderDAL orderDal)
        {
            _productDal = productDal;
            _orderDal = orderDal;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart != null)
                ViewBag.Total = cart.Sum(i => i.Product.Price * i.Quantity).ToString("c");
            return View(cart);
        }
        [HttpPost]
        public IActionResult Buy(int id, int quantity)
        {
            if (SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "Cart") == null)
            {
                var cart = new List<CartItem>();
                cart.Add(new CartItem()
                {
                    Product = _productDal.Get(id),
                    Quantity = quantity
                });
                SessionHelper.SetAsJson(HttpContext.Session, "Cart", cart);
            }
            else
            {
                var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "Cart");
                int index = isIndex(cart, id);
                if (index == -1)
                {
                    cart.Add(new CartItem()
                    {
                        Product = _productDal.Get(id),
                        Quantity = quantity
                    });
                }
                else
                    cart[index].Quantity += quantity;
                SessionHelper.SetAsJson(HttpContext.Session, "Cart", cart);
                
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddToCart(int Id, int Q)
        {
            if (SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "Cart") == null)
            {
                var cart = new List<CartItem>();
                Product product = _productDal.Get(Id);
                cart.Add(new CartItem()
                {
                    Product = product,
                    Quantity = Q
                });
                SessionHelper.SetAsJson(HttpContext.Session, "Cart", cart);
                SessionHelper.Count = cart.Count;
            }
            else
            {
                var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "Cart");
                int index = isIndex(cart, Id);
                if (index == -1)
                {
                    cart.Add(new CartItem()
                    {
                        Product = _productDal.Get(Id),
                        Quantity = Q
                    });
                }
                else
                    cart[index].Quantity += Q;
                SessionHelper.SetAsJson(HttpContext.Session, "Cart", cart);
                SessionHelper.Count = cart.Count;

            }
            return Ok();
        }
        public IActionResult Remove(int id)
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            int index = isIndex(cart, id);
            cart.RemoveAt(index);
            SessionHelper.SetAsJson(HttpContext.Session, "Cart", cart);
            return RedirectToAction("Index");
        }

        private int isIndex(List<CartItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                    return i;
            }
            return -1;
        }

        public IActionResult CheckOut()
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                ModelState.AddModelError("NullCartError", "Sepetinizde hiç bir ürün bulunmamamakta");
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CheckOut(ShippingDetail model)
        {
            var cart = SessionHelper.GetFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (ModelState.IsValid)
            {
                SaveOrder(cart, model);
                cart.Clear();
                SessionHelper.Count = 0;
                return View("Completed");
            }
            return View(model);
        }

        private void SaveOrder(List<CartItem>? cart, ShippingDetail model)
        {
            var order = new Order(OrderState.Waiting, DateTime.Now, cart.Sum(c => c.Product.Price * c.Quantity), model.Username, model.AddressTitle, model.Address, model.City);

            order.OrderLines = new List<OrderLine>();
            foreach (var line in cart)
            {
                var orderline = new OrderLine(line.Quantity, line.Product.Price, line.Product);
                orderline.ProductId = line.Product.Id;
                order.OrderLines.Add(orderline);
            }
            _orderDal.Add(order);
        }


    }
}
