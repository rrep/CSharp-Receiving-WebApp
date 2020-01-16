using eRaceSystem.DAL;
using eRaceSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL.Receiving
{
    [DataObject]
    public class UnOrderedItemsController
    {
        public void Clear_UnOrderedItems()
        {
            using (var context = new ERaceContext())
            {
                context.UnOrderedItems.RemoveRange(context.UnOrderedItems);
                context.SaveChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnOrderedItem> UnOrderedItems_List()
        {
            using (var context = new ERaceContext())
            {
                return context.UnOrderedItems.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_UnOrderedItems(UnOrderedItem item)
        {
            using (var context = new ERaceContext())
            {
                List<string> reasons = new List<string>();
                if(item.Quantity == null || item.Quantity < 1)
                {
                    reasons.Add("Item quantity must not be 0");
                }
                if (reasons.Count() > 0)
                {
                    throw new BusinessRuleException("Add item failed", reasons);
                }
                else
                {
                    context.UnOrderedItems.Add(item);   //staging
                    context.SaveChanges();      //committed
                }
            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int Delete_UnOrderedItems(UnOrderedItem item)
        {
            return Delete_UnOrderedItemById(item.ItemID);
        }
        public int Delete_UnOrderedItemById(int itemid)
        {
            using (var context = new ERaceContext())
            {
                var existing = context.UnOrderedItems.Find(itemid);
                if (existing == null)
                {
                    throw new Exception("UnOrderedItem not on file. Delete unnecessary.");
                }
                else
                {
                    context.UnOrderedItems.Remove(existing);
                    return context.SaveChanges();
                }
            }
        }
    }
}
