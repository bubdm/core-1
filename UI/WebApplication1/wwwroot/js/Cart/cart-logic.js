Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        minusFromCartLink: "",
        removeFromCartLink: "",
        clearCartLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);

        $(".add-to-cart").click(Cart.addToCart);

        $(".cart_quantity_up").click(Cart.plusItemInCart);
        $(".cart_quantity_down").click(Cart.minusItemFromCart);
        $(".cart_quantity_delete").click(Cart.removeItemFromCart);
        $(".cart_items_clear").click(Cart.cleatItemsInCart);
    },

    addToCart: function (event) {
        event.preventDefault();

        let button = $(this);
        let id = button.data("id");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(text => {
                Cart.showToolTip(button, text.message);
                Cart.refreshCartView();
            })
            .fail( () => {
                console.log("addToCart fail");
            });
    },

    plusItemInCart: function (event) {
        event.preventDefault();

        let button = $(this);
        let id = button.data("id");

        var tr = button.closest("tr");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done( () => {
                let count = parseInt($(".cart_quantity_input", tr).val());
                $(".cart_quantity_input", tr).val(count + 1);
                Cart.refreshPrice(tr);
                Cart.refreshCartView();
            })
            .fail( () => {
                console.log("plusItemInCart fail");
            });
    },

    minusItemFromCart: function() {
        event.preventDefault();

        let button = $(this);
        let id = button.data("id");

        var tr = button.closest("tr");

        $.get(Cart._properties.minusFromCartLink + "/" + id)
            .done(() => {
                let count = parseInt($(".cart_quantity_input", tr).val());
                if (count > 1) {
                    $(".cart_quantity_input", tr).val(count - 1);
                    Cart.refreshPrice(tr);
                } else {
                    tr.remove();
                    Cart.refreshTotalPrice();
                }
                Cart.refreshCartView();
            })
            .fail( () => {
                console.log("minusItemFromCart fail");
            });
    },

    removeItemFromCart: function() {
        event.preventDefault();

        let button = $(this);
        let id = button.data("id");

        var tr = button.closest("tr");

        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done( () => {
                tr.remove();
                Cart.refreshTotalPrice();
                Cart.refreshCartView();
            }).fail( () => {
                console.log("removeItemFromCart fail");
            });
    },

    cleatItemsInCart: function() {
        event.preventDefault();

        $.get(Cart._properties.clearCartLink)
            .done( () => {
                $(".cart_total_price").closest("tr").each(function() {
                    this.remove();
                });
                Cart.refreshTotalPrice();
                Cart.refreshCartView();
            }).fail( () => {
                console.log("cleatItemsInCart fail");
            });
    },

    showToolTip: function (button, message) {
        button.tooltip({ title: message }).tooltip("show");
        setTimeout( () => {
            button.tooltip("destroy");
        }, 1000);
    },

    refreshCartView: function () {
        var container = $("#cart-container");
        $.get(Cart._properties.getCartViewLink)
            .done( cartHtml => {
                container.html(cartHtml);
            })
            .fail( () => {
                console.log("refreshCartView fail");
            });
    },

    refreshPrice: function (tr) {
        let count = parseInt($(".cart_quantity_input", tr).val());
        let price = parseInt($(".cart_price", tr).data("price"));

        let sumPrice = price * count;
        let value = sumPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });

        let cartTotalPrice = $(".cart_total_price", tr);
        cartTotalPrice.data("price", sumPrice);
        cartTotalPrice.html(value);

        Cart.refreshTotalPrice();
    },

    refreshTotalPrice: function () {
        let totalPrice = 0;

        $(".cart_total_price").each(function () {
            let price = parseFloat($(this).data("price"));
            totalPrice += price;
        });
        let value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });

        $("#total-order-price").html(value);
    }
}