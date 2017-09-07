﻿using SandwichOrderSystemShared.Models;
using System.Collections.Generic;

namespace SandwichOrderSystemShared.Services
{
    public interface IOrderManager
    {
        IOrders Orders { get; }
        IOrder CurrentOrder { get; }
        int Count { get; }

        void AddItemToOrder(IItem item);
        void AddOrderToOrders();
        void ResetCurrentOrder();
        void ResetOrders();
        void FinishOrders();
    }
}
