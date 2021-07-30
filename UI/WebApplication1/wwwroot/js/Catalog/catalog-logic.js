
Catalog = {
    _properties: {
        getViewLink: "",
        getCatalogPagination: ""
    },

    init: properties => {
        $.extend(Catalog._properties, properties);

        $(".pagination li a").click(Catalog.clickOnPage);
    },

    clickOnPage: function(e) {
        e.preventDefault();

        let button = $(this);

        if (button.prop("href").length > 0) {
            let container = $("#catalog-items-container");
            
            let page = button.data("page");

            container.LoadingOverlay("show", { fade: [300, 200] });

            let query = "";

            let data = button.data();

            for (let key in data)
                if (data.hasOwnProperty(key))
                    query += `${key}=${data[key]}&`;

            $.get(Catalog._properties.getViewLink + "?" + query)
                .done(function (catalogHtml) {
                    container.html(catalogHtml);
                    container.LoadingOverlay("hide");

                    $.get(Catalog._properties.getCatalogPagination + "?" + query)
                        .done(function (paginationHtml) {
                            let pagination = $("#catalog-pagination-container");
                            pagination.html(paginationHtml);
                            $(".pagination li a").click(Catalog.clickOnPage);
                        }).fail(function () {
                            console.log("getCatalogPagination fail");
                        });

                }).fail(function () {
                    container.LoadingOverlay("hide");
                    console.log("clickOnPage fail");
                });
        }        
    }

}