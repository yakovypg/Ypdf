using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace YpdfDesktop.Infrastructure.Collections
{
    public class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        private bool _isNotificationSupressed = false;

        private bool _supressNotifications = false;
        public bool SupressNotifications
        {
            get => _supressNotifications;
            set
            {
                _supressNotifications = value;

                if (!value && _isNotificationSupressed)
                {
                    var action = NotifyCollectionChangedAction.Reset;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));

                    _isNotificationSupressed = false;
                }
            }
        }

        public void AddRange(IEnumerable<T> newItems)
        {
            if (newItems is null)
                return;

            SupressNotifications = true;

            foreach (var item in newItems)
            {
                Items.Add(item);
            }

            SupressNotifications = false;
        }

        public void ReplaceItems(IEnumerable<T>? newItems)
        {
            if (newItems is null)
            {
                Clear();
                return;
            }
            
            SupressNotifications = true;

            Items.Clear();

            foreach (var item in newItems)
            {
                Add(item);
            }

            SupressNotifications = false;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {           
            if (SupressNotifications)
            {
                _isNotificationSupressed = true;
                return;
            }

            base.OnCollectionChanged(e);
        }
    }
}