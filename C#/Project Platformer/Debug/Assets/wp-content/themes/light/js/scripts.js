$(document).ready(function () {

  //const BASE_DOMAIN = '.setsushi.local';
  const BASE_DOMAIN = '.setsushi.ru';

  window.BASE_DOMAIN = BASE_DOMAIN;
  window.getCookie = getCookie;
  window.setCookie = setCookie;
  window.deleteCookie = deleteCookie;
  window.update_mini_cart = update_mini_cart;

  if (!getCookie('city')) {
    cityDetect();
  }

  update_mini_cart();
  
  if($(window).width() < 768){
    sticky();
  }


  // Убираем старую цену в городах  
  let town = $('.choose-region .selected-region').text();
  if (town == 'Хабаровск' || town == 'Благовещенск' || town == 'Белогорск' || town == 'Биробиджан' || town == 'Москва Сити' || town == 'Комсомольск-на-Амуре'){
      $('.price .old-price').hide();  
  }

  if(typeof getCookie('city') == 'undefined'){
    get_list_regions();
    $('#layer').addClass('loader').fadeIn(100);
  }

  function get_list_regions(){
    $.post("/wp-admin/admin-ajax.php?action=get_list_regions",
      function(data){
        //console.log(data);
        let list_regions = '';
        try{
          $.each(JSON.parse(data), function(key, item){

            let classItem = '';

            if(typeof item.favorite != 'undefined'){
              classItem = ' class="favorite"';
            }else{
              classItem = '';
            }

            if(typeof item.parent == 'undefined'){
              list_regions += '<div class="city"><span data-id="'+ item.id +'"'+classItem+'>'+ item.name +'</span></div>';
            }else{
              list_regions += '<div class="city"><span data-id="'+ item.id +'" data-parent="'+ item.parent +'"'+classItem+'>'+ item.name +'</span></div>';
            }
          });

          let html_regions = '<div class="regions"><div class="title">Выберите город</div><div class="items flex">'+list_regions+'</div></div>';

          $('#general .modal-content').html(html_regions);
        }
        
        catch (e) {
          $('#general .modal-content').html('<div class="err">Не удалось получить список городов.</div>');
          //console.log('Не удалось получить список городов.');
        } 

      }
    );
  }


  // Sticky menu for mobile
  function sticky(){
    if(window.location.pathname != '/korzina/'){
      menu_top = $('.top-menu').offset().top;
      $(window).scroll(function () {
        if ($(window).scrollTop() > menu_top) {
          if (!$('.top-menu').hasClass('sticky')) {
            $('.header .container, .top-menu').addClass('sticky');
          }
        } else {
          // if ($('.top-menu').css('position') == 'fixed') {
            $('.header .container, .top-menu').removeClass('sticky');
          //}
        }
      });
    }
  }

  function checkDetectedCity(city) {
    var city = $('.regions .city').find('span:contains(' + city + ')'),
      result = {
        hasCity: false,
        cityId: 0
      };

    if (city.length) {
      result.hasCity = true;
      result.cityId = city.attr('data-id');
    } else {
      //console.log(city);
    }

    return result;
  }

  function cityPopupDetectedAction(cityPopup, city) {
    cityPopup.find('.yes').removeClass('yes_hidden');
    cityPopup.find('#yes_city').attr('data-city-id', city.cityId);
  }

  // Detect city.
  function cityDetect() {
    $.ajax({
      url: '/wp-admin/admin-ajax.php',
      method: 'POST',
      dataType: 'json',
      data: 'action=city_detect',
      success: function (data) {
          //console.log(data);
        var cityPopup = $('.city-confirm');
        if (data.response && checkDetectedCity(data.data.location.data.city).hasCity) {
          cityPopup.find('.city_ss').text(data.messages.popup_title);
          cityPopup.find('#yes_city').text(data.messages.accept);
          cityPopupDetectedAction(cityPopup, checkDetectedCity(data.data.location.data.city));
        } else {
          if (typeof ymaps != 'undefined') {
            ymaps.ready(function () {
              var city = ymaps.geolocation.city;
          console.log(city);

              if (typeof city != 'undefined' && checkDetectedCity(city).hasCity) {
                cityPopup.find('.city_ss').text('Ваш город ' + city + '?');
                cityPopup.find('#yes_city').text('Да, я в г. ' + city);
                cityPopupDetectedAction(cityPopup, checkDetectedCity(city));
                //console.log(typeof city);
              } else {
                //console.log(typeof city);
              }
            });
          } else {
            //console.log('error yamaps');
          }
        }

        //cityPopup.addClass('city-confirm_opened');
        $('#layer').fadeIn(100);
        $(cityPopup).delay(800).fadeIn(100);
        //console.log(typeof ymaps);
      },
      error: function () {
        console.log('При определении города произошла неопределенная ошибка!');
      }
    });
  }

  function setCookie(name, value, expires, path, domain, secure) {

    domain = domain ? domain : BASE_DOMAIN;
    path = path ? path : "/";

    var comand = encodeURIComponent(name) + "=" + encodeURIComponent(value) +
      ((expires) ? "; expires=" + expires : "") +
      ((path) ? "; path=" + path : "") +
      ((domain) ? "; domain=" + domain : "") +
      ((secure) ? "; secure" : "");

    //console.log(comand);

    document.cookie = comand;

  }

  // возвращает cookie с именем name, если есть, если нет, то undefined
  function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
  }

  function deleteCookie(name, domain) {
    //setCookie(name, "",  -1,"/" );

    domain = domain ? domain : BASE_DOMAIN;

    var path = "/",
      value = "",
      expires = -1;

    var comand = encodeURIComponent(name) + "=" + encodeURIComponent(value) +
      ((expires) ? "; expires=" + expires : "") +
      ((path) ? "; path=" + path : "") +
      ((domain) ? "; domain=" + domain : "");

    document.cookie = comand;
  }

  $("#yes_city").click(function () {
    var date = new Date();
    date.setDate(date.getDate() + 90);
    setCookie("city", $(this).attr('data-city-id'), date.toUTCString(), "/", BASE_DOMAIN);
    change_city();

    //$('.city-confirm').removeClass('city-confirm_opened');
    //$('#region-selection .city span[data-id="' + cityId + '"]').trigger('click');
    //console.log($('#region-selection .city span[data-id="' + cityId + '"]').text());
    return false;

  });

  $(".modal-content").on('click', '.city span', function () {

    //window.location.reload();

      change_city($(this).attr("data-id"));

    $('.popup .close').css('display', 'block');
  });

  function deleteCookie(name) {
    setCookie(name, "", {
      expires: -1
    });
  }

  function change_city(id) {
    $.ajax({
      url: '/wp-admin/admin-ajax.php?action=get_slug_by_id',
      method: 'POST',
      dataType: 'json',
      data: {
        id: id
      },
      success: function (city) {
      var date = new Date();
      date.setDate(date.getDate() + 90);
        try{
          if(typeof(city) != 'undefined'){
            setCookie("city", city.term_id, date.toUTCString(), "/", BASE_DOMAIN);
            setCookie("parent", city.parent, date.toUTCString(), "/", BASE_DOMAIN);
            if(city.slug != 'moskva'){
              window.location.replace(window.location.protocol + '//' + city.slug + BASE_DOMAIN + window.location.pathname);
            }else{
              window.location.replace(window.location.protocol + '//' + BASE_DOMAIN.slice(1) + window.location.pathname);
            }
          }
        }

        catch (e) {
          console.log('Не удалось выполнить редирект.');
        }
      },
      error: function () {
        console.log('Не удалось получить данные города.');
      }
    });
  }

  //Popup
  $('#other-region, .set-other-region, .mobile-icons .icon-city, .change-city').click(function () {
    $('#ask-city').fadeOut(100);

    get_list_regions();

    $('#layer').fadeIn(100).addClass('loader');

    $('#general').delay(300).fadeIn(100);
    $('html').addClass('popup-lock');

    if ($('.mobile-icons .icon-menu span').hasClass('close') && $(window).width() < 768) {
      $('#layer-menu, .choose-region, .phone, .drop-menu div').hide();
      $('.mobile-icons .icon-menu span').removeClass('close');
    }

    if ($(this).hasClass('set-other-region')) {
      $('.popup .close').css('display', 'none');
    }

  });

  $('.popup .close, .basket-popup .close').click(function () {
    $('#layer, .popup, .basket-popup').fadeOut(100);
    $('html').removeClass('popup-lock');
    $('#layer').removeClass('loader');
  });

  // Mobile menu
  $('.mobile-icons .icon-menu span').click(function () {
    $(this).toggleClass("close");
    $('#layer-menu, .phone, .drop-menu div').toggle();

    if ($(window).width() > 576) {
      $('#layer').toggle();
    }

    if ($(this).hasClass('close')) {
      $('.choose-region').css('display', 'flex');
      $('html').addClass('popup-lock');
    } else {
      $('.choose-region').css('display', 'none');
      $('html').removeClass('popup-lock');
    }

  });

  window.onresize = function (event) {
    viewportwidth = $(window).width();
    if (viewportwidth > 768) {
      $('#layer-menu, .choose-region, .phone, .drop-menu div').removeAttr('style');
      $('.other-region').text('Город доставки');
      $('.mobile-icons span').removeClass('close');
      $('.basket-btn').show();

    } else {
      $('.other-region').text('Изменить');
      $('.basket-btn').show();
      sticky();

      if(window.location.pathname == '/korzina/'){
        $('.basket-btn').hide();
      }
    }

    if (viewportwidth < 576) {
      //$('#layer').hide();
    }

  }

  if(window.location.pathname == '/korzina/' && $(window).width() < 768){
    $('.basket-btn').hide();
  } else{
    $('.basket-btn').show();
  }

  if ($(window).width() < 768) {
    $('.other-region').text('Изменить');
  }

  // Call
  $('.phone .number span, .mobile-icons .icon-phone').click(function () {

    if(BASE_DOMAIN == '.setsushi.ru' && typeof(ym) != 'undefined'){
      ym(28942700,'reachGoal','headerPhoneClick');
    }

    
    var dataLayer = window.dataLayer || [];
    dataLayer.push({'event': 'phoneClick'});

    let str = $('.phone .number').text();
    let phone = str.replace(/[^\d]+?/g, '');
    location.href = 'tel:' + phone;
  });

  // Basket
  $('.basket-btn').on('click', 'a', function () {
    if($(this).hasClass('empty')){
      return false;
    }
  });

  // Menu
  $(".top-menu > ul > li, .left-menu ul li").each(function () {
    var link = $(this).children("a").attr("href");
    if (window.location.pathname == link || window.location.pathname + window.location.search == link) {
      $(this).find("a").addClass("active");
    }
  });

  function update_mini_cart(){
    $cook=getCookie('tovar');

    $sum = 0;
    $count = 0;

    if($cook){
      $arr=$cook.split(',');
      $count = $arr.length;
      $sum=0;

      for(var i=0;i< $count;i++){
          var key = $arr[i];
          var price=($arr[i].split("#"));
          $sum+=parseFloat(price[1]);
      }
    }
    
    if(window.location.pathname == '/korzina/' && $sum == 0 && $('h1').text() != 'Заказ отправлен'){
      window.location.href = window.location.origin;
    }

    $('.basket-btn .sum').text($sum);
    $('.basket-btn .count span').text($count);

    setCookie('sum', $sum, "", '/');
    setCookie('count', $count, "", '/');

    if($sum > 0){
      $('.basket-btn a').addClass('show').removeClass('empty');
    } else{
      $('.basket-btn a').addClass('empty').removeClass('show');
    }

  }

  //$("#general, .full-desc, .product").on('click', '.btn-order', function () {
  function animate_sale(object){
    if ($(window).width() > 768) {
      var e = $(object.closest(".card").find("img")),
        t = $(".basket-btn a .count");
      if (e) {
        var a = new Image;
        a.src = e.attr("src");
        $(a).css({
          width: e.width(),
          position: "absolute",
          zIndex: 200,
          top: e.offset().top,
          left: e.offset().left
        });
        $("body").append(a);
        $(a).animate({
          top: t.offset().top,
          left: t.offset().left,
          width: "30px",
          opacity: 0
        }, 600, function() {
          $(a).remove();
        })
      }
    } else {
      return false;
    }
  }
 // });

  $(".full-desc, #general").on('click', '.btn-order-full-product', function () {
    if(BASE_DOMAIN == '.setsushi.ru' && typeof(ym) != 'undefined'){
      ym(28942700,'reachGoal','orderbtn');
    }
    var $this = $(this),
      product_id = $this.attr('data-product-id'),
      product_price = parseFloat($this.attr('data-product-price')),
      cookie_product = getCookie('tovar'),
      city_id = parseInt(getCookie('city')),
      parent_id = parseInt(getCookie('parent')),
      amount_price = 0,
      cart_products = [];

    if (isNaN(product_price))
      product_price = 0;
    if (isNaN(city_id))
      city_id = 0;
    if (isNaN(parent_id))
      city_id = 0;

    if ((typeof cookie_product === 'undefined') || (cookie_product === '')) {
      cart_products.push(1);
      amount_price += product_price;
      setCookie('tovar', product_id + '#' + product_price, '', '/');
    }
    else {
      cookie_product += ',' + product_id + '#' + product_price;
      setCookie('tovar', cookie_product, '', '/');

      cart_products = cookie_product.split(',');
      for (var i = 0; i < cart_products.length; i++) {
        try {
          var price = parseFloat(cart_products[i].split('#')[1]);
          if (isNaN(price))
            price = 0;
          amount_price += price;
          
          animate_sale($(this));
        }
        catch (error) {
          console.error('Не удалось получить сумму продукта', error);
        }
      }
    }

    var dataLayer = window.dataLayer || [];
    var product = $(this).parents('.full-desc');

    // VK Retargeting
    if(typeof(VK) != 'undefined'){
        VK.Retargeting.Event('addToCart');
    }
    
    // Top Mail.ru
    var _tmr = window._tmr || (window._tmr = []);
    _tmr.push({ id: "3214845", type: "reachGoal", goal: "addToCart" });

    dataLayer.push({
      "event": "addToCart",
      "ecommerce": {
        "currencyCode": "RUB",
          "add": {
              "products": [
                  {
                      "id": product.attr('data-id'),
                      "name": product.find('h1').text(),
                      "price": product.find('.price').text(),
                      "category": decodeURIComponent(escape(window.atob(product.attr('data-category')))),
                      "quantity": 1
                  }
              ]
          }
      }
    });

    update_mini_cart();
  });
  
    $('.content').on("click", ".inst-link", function () {
        if(BASE_DOMAIN == '.setsushi.ru' && typeof(ym) != 'undefined'){
          ym(28942700,'reachGoal','inst-link');
        }
    });


  $('.product').on("click", ".btn-order", function () {
    if(BASE_DOMAIN == '.setsushi.ru' && typeof(ym) != 'undefined'){
      ym(28942700,'reachGoal','orderbtn');
    }

    var tovar_str = $(this).attr('data-str');
    if (!tovar_str) alert('Ошибка добавления товара в корзину....');

    var cart_cookie = getCookie('tovar');

    if (!cart_cookie) {
      cart_cookie = tovar_str;
      //console.log(cart_cookie);
    } else {
      cart_cookie += "," + tovar_str;
      //console.log('i tut' + tovar_str);
    }

    setCookie('tovar', cart_cookie, "", '/');

    update_mini_cart();
    animate_sale($(this));

    var dataLayer = window.dataLayer || [];
    var product = $(this).parents('.product');

    // VK Retargeting
    if(typeof(VK) != 'undefined'){
        VK.Retargeting.Event('addToCart');
    }

    // Top Mail.ru
    var _tmr = window._tmr || (window._tmr = []);
    _tmr.push({ id: "3214845", type: "reachGoal", goal: "addToCart" });

    dataLayer.push({
      "event": "addToCart",
      "ecommerce": {
        "currencyCode": "RUB",
          "add": {
              "products": [
                  {
                      "id": product.attr('data-id'),
                      "name": product.find('.title a').text(),
                      "price": product.find('.default-price').text(),
                      "category": decodeURIComponent(escape(window.atob(product.attr('data-category')))),
                      "quantity": 1
                  }
              ]
          }
      }
    });

  });

  $('.btn-order, .plus, .minus, .change-city, .btn-submit').on('touchstart ', function (e) {
    $(this).css({'color':'#fff','background-color':'#c81f40'});
  });

  $('.btn-order, .plus, .minus, .change-city, .btn-submit').on('touchend ', function (e) {
    $(this).css({'color':'#4F4F4F','background-color':'#fff','transition':'all .2s ease'});
  });

  $('.product .image, .product .title a').on('click', function (e) {

    var product = $(this).parents('.product');
    var product_link = $(this).attr('data-href');
    var dataLayer = window.dataLayer || [];
    
    dataLayer.push({
      "event": "productClick",
      "ecommerce": {
          "click": {
            "actionField": {"list": decodeURIComponent(escape(window.atob(product.attr('data-category'))))}, 
            "products": [
              {
                "id": product.attr('data-id'),
                "name" : product.find('.title a').text(),
                "price": product.find('.default-price').text(),
                "category": decodeURIComponent(escape(window.atob(product.attr('data-category')))),
              }
            ]
          }
      }
    });

    if($(window).width() > 768){
      $('#layer, #full-desc').fadeIn(100);
      $('html').addClass('popup-lock');
      $('#layer').addClass('loader');

      $.get(product_link, [], function (data) {
        var $info = $(data).find('#content-product');
        $('#general').find('.modal-content').html($info.html());
        $('#general').fadeIn(100);
      });

      dataLayer.push({
        "ecommerce": {
            "detail": {
              "actionField": {"list": decodeURIComponent(escape(window.atob(product.attr('data-category'))))},
              "products": [
                {
                  "id": product.attr('data-id'),
                  "name" : product.find('.title a').text(),
                  "price": product.find('.default-price').text(),
                  "category": decodeURIComponent(escape(window.atob(product.attr('data-category')))),
                }
              ]
            }
        }
     });

    } else{
      window.location.href = product_link;
    }

  });


});