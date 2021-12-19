$(document).ready(function(){

  var dataLayer = window.dataLayer || [];
  var product = $('.full-desc');
  dataLayer.push({
    "ecommerce": {
        "detail": {
          "actionField": {"list": decodeURIComponent(escape(window.atob(product.attr('data-category'))))},
          "products": [
              {
                "id": product.attr('data-id'),
                "name" : product.find('h1').text(),
                "price": product.find('.price').text(),
                "category": decodeURIComponent(escape(window.atob(product.attr('data-category')))),
              }
          ]
        }
    }
  });

});