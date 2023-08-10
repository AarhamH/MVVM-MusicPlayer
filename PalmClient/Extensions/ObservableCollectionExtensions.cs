using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static int RemoveAll<T>(this ObservableCollection<T> observable, Predicate<T> predicate)
        {
            var itemsToRemove = observable.Where(x => predicate.Invoke(x)).ToList();

            foreach (T item in itemsToRemove)
            {
                observable.Remove(item);
            }

            return itemsToRemove.Count;
        }
    }
}
