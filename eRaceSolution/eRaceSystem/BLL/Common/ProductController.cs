using eRaceSystem.DAL;
using eRaceSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL.Common
{
    [DataObject]
    public class ProductController
    {
        #region Sales Methods
        //product get method using category id
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Product> Products_GetByCategoryID(int categoryID)
        {
            using (var context = new ERaceContext())
            {
                var products = from x in context.Products
                               where x.CategoryID.Equals(categoryID)
                               select x;
                return products.ToList();
            }
        }
        #endregion

        public Product Product_Get(int productid)
        {
            using (var context = new ERaceContext())
            {
                return context.Products.Find(productid);
            }
        }

        public Product Product_Get(string product)
        {
            using (var context = new ERaceContext())
            {
                var item = (from x in context.Products
                            where x.ItemName.Equals(product)
                            select x).FirstOrDefault();
                return item;
            };
        }
    }
}
