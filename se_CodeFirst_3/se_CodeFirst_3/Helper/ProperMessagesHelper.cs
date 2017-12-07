using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Helper
{
    public class ProperMessagesHelper
    {
        public string Successful()
        {
            return "با موفقیت انجام شد.";
        }

        public string Failure()
        {
            return "خطا رخ داد.";
        }

        public string SuccessfulInsert(string nameOfNewlyAddedItem)
        {
            return nameOfNewlyAddedItem + " با موفقیت اضافه شد.";
        }

        public string SuccessfulDelete(string nameOfDeletedItem)
        {
            return nameOfDeletedItem + " با موفقیت حذف شد.";
        }

        public string SuccessfulChange(string nameOfChangedItem)
        {
            return nameOfChangedItem + " با موفقیت تغییر داده شد.";
        }

    }
}