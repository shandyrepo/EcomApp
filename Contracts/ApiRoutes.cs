using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApp.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public static class Producs
        {
            public const string GetAll = Root + "/products";
            public const string GetOne = Root + "/products/get";
            public const string Create = Root + "/products/post";
            public const string Delete = Root + "/products/delete";
            public const string Update = Root + "/products/put";
        }

        public static class Orders
        {
            public const string Create = Root + "/orders";
            public const string GetPopularProducts = Root + "/orders/popular";
            public const string GetCustomerOrders = Root + "/orders/ordersbycustomer";
            public const string GetCustomersOverTotalPrice = Root + "/orders/preciouscustomers";

        }
    }
}
