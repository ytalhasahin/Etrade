using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.Entities
{
    public class Order
    {
        public Order()
        {
        }
        public Order(OrderState orderState, DateTime orderDate, decimal total, string userName, string addressTitle, string address, string city)
        {
            OrderState = orderState;
            OrderDate = orderDate;
            Total = total;
            UserName = userName;
            AddressTitle = addressTitle;
            Address = address;
            City = city;
        }

        public int Id { get; set; }
        public OrderState OrderState { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string UserName { get; set; }
        public string AddressTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
