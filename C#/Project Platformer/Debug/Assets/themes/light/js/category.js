$(document).ready(function(){

  var dataLayer = window.dataLayer || [];
  var products_cat = [];
  $('.items .product').each(function(i){
    products_cat.push({
      "id": $(this).attr('data-id'),
      "name": $(this).find('.title a').text(),
      "price": $(this).find('.default-price').text(),
      "category": decodeURIComponent(escape(window.atob($(this).attr('data-category')))),
      //"list": decodeURIComponent(escape(window.atob($(this).attr('data-category')))),
      "position": i + 1,
    });
  });

  dataLayer.push({
    "ecommerce": {
      "currencyCode": "RUB",
      "impressions": products_cat
    }
  });

}); 
