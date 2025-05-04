using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Helpers
{
    public class OrderStatusFilterHelper
    {
        private LinearLayout _pendingLayout, _confirmedLayout,
                             _shippedLayout, _deliveredLayout,
                             _returnLayout, _cancelLayout;
        private Action<string, LinearLayout> _onStatusSelected;

        public OrderStatusFilterHelper(Activity activity, Action<string, LinearLayout> onStatusSelected)
        {
            _pendingLayout = activity.FindViewById<LinearLayout>(Resource.Id.PendingLayout);
            _confirmedLayout = activity.FindViewById<LinearLayout>(Resource.Id.ConfirmedLayout);
            _shippedLayout = activity.FindViewById<LinearLayout>(Resource.Id.ShippedLayout);
            _deliveredLayout = activity.FindViewById<LinearLayout>(Resource.Id.DeliveredLayout);
            _returnLayout = activity.FindViewById<LinearLayout>(Resource.Id.ReturnLayout);
            _cancelLayout = activity.FindViewById<LinearLayout>(Resource.Id.CancelLayout);

            _onStatusSelected = onStatusSelected;

            SetClickListeners();
        }

        private void SetClickListeners()
        {
            _pendingLayout.Click += (s, e) => _onStatusSelected?.Invoke("Pending", _pendingLayout);
            _confirmedLayout.Click += (s, e) => _onStatusSelected?.Invoke("Confirmed", _confirmedLayout);
            _shippedLayout.Click += (s, e) => _onStatusSelected?.Invoke("Shipped", _shippedLayout);
            _deliveredLayout.Click += (s, e) => _onStatusSelected?.Invoke("Delivered", _deliveredLayout);
            _returnLayout.Click += (s, e) => _onStatusSelected?.Invoke("Returned", _returnLayout);
            _cancelLayout.Click += (s, e) => _onStatusSelected?.Invoke("Canceled", _cancelLayout);

        }

        public void SelectDefaultStatus(string defaultStatus)
        {
            switch (defaultStatus)
            {
                case "Pending":
                    _pendingLayout.PerformClick();
                    break;
                case "Confirmed":
                    _confirmedLayout.PerformClick();
                    break;
                case "Shipped":
                    _shippedLayout.PerformClick();
                    break;
                case "Delivered":
                    _deliveredLayout.PerformClick();
                    break;
                case "Canceled":
                    _cancelLayout.PerformClick();
                    break;
                case "Returned":
                    _returnLayout.PerformClick();
                    break;
            }
        }

    }

}
