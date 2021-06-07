﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public SectionsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var all = _productData.GetSections();

            var parents = all.Where(s => s.ParentId == null);
            var parentsViews = parents.Select(p => new SectionViewModel
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
                    parentView.ChildSections.Add(new SectionViewModel
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
            
            return View(parentsViews);
        }
    }
}