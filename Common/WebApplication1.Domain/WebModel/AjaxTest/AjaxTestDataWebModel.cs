using System;

namespace WebApplication1.Domain.WebModel.AjaxTest
{
    public class AjaxTestDataWebModel
    {
        public int Id { get;set; }
        public string Message { get; set; }
        public DateTime ServerTime { get; set; } = DateTime.Now;
    }
}
