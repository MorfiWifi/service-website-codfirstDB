using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace se_CodeFirst_3.Helper
{
    public class NotificationProviderHelper
    {
        private Controller _controller;
        public NotificationProviderHelper(Controller controller /* How to pass controller instance? */)
        {
            _controller = controller;
        }

        public void SuccessfulInsert(string item)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", item + " با موفقیت اضافه شد.");
            _controller.TempData.Add("successful", true);
        }

        public void SuccessfulDelete(string item)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", item + " با موفقیت حذف شد.");
            _controller.TempData.Add("successful", true);
        }

        public void SuccessfulChange(string item)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", item + " با موفقیت تغییر داده شد.");
            _controller.TempData.Add("successful", true);
        }

        public void Failure()
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", "خطا رخ داد.");
            _controller.TempData.Add("successful", false);
        }

        public void FailureInsert(string item)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", "خطا در افزودن " + item);
            _controller.TempData.Add("successful", false);
        }

        public void FailureDelete(string item)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", "خطا در حذف " + item);
            _controller.TempData.Add("successful", false);
        }

        public void FailureChange(string item)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", "خطا در تغییر " + item);
            _controller.TempData.Add("successful", false);
        }

        public void CustomSuccessfulMessage(string message)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", message);
            _controller.TempData.Add("successful", true);
        }

        public void CustomFailureMessage(string message)
        {
            _controller.TempData.Clear();
            _controller.TempData.Add("message", message);
            _controller.TempData.Add("successful", false);
        }


    }


}