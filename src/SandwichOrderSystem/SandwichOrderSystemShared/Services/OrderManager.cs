﻿using SandwichOrderSystemShared.Models;
using System.Linq;
using static SandwichOrderSystemShared.Constants;

namespace SandwichOrderSystemShared.Services
{
    public class OrderManager : IOrderManager
    {
        public IOrders Orders { get; }

        private IDiscounter discounter;

        public OrderManager(IDiscounter discounter)
        {
            this.discounter = discounter;

            Orders = new Orders();
            ResetCurrentOrder();
        }

        private IOrder currentOrder;
        public IOrder CurrentOrder
        {
            get
            {
                if (currentOrder == null)
                {
                    ResetCurrentOrder();
                    return currentOrder;
                }
                else
                {
                    return currentOrder;
                }
            }

            private set
            {
                currentOrder = value;
            }
        }

        public int Count
        {
            get
            {
                if (Orders != null && Orders.OrderCollection != null)
                {
                    return Orders.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public decimal OrdersPrice
        {
            get
            {
                return Orders.OrderCollection
                    .Sum(o => o.Items
                    .Sum(i => i.Price));
            }
        }

        public decimal CurrentOrderPrice
        {
            get
            {
                return currentOrder.Items
                    .Sum(i => i.Price);
            }
        }

        public void AddItemToCurrentOrder(IItem item)
        {
            addItemToOrder(item);
            addDiscountConditionally();
        }

        public void AddCurrentOrderToOrders()
        {
            if (CurrentOrder != null && CurrentOrder.Items.Count > 0)
            {
                Orders.Add(CurrentOrder);
                ResetCurrentOrder();
            }
        }

        public void ResetCurrentOrder()
        {
            CurrentOrder = new Order();
        }

        public void ResetOrders()
        {
            ResetCurrentOrder();
            Orders.Reset();
        }

        public void FinishOrders(PaymentMethodType type)
        {
            // TODO: Pass order along to payment and repository services
            switch (type)
            {
                case PaymentMethodType.CreditCard:
                    break;
                case PaymentMethodType.Cash:
                    break;
                default:
                    break;
            }

            ResetOrders();
        }

        private void addItemToOrder(IItem item)
        {
            if (item != null)
            {
                CurrentOrder.Items.Add(item);
            }
        }

        private void addDiscountConditionally()
        {
            var discountItem = discounter.GetDiscountItemConditionally(CurrentOrder);
            if (discountItem != null)
            {
                addItemToOrder(discountItem);
            }
        }
    }
}
