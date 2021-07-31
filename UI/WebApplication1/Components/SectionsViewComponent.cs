using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public SectionsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke(string SectionId)
        {
            var sectionId = int.TryParse(SectionId, out var id) ? id : (int?)null;
            int? parentSectionId = null;

            var all = _productData.GetSections();

            var parents = all.Where(s => s.ParentId == null);
            
            var parentsViews = parents.Select(p => new SectionWebModel
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
            }).ToList();

            foreach (var parentView in parentsViews)
            {
                var childs = all.Where(p => p.ParentId == parentView.Id);
                foreach (var child in childs)
                {
                    if (child.Id == sectionId)
                        parentSectionId = child.ParentId;

                    parentView.ChildSections.Add(new SectionWebModel
                    {
                        Id = child.Id,
                        Name = child.Name,
                        Order = child.Order,
                        Parent = parentView,
                    });
                }
                parentView.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parentsViews.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            ViewBag.SectionId = sectionId;
            ViewBag.ParentSectionId = parentSectionId;

            return View(parentsViews);
        }
    }
}
